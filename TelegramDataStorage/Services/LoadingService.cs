using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TelegramDataStorage.Configuration;
using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage.Services;

/// <inheritdoc />
public partial class LoadingService(
    ILogger<LoadingService> logger,
    IOptions<TelegramDataStorageConfig> config,
    ITelegramBotWrapper botClient,
    IMessagesRegistryService messagesRegistryService,
    IDataConverter dataConverter)
    : ILoadingService
{
    /// <inheritdoc cref="ILoadingService.LoadAsync{T}(CancellationToken)" />
    public async Task<T?> LoadAsync<T>(CancellationToken cancellationToken = default)
        where T : class, IStoredData, new()
    {
        var messageId = await messagesRegistryService.TryGetAsync(T.Key);
        if (messageId is null)
        {
            Log.NoMessageIdForKey(logger, T.Key);
            return null;
        }

        var fileContent = await GetFileContentByMessageIdAsync(messageId.Value, cancellationToken);

        var result = dataConverter.Deserialize<T>(fileContent);
        if (result is null)
        {
            throw new InvalidOperationException("The file does not contain necessary data.");
        }

        return result;
    }

    private async Task<byte[]> GetFileContentByMessageIdAsync(
        int messageId,
        CancellationToken cancellationToken = default)
    {
        var (forwardedMessageId, filePath) = await botClient.ForwardFileMessageAsync(
            config.Value.ChatId,
            config.Value.ChatId,
            messageId,
            cancellationToken: cancellationToken);
        if (filePath is null)
        {
            throw new InvalidOperationException("The message does not contain a file.");
        }

        var fileContent = await DownloadFileAsync(filePath, cancellationToken);

        await botClient.DeleteMessageAsync(config.Value.ChatId, forwardedMessageId, cancellationToken);

        return fileContent;
    }

    private async Task<byte[]> DownloadFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        await using var memoryStream = new MemoryStream();
        await botClient.DownloadFileAsync(filePath, memoryStream, cancellationToken);
        return memoryStream.ToArray();
    }
}