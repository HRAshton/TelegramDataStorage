using Microsoft.Extensions.Logging;
using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage.Services;

/// <inheritdoc cref="ISubscriptionService" />
public partial class SubscriptionService
{
    private static partial class Log
    {
        [LoggerMessage(LogLevel.Information, "Subscription added: {key}")]
        public static partial void SubscriptionAdded(ILogger logger, Guid key);

        [LoggerMessage(LogLevel.Information, "Subscription removed: {key}")]
        public static partial void SubscriptionRemoved(ILogger logger, Guid key);

        [LoggerMessage(LogLevel.Information, "Starting listener")]
        public static partial void StartingListener(ILogger logger);

        [LoggerMessage(LogLevel.Information, "No subscriptions left, stopping listener")]
        public static partial void NoSubscriptionsLeft(ILogger logger);

        [LoggerMessage(LogLevel.Information, "Changes detected, notifying subscribers")]
        public static partial void ChangesDetected(ILogger logger);

        [LoggerMessage(LogLevel.Information, "SubscriptionService disposed")]
        public static partial void Disposed(ILogger logger);
    }
}