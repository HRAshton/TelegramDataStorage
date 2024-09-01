using Microsoft.Extensions.Logging;
using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage.Services;

/// <inheritdoc cref="ILoadingService" />
public partial class LoadingService
{
    private static partial class Log
    {
        [LoggerMessage(LogLevel.Warning, "There is no message ID for the key '{Key}'")]
        public static partial void NoMessageIdForKey(ILogger logger, string key);
    }
}