using Microsoft.Extensions.Logging;
using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage.Services;

/// <inheritdoc cref="IMessagesRegistryService" />
public partial class MessagesRegistryService
{
    private static partial class Log
    {
        [LoggerMessage(LogLevel.Warning, "The chat with ID '{ChatId}' does not contain a description")]
        public static partial void ChatDoesNotContainDescription(ILogger logger, long chatId);

        [LoggerMessage(LogLevel.Information, "No update required for key '{Key}' with message ID '{MessageId}'")]
        public static partial void NoUpdateRequired(ILogger logger, string key, int messageId);
    }
}