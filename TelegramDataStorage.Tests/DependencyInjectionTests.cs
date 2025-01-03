using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using TelegramDataStorage.Configuration;
using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage.Tests;

public class DependencyInjectionTests
{
    [Fact]
    public void AddTelegramDataStorage_ShouldRegisterAllServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var config = new TelegramDataStorageConfig("0000000000:aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", 123456789);

        // Act
        services.AddLogging();
        services.AddTelegramDataStorage(config);
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        Assert.Null(serviceProvider.GetService<ITelegramBotClient>());
        Assert.Null(serviceProvider.GetService<TelegramBotClient>());

        Assert.NotNull(serviceProvider.GetService<IOptions<TelegramDataStorageConfig>>());
        Assert.NotNull(serviceProvider.GetService<IMessagesRegistryService>());
        Assert.NotNull(serviceProvider.GetService<IDataConverter>());
        Assert.NotNull(serviceProvider.GetService<ILoadingService>());
        Assert.NotNull(serviceProvider.GetService<ISavingService>());
        Assert.NotNull(serviceProvider.GetService<ITelegramBotWrapper>());

        Assert.NotNull(serviceProvider.GetService<ITelegramDataStorage>());
    }

    [Fact]
    public void AddTelegramDataStorage_ShouldThrowArgumentNullException_WhenServicesIsNull()
    {
        // Arrange
        IServiceCollection services = null;
        var config = new TelegramDataStorageConfig("test", 123);

        // Act & Assert
 #pragma warning disable CS8604 // Possible null reference argument.
        Assert.Throws<ArgumentNullException>(() => services.AddTelegramDataStorage(config));
 #pragma warning restore CS8604 // Possible null reference argument.
    }

    [Fact]
    public void AddTelegramDataStorage_ShouldThrowArgumentNullException_WhenConfigIsNull()
    {
        // Arrange
        var services = new ServiceCollection();
        TelegramDataStorageConfig config = null;

        // Act & Assert
 #pragma warning disable CS8604 // Possible null reference argument.
        Assert.Throws<ArgumentNullException>(() => services.AddTelegramDataStorage(config));
 #pragma warning restore CS8604 // Possible null reference argument.
    }
}