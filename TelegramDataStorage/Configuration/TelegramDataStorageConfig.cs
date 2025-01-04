namespace TelegramDataStorage.Configuration;

/// <summary>
/// Configuration for the Telegram data storage.
/// </summary>
/// <param name="BotToken">The bot token.</param>
/// <param name="ChatId">The chat ID.</param>
/// <param name="SubscriptionLongPollingTimeout">The subscription long polling timeout. Default is 60 seconds.</param>
public record TelegramDataStorageConfig(string BotToken, long ChatId, int SubscriptionLongPollingTimeout = 60);