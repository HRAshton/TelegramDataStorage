namespace TelegramDataStorage.Interfaces;

/// <summary>
/// Wrapper for the Telegram bot API.
/// </summary>
public interface ITelegramBotWrapper
{
    /// <summary>
    /// Sends the document to the chat.
    /// </summary>
    /// <param name="chatId">Unique identifier for the target chat.</param>
    /// <param name="filename">Name of the file to send.</param>
    /// <param name="documentStream">File to send.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <returns>Returns the message Id of the sent document.</returns>
    Task<int> SendDocumentAsync(
        long chatId,
        string filename,
        Stream documentStream,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Forwards the message.
    /// </summary>
    /// <param name="chatId">Unique identifier for the target chat.</param>
    /// <param name="fromChatId">Unique identifier for the source chat.</param>
    /// <param name="messageId">Unique identifier for the message to forward.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <returns>Returns the tuple of the message Id and the file Id of the forwarded message.</returns>
    Task<(int MessageId, string? FileId)> ForwardFileMessageAsync(
        long chatId,
        long fromChatId,
        int messageId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Edits the media content of the message.
    /// </summary>
    /// <param name="chatId">Unique identifier for the target chat.</param>
    /// <param name="messageId">Unique identifier for the message to edit.</param>
    /// <param name="filename">New name of the file.</param>
    /// <param name="documentStream">New media content of the message.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task EditMessageMediaAsync(
        long chatId,
        int messageId,
        string filename,
        Stream documentStream,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the message.
    /// </summary>
    /// <param name="chatId">Unique identifier for the target chat.</param>
    /// <param name="messageId">Unique identifier for the message to delete.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteMessageAsync(long chatId, int messageId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads the file to the stream.
    /// </summary>
    /// <param name="fileId">Unique identifier for the file.</param>
    /// <param name="destination">Stream to write the file content to.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DownloadFileAsync(string fileId, Stream destination, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets description of the chat.
    /// </summary>
    /// <param name="chatId">Unique identifier for the chat.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <returns>Information about the chat.</returns>
    public Task<string?> GetChatDescriptionAsync(long chatId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets description of the chat.
    /// </summary>
    /// <param name="chatId">Unique identifier for the target chat.</param>
    /// <param name="description">New description of the chat.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SetChatDescriptionAsync(long chatId, string description, CancellationToken cancellationToken = default);
}