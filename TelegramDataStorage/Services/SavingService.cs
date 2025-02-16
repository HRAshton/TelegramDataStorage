using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TelegramDataStorage.Configuration;
using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage.Services;

/// <inheritdoc />
public partial class SavingService(
    ILogger<SavingService> logger,
    IOptions<TelegramDataStorageConfig> config,
    ITelegramBotWrapper botClient,
    IMessagesRegistryService messagesRegistryService,
    IDataConverter dataConverter)
    : ISavingService
{
    /// <inheritdoc cref="ISavingService.SaveAsync{T}(T, CancellationToken)" />
    public async Task SaveAsync<T>(T data, CancellationToken cancellationToken = default)
        where T : class, IStoredData, new()
    {
        ArgumentNullException.ThrowIfNull(data);

        if (T.Key.Contains(',') || T.Key.Contains(';'))
        {
            throw new InvalidOperationException("Stored data's key cannot contain ',' or ';'.");
        }

        var messageId = await messagesRegistryService.TryGetAsync(T.Key);
        if (messageId is null)
        {
            Log.NoMessageIdForKey(logger, T.Key);
            await AddNewEntryAsync(data, cancellationToken);
        }
        else
        {
            Log.UpdatingEntryForKey(logger, T.Key);
            await UpdateEntryAsync(data, messageId.Value, cancellationToken);
        }
    }

    private async Task AddNewEntryAsync<T>(T data, CancellationToken cancellationToken)
        where T : IStoredData
    {
        await using var jsonStream = dataConverter.Serialize(data, out var filename);

        var messageId = await botClient.SendDocumentAsync(
            config.Value.ChatId,
            filename,
            jsonStream,
            cancellationToken: cancellationToken);

        await messagesRegistryService.AddOrUpdateAsync(T.Key, messageId);
        Log.NewEntryCreated(logger, T.Key, messageId);
    }

    private async Task UpdateEntryAsync<T>(T data, int messageId, CancellationToken cancellationToken)
        where T : IStoredData
    {
        await using var jsonStream = dataConverter.Serialize(data, out var inputFileStream);
        await botClient.EditMessageMediaAsync(
            config.Value.ChatId,
            messageId,
            inputFileStream,
            jsonStream,
            cancellationToken: cancellationToken);

        await messagesRegistryService.AddOrUpdateAsync(T.Key, messageId);
    }
}