using Telegram.Bot.Types;

namespace TelegramDataStorage.Configuration;

/// <summary>
/// Configuration for the Telegram data storage.
/// </summary>
/// <param name="BotToken">The bot token.</param>
/// <param name="ChatId">The chat ID.</param>
public record TelegramDataStorageConfig(string BotToken, long ChatId);