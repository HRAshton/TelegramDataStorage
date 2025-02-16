using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TelegramDataStorage.Core.Configuration;
using TelegramDataStorage.Core.Converters;
using TelegramDataStorage.Core.Interfaces;
using TelegramDataStorage.Core.Services;
using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage.Core;

/// <summary>
/// Contains extension methods for configuring the Telegram data storage services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds the Telegram data storage services to the specified <see cref="IServiceCollection"/>.
    /// Warn: It does not register the <see cref="ITelegramBotWrapper"/> service.
    /// Use other packages for that or implement it yourself.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="config">The configuration of the Telegram data storage.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddTelegramDataStorageCore(
        this IServiceCollection services,
        Action<TelegramDataStorageConfig> config)
    {
        var configuration = new TelegramDataStorageConfig();
        config(configuration);

        return services.AddTelegramDataStorageCore(configuration);
    }

    /// <summary>
    /// Adds the Telegram data storage services to the specified <see cref="IServiceCollection"/>.
    /// Warn: It does not register the <see cref="ITelegramBotWrapper"/> service.
    /// Use other packages for that or implement it yourself.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="config">The configuration of the Telegram data storage.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained
    /// or <see langword="null"/> if <paramref name="services"/> is <see langword="null"/>.</returns>
    public static IServiceCollection AddTelegramDataStorageCore(
        this IServiceCollection? services,
        TelegramDataStorageConfig config)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(config);

        return services
            .AddSingleton(_ => Options.Create(config))
            .AddSingleton<IMessagesRegistryService, MessagesRegistryService>()
            .AddSingleton<IDataConverter, JsonDataConverter>()
            .AddSingleton<ILoadingService, LoadingService>()
            .AddSingleton<ISavingService, SavingService>()
            .AddSingleton<ITelegramDataStorage, TelegramDataStorage>();
    }
}