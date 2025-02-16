namespace TelegramDataStorage.Core.Configuration;

/// <summary>
/// Configuration for the Telegram data storage.
/// </summary>
public record TelegramDataStorageConfig
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TelegramDataStorageConfig"/> class.
    /// </summary>
    public TelegramDataStorageConfig()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TelegramDataStorageConfig"/> class.
    /// </summary>
    /// <param name="botToken">The bot token.</param>
    /// <param name="chatId">The chat ID.</param>
    public TelegramDataStorageConfig(string botToken, long chatId)
    {
        BotToken = botToken;
        ChatId = chatId;
    }

    /// <summary>
    /// Gets or sets the bot token.
    /// </summary>
    public string BotToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the chat ID.
    /// </summary>
    public long ChatId { get; set; }
}