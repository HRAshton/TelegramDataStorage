using System.Globalization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TelegramDataStorage.Configuration;
using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage.Services;

/// <inheritdoc />
public partial class MessagesRegistryService(
    ILogger<MessagesRegistryService> logger,
    IOptions<TelegramDataStorageConfig> config,
    ITelegramBotWrapper telegramBotWrapper)
    : IMessagesRegistryService
{
    /// <inheritdoc cref="IMessagesRegistryService.TryGetAsync" />
    public async Task<int?> TryGetAsync(string key)
    {
        var registry = await RetrieveMessagesRegistryAsync();
        return registry.TryGetValue(key, out var messageId) ? messageId : null;
    }

    /// <inheritdoc cref="IMessagesRegistryService.AddOrUpdateAsync" />
    public async Task AddOrUpdateAsync(string key, int messageId)
    {
        var registry = await RetrieveMessagesRegistryAsync();

        if (registry.TryGetValue(key, out var currentMessageId)
            && currentMessageId == messageId)
        {
            Log.NoUpdateRequired(logger, key, messageId);
            return;
        }

        registry[key] = messageId;

        var description = string.Join(';', registry.Select(pair => $"{pair.Key},{pair.Value}"));
        await telegramBotWrapper.SetChatDescriptionAsync(config.Value.ChatId, description);
    }

    private async Task<IDictionary<string, int>> RetrieveMessagesRegistryAsync()
    {
        var description = await telegramBotWrapper.GetChatDescriptionAsync(config.Value.ChatId);
        if (description is null or "")
        {
            Log.ChatDoesNotContainDescription(logger, config.Value.ChatId);
            return new Dictionary<string, int>();
        }

        var registry = description.Split(';')
            .Select(line => line.Split(','))
            .ToDictionary(parts => parts[0], parts => int.Parse(parts[1], CultureInfo.InvariantCulture));

        return registry;
    }
}