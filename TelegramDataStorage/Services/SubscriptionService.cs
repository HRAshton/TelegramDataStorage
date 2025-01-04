using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types.Enums;
using TelegramDataStorage.Configuration;
using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage.Services;

/// <inheritdoc cref="ISubscriptionService" />
public sealed partial class SubscriptionService(
    ILogger<SubscriptionService> logger,
    IOptions<TelegramDataStorageConfig> config,
    ITelegramBotWrapper botClient,
    IMessagesRegistryService messagesRegistryService)
    : ISubscriptionService, IDisposable
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly ConcurrentDictionary<Guid, Action<HashSet<string>>> _subscriptions = [ ];

    private bool _isActive;
    private IDictionary<string, int>? _previousMessageIds;
    private CancellationTokenSource? _listenerCancellationTokenSource;

    /// <inheritdoc />
    public void Dispose()
    {
        Log.Disposed(logger);
        _listenerCancellationTokenSource?.Cancel();
        _semaphore.Dispose();
        _listenerCancellationTokenSource?.Dispose();
    }

    /// <inheritdoc />
    public Guid Subscribe(Action<HashSet<string>> onChanges)
    {
        _semaphore.Wait();
        try
        {
            var subscriptionId = Guid.NewGuid();
            _subscriptions.TryAdd(subscriptionId, onChanges);
            Log.SubscriptionAdded(logger, subscriptionId);
            UpdateStatus();
            return subscriptionId;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <inheritdoc />
    public void Unsubscribe(Guid subscriptionId)
    {
        _semaphore.Wait();
        try
        {
            _subscriptions.TryRemove(subscriptionId, out _);
            Log.SubscriptionRemoved(logger, subscriptionId);
            UpdateStatus();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private void UpdateStatus()
    {
        switch (_isActive)
        {
            case true when _subscriptions.IsEmpty:
                Log.NoSubscriptionsLeft(logger);
                _isActive = false;
                _listenerCancellationTokenSource?.Cancel();
                _listenerCancellationTokenSource = null;
                break;
            case false when !_subscriptions.IsEmpty:
                Log.StartingListener(logger);
                _isActive = true;
                _listenerCancellationTokenSource = new CancellationTokenSource();
                _ = Listen(_listenerCancellationTokenSource.Token);
                break;
        }
    }

    private async Task Listen(CancellationToken cancellationToken)
    {
        var offset = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            var updates = await botClient.GetUpdatesAsync(
                offset,
                timeout: config.Value.SubscriptionLongPollingTimeout,
                allowedUpdates: [ UpdateType.Message ],
                cancellationToken);
            if (updates.Length == 0)
            {
                continue;
            }

            offset = updates.Last().Id + 1;

            var isFromSourceChat = updates.Any(u => u.ChannelPost?.Chat.Id == config.Value.ChatId);
            if (!isFromSourceChat)
            {
                continue;
            }

            await RaiseUpdateAsync();
        }
    }

    private async Task RaiseUpdateAsync()
    {
        ArgumentNullException.ThrowIfNull(_previousMessageIds);

        var newRegistryValues = await messagesRegistryService.ListAsync();

        HashSet<string> keys = [ .._previousMessageIds.Keys, ..newRegistryValues.Keys ];
        var nonMatchingKeys = keys
            .Where(
                k =>
                    _previousMessageIds.TryGetValue(k, out var previousMessageId) !=
                    newRegistryValues.TryGetValue(k, out var newMessageId)
                    || previousMessageId != newMessageId)
            .ToHashSet();

        Log.ChangesDetected(logger);
        foreach (var subscription in _subscriptions)
        {
            subscription.Value(nonMatchingKeys);
        }

        _previousMessageIds = newRegistryValues;
    }
}