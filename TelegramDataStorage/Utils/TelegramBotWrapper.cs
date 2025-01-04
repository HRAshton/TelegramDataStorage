using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage.Utils;

/// <inheritdoc />
public class TelegramBotWrapper(ITelegramBotClient botClient)
    : ITelegramBotWrapper
{
    /// <inheritdoc />
    public Task<Message> SendDocumentAsync(
        ChatId chatId,
        InputFileStream document,
        CancellationToken cancellationToken = default)
    {
        return botClient.SendDocument(chatId, document, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public Task<Message> ForwardMessageAsync(
        ChatId chatId,
        ChatId fromChatId,
        int messageId,
        CancellationToken cancellationToken = default)
    {
        return botClient.ForwardMessage(chatId, fromChatId, messageId, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public Task EditMessageMediaAsync(
        ChatId chatId,
        int messageId,
        InputMediaDocument inputMediaDocument,
        CancellationToken cancellationToken = default)
    {
        return botClient.EditMessageMedia(
            chatId,
            messageId,
            inputMediaDocument,
            cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public Task DeleteMessageAsync(
        ChatId chatId,
        int messageId,
        CancellationToken cancellationToken = default)
    {
        return botClient.DeleteMessage(chatId, messageId, cancellationToken);
    }

    /// <inheritdoc />
    public Task GetInfoAndDownloadFileAsync(
        string fileId,
        Stream destination,
        CancellationToken cancellationToken = default)
    {
        return botClient.GetInfoAndDownloadFile(fileId, destination, cancellationToken);
    }

    /// <inheritdoc />
    public Task<ChatFullInfo> GetChatAsync(
        ChatId chatId,
        CancellationToken cancellationToken = default)
    {
        return botClient.GetChat(chatId, cancellationToken);
    }

    /// <inheritdoc />
    public Task SetChatDescriptionAsync(
        ChatId chatId,
        string description,
        CancellationToken cancellationToken = default)
    {
        return botClient.SetChatDescription(chatId, description, cancellationToken);
    }

    /// <inheritdoc />
    public Task<Update[]> GetUpdatesAsync(
        int? offset,
        int? timeout,
        IEnumerable<UpdateType>? allowedUpdates = null,
        CancellationToken cancellationToken = default)
    {
        return botClient.GetUpdates(
            offset: offset,
            timeout: timeout,
            allowedUpdates: allowedUpdates,
            cancellationToken: cancellationToken);
    }
}