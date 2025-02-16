using Microsoft.Extensions.Logging;
using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage.Core.Services;

/// <inheritdoc cref="ISavingService" />
public partial class SavingService
{
    private static partial class Log
    {
        [LoggerMessage(LogLevel.Warning, "There is no message ID for the key '{Key}'")]
        public static partial void NoMessageIdForKey(ILogger logger, string key);

        [LoggerMessage(LogLevel.Debug, "Updating the entry for the key '{Key}'")]
        public static partial void UpdatingEntryForKey(ILogger logger, string key);

        [LoggerMessage(
            LogLevel.Information,
            "The new entry for the key '{Key}' has been created. Set MessageId to '{MessageId}'")]
        public static partial void NewEntryCreated(ILogger logger, string key, int messageId);
    }
}