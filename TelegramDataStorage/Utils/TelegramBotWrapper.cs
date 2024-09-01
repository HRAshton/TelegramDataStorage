using Telegram.Bot;
using Telegram.Bot.Types;
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
        return botClient.SendDocumentAsync(chatId, document, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public Task<Message> ForwardMessageAsync(
        ChatId chatId,
        ChatId fromChatId,
        int messageId,
        CancellationToken cancellationToken = default)
    {
        return botClient.ForwardMessageAsync(chatId, fromChatId, messageId, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public Task EditMessageMediaAsync(
        ChatId chatId,
        int messageId,
        InputMediaDocument inputMediaDocument,
        CancellationToken cancellationToken = default)
    {
        return botClient.EditMessageMediaAsync(
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
        return botClient.DeleteMessageAsync(chatId, messageId, cancellationToken);
    }

    /// <inheritdoc />
    public Task GetInfoAndDownloadFileAsync(
        string fileId,
        Stream destination,
        CancellationToken cancellationToken = default)
    {
        return botClient.GetInfoAndDownloadFileAsync(fileId, destination, cancellationToken);
    }

    /// <inheritdoc />
    public Task<Chat> GetChatAsync(
        ChatId chatId,
        CancellationToken cancellationToken = default)
    {
        return botClient.GetChatAsync(chatId, cancellationToken);
    }

    /// <inheritdoc />
    public Task SetChatDescriptionAsync(
        ChatId chatId,
        string description,
        CancellationToken cancellationToken = default)
    {
        return botClient.SetChatDescriptionAsync(chatId, description, cancellationToken);
    }
}