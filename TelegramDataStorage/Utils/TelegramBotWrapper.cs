using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage.Utils;

/// <inheritdoc />
public class TelegramBotWrapper(ITelegramBotClient botClient)
    : ITelegramBotWrapper
{
    /// <inheritdoc />
    public Task<int> SendDocumentAsync(
        long chatId,
        string filename,
        Stream documentStream,
        CancellationToken cancellationToken = default)
    {
        return botClient.SendDocument(
                chatId,
                InputFile.FromStream(documentStream, filename),
                cancellationToken: cancellationToken)
            .ContinueWith(task => task.Result.Id, cancellationToken);
    }

    /// <inheritdoc />
    public Task<(int MessageId, string? FileId)> ForwardFileMessageAsync(
        long chatId,
        long fromChatId,
        int messageId,
        CancellationToken cancellationToken = default)
    {
        return botClient.ForwardMessage(
                chatId,
                fromChatId,
                messageId,
                cancellationToken: cancellationToken)
            .ContinueWith(task => (task.Result.Id, task.Result.Document?.FileId), cancellationToken);
    }

    /// <inheritdoc />
    public Task EditMessageMediaAsync(
        long chatId,
        int messageId,
        string filename,
        Stream documentStream,
        CancellationToken cancellationToken = default)
    {
        return botClient.EditMessageMedia(
            chatId,
            messageId,
            new InputMediaDocument(InputFile.FromStream(documentStream, filename)),
            cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public Task DeleteMessageAsync(long chatId, int messageId, CancellationToken cancellationToken = default)
    {
        return botClient.DeleteMessage(chatId, messageId, cancellationToken);
    }

    /// <inheritdoc />
    public Task DownloadFileAsync(string fileId, Stream destination, CancellationToken cancellationToken = default)
    {
        return botClient.GetInfoAndDownloadFile(fileId, destination, cancellationToken);
    }

    /// <inheritdoc />
    public Task<string?> GetChatDescriptionAsync(long chatId, CancellationToken cancellationToken = default)
    {
        return botClient.GetChat(chatId, cancellationToken)
            .ContinueWith(task => task.Result.Description, cancellationToken);
    }

    /// <inheritdoc />
    public Task SetChatDescriptionAsync(long chatId, string description, CancellationToken cancellationToken = default)
    {
        return botClient.SetChatDescription(chatId, description, cancellationToken);
    }
}