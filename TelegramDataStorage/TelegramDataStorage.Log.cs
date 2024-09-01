using Microsoft.Extensions.Logging;
using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage;

/// <inheritdoc cref="ITelegramDataStorage" />
public partial class TelegramDataStorage
{
    private static partial class Log
    {
        [LoggerMessage(LogLevel.Debug, "Saving data for '{Key}'...")]
        public static partial void SavingDataForKey(ILogger logger, string key);

        [LoggerMessage(LogLevel.Information, "Data for '{Key}' has been saved")]
        public static partial void DataSavedForKey(ILogger logger, string key);

        [LoggerMessage(LogLevel.Error, "Failed to save data for '{Key}'")]
        public static partial void FailedToSaveDataForKey(ILogger logger, string key);

        [LoggerMessage(LogLevel.Information, "Loading data for '{Key}'...")]
        public static partial void LoadingDataForKey(ILogger logger, string key);

        [LoggerMessage(LogLevel.Error, "Failed to load data for '{Key}'")]
        public static partial void FailedToLoadDataForKey(ILogger logger, string key);
    }
}