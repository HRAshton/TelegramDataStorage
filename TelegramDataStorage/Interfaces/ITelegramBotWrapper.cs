using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramDataStorage.Interfaces;

/// <summary>
/// Wrapper for the Telegram bot API.
/// </summary>
public interface ITelegramBotWrapper
{
    /// <summary>
    /// Wraps the <see cref="Telegram.Bot.TelegramBotClientExtensions.SendDocument" /> method.
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
    /// Wraps the <see cref="Telegram.Bot.TelegramBotClientExtensions.ForwardMessage" /> method.
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
    /// Wraps the "Telegram.Bot.TelegramBotClientExtensions.EditMessageMedia" method.
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
    /// Wraps the <see cref="Telegram.Bot.TelegramBotClientExtensions.DeleteMessage" /> method.
    /// </summary>
    /// <param name="chatId">Unique identifier for the target chat.</param>
    /// <param name="messageId">Unique identifier for the message to delete.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteMessageAsync(ChatId chatId, int messageId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Wraps the <see cref="Telegram.Bot.TelegramBotClientExtensions.GetFile" /> method.
    /// </summary>
    /// <param name="fileId">Unique identifier for the file.</param>
    /// <param name="destination">Stream to write the file content to.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task GetInfoAndDownloadFileAsync(string fileId, Stream destination, CancellationToken cancellationToken = default);

    /// <summary>
    /// Wraps the <see cref="Telegram.Bot.TelegramBotClientExtensions.GetChat" /> method.
    /// </summary>
    /// <param name="chatId">Unique identifier for the chat.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <returns>Information about the chat.</returns>
    public Task<ChatFullInfo> GetChatAsync(ChatId chatId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Wraps the <see cref="Telegram.Bot.TelegramBotClientExtensions.SetChatDescription" /> method.
    /// </summary>
    /// <param name="chatId">Unique identifier for the target chat.</param>
    /// <param name="description">New description of the chat.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SetChatDescriptionAsync(ChatId chatId, string description, CancellationToken cancellationToken = default);

    /// <summary>
    /// Wraps the <see cref="Telegram.Bot.TelegramBotClientExtensions.GetUpdates" /> method.
    /// </summary>
    /// <param name="offset">Identifier of the first update to be returned.</param>
    /// <param name="timeout">Timeout in seconds for long polling.</param>
    /// <param name="allowedUpdates">List of the update types you want your bot to receive.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <returns>Array of <see cref="Update" /> objects.</returns>
    Task<Update[]> GetUpdatesAsync(
        int? offset,
        int? timeout,
        IEnumerable<UpdateType>? allowedUpdates = null,
        CancellationToken cancellationToken = default);
}