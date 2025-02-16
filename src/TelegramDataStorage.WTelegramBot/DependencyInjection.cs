using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using TelegramDataStorage.Core;
using TelegramDataStorage.Core.Configuration;
using TelegramDataStorage.Core.Interfaces;

namespace TelegramDataStorage.WTelegramBot;

/// <summary>
/// Contains extension methods for configuring the Telegram data storage services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds the Telegram data storage services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="config">The configuration of the Telegram data storage.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddTelegramDataStorageForWTelegramBot(
        this IServiceCollection services,
        Action<TelegramDataStorageConfig> config)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(config);

        var configuration = new TelegramDataStorageConfig();
        config(configuration);

        return services.AddTelegramDataStorageForWTelegramBot(configuration);
    }

    /// <summary>
    /// Adds the Telegram data storage services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="config">The configuration of the Telegram data storage.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddTelegramDataStorageForWTelegramBot(
        this IServiceCollection services,
        TelegramDataStorageConfig config)
    {
        ArgumentNullException.ThrowIfNull(services);

        return services
            .AddTelegramDataStorageCore(config)
            .AddSingleton<ITelegramBotClient>(new TelegramBotClient(config.BotToken))
            .AddSingleton<ITelegramBotWrapper, WTelegramBotWrapper>();
    }
}