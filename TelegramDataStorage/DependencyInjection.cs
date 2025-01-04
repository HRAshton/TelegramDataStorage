using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using TelegramDataStorage.Configuration;
using TelegramDataStorage.Converters;
using TelegramDataStorage.Interfaces;
using TelegramDataStorage.Services;
using TelegramDataStorage.Utils;

namespace TelegramDataStorage;

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
    public static IServiceCollection AddTelegramDataStorage(
        this IServiceCollection services,
        TelegramDataStorageConfig config)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(config);

        return services
            .AddSingleton<IOptions<TelegramDataStorageConfig>>(new OptionsWrapper<TelegramDataStorageConfig>(config))
            .AddSingleton<IMessagesRegistryService, MessagesRegistryService>()
            .AddSingleton<IDataConverter, JsonDataConverter>()
            .AddSingleton<ILoadingService, LoadingService>()
            .AddSingleton<ISavingService, SavingService>()
            .AddSingleton<ITelegramDataStorage, TelegramDataStorage>()
            .AddSingleton<ISubscriptionService, SubscriptionService>()
            .AddSingleton<ITelegramBotWrapper>(new TelegramBotWrapper(new TelegramBotClient(config.BotToken)));
    }
}