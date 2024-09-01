using Telegram.Bot.Types;

namespace TelegramDataStorage.Interfaces;

/// <summary>
/// Wrapper for the Telegram bot API.
/// </summary>
public interface ITelegramBotWrapper
{
    /// <summary>
    /// Wraps the <see cref="Telegram.Bot.TelegramBotClientExtensions.SendDocumentAsync" /> method.
    /// </summary>
    /// <param name="chatId">Unique identifier for the target chat.</param>
    /// <param name="document">File to send.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <returns>Information about the sent message.</returns>
    Task<Message> SendDocumentAsync(
        ChatId chatId,
        InputFileStream document,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Wraps the <see cref="Telegram.Bot.TelegramBotClientExtensions.ForwardMessageAsync" /> method.
    /// </summary>
    /// <param name="chatId">Unique identifier for the target chat.</param>
    /// <param name="fromChatId">Unique identifier for the source chat.</param>
    /// <param name="messageId">Unique identifier for the message to forward.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <returns>Information about the sent message.</returns>
    Task<Message> ForwardMessageAsync(
        ChatId chatId,
        ChatId fromChatId,
        int messageId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Wraps the "Telegram.Bot.TelegramBotClientExtensions.EditMessageMediaAsync" method.
    /// </summary>
    /// <param name="chatId">Unique identifier for the target chat.</param>
    /// <param name="messageId">Unique identifier for the message to edit.</param>
    /// <param name="inputMediaDocument">New media content of the message.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task EditMessageMediaAsync(
        ChatId chatId,
        int messageId,
        InputMediaDocument inputMediaDocument,
        CancellationToken cancellationToken);

    /// <summary>
    /// Wraps the <see cref="Telegram.Bot.TelegramBotClientExtensions.DeleteMessageAsync" /> method.
    /// </summary>
    /// <param name="chatId">Unique identifier for the target chat.</param>
    /// <param name="messageId">Unique identifier for the message to delete.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteMessageAsync(ChatId chatId, int messageId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Wraps the <see cref="Telegram.Bot.TelegramBotClientExtensions.GetFileAsync" /> method.
    /// </summary>
    /// <param name="fileId">Unique identifier for the file.</param>
    /// <param name="destination">Stream to write the file content to.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task GetInfoAndDownloadFileAsync(string fileId, Stream destination, CancellationToken cancellationToken = default);

    /// <summary>
    /// Wraps the <see cref="Telegram.Bot.TelegramBotClientExtensions.GetChatAsync" /> method.
    /// </summary>
    /// <param name="chatId">Unique identifier for the chat.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <returns>Information about the chat.</returns>
    public Task<Chat> GetChatAsync(ChatId chatId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Wraps the <see cref="Telegram.Bot.TelegramBotClientExtensions.SetChatDescriptionAsync" /> method.
    /// </summary>
    /// <param name="chatId">Unique identifier for the target chat.</param>
    /// <param name="description">New description of the chat.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SetChatDescriptionAsync(ChatId chatId, string description, CancellationToken cancellationToken = default);
}