using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using TelegramDataStorage.Core.Configuration;
using TelegramDataStorage.Core.Interfaces;
using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage.Core.Tests;

public class DependencyInjectionTests
{
    [Fact]
    public void AddTelegramDataStorage_ConfigAsModel_ShouldRegisterAllServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var config = new TelegramDataStorageConfig("0000000000:aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", 123456789);
        var telegramBotWrapper = Mock.Of<ITelegramBotWrapper>();
        services.AddLogging();

        // Act
        services.AddTelegramDataStorageCore(config);
        services.AddSingleton(telegramBotWrapper);
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetService<IOptions<TelegramDataStorageConfig>>());
        Assert.NotNull(serviceProvider.GetService<IMessagesRegistryService>());
        Assert.NotNull(serviceProvider.GetService<IDataConverter>());
        Assert.NotNull(serviceProvider.GetService<ILoadingService>());
        Assert.NotNull(serviceProvider.GetService<ISavingService>());

        Assert.NotNull(serviceProvider.GetService<ITelegramDataStorage>());
        var registeredWrapper = Assert.Single(serviceProvider.GetServices<ITelegramBotWrapper>());
        Assert.Same(telegramBotWrapper, registeredWrapper);
    }

    [Fact]
    public void AddTelegramDataStorage_ConfigAsAction_ShouldRegisterAllServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var telegramBotWrapper = Mock.Of<ITelegramBotWrapper>();
        services.AddLogging();
        services.AddSingleton(telegramBotWrapper);

        // Act
        services.AddTelegramDataStorageCore(
            c =>
            {
                c.BotToken = "0000000000:aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                c.ChatId = 123456789;
            });
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetService<IOptions<TelegramDataStorageConfig>>());
        Assert.NotNull(serviceProvider.GetService<IMessagesRegistryService>());
        Assert.NotNull(serviceProvider.GetService<IDataConverter>());
        Assert.NotNull(serviceProvider.GetService<ILoadingService>());
        Assert.NotNull(serviceProvider.GetService<ISavingService>());

        Assert.NotNull(serviceProvider.GetService<ITelegramDataStorage>());
        var registeredWrapper = Assert.Single(serviceProvider.GetServices<ITelegramBotWrapper>());
        Assert.Same(telegramBotWrapper, registeredWrapper);
    }

    [Fact]
    public void AddTelegramDataStorage_ShouldThrowArgumentNullException_WhenServicesIsNull()
    {
        // Arrange
        IServiceCollection services = null;
        var config = new TelegramDataStorageConfig("test", 123);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => services.AddTelegramDataStorageCore(config));
    }

    [Fact]
    public void AddTelegramDataStorage_ShouldThrowArgumentNullException_WhenConfigIsNull()
    {
        // Arrange
        var services = new ServiceCollection();
        TelegramDataStorageConfig config = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => services.AddTelegramDataStorageCore(config!));
    }
}