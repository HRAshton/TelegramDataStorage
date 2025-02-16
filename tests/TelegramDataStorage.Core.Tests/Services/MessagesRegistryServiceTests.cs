﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TelegramDataStorage.Core.Configuration;
using TelegramDataStorage.Core.Interfaces;
using TelegramDataStorage.Core.Services;

namespace TelegramDataStorage.Core.Tests.Services;

public class MessagesRegistryServiceTests
{
    private const long ChatId = 0xdeadbeef;
    private readonly Mock<ILogger<MessagesRegistryService>> _mockLogger;
    private readonly Mock<ITelegramBotWrapper> _mockTelegramBotWrapper;
    private readonly MessagesRegistryService _messagesRegistryService;

    public MessagesRegistryServiceTests()
    {
        Mock<IOptions<TelegramDataStorageConfig>> mockConfig = new();

        _mockLogger = new Mock<ILogger<MessagesRegistryService>>();
        _mockLogger.Setup(l => l.IsEnabled(It.IsAny<LogLevel>())).Returns(true);

        _mockTelegramBotWrapper = new Mock<ITelegramBotWrapper>();

        mockConfig
            .Setup(c => c.Value)
            .Returns(new TelegramDataStorageConfig("fake-bot-token", ChatId));

        _messagesRegistryService = new MessagesRegistryService(
            _mockLogger.Object,
            mockConfig.Object,
            _mockTelegramBotWrapper.Object);
    }

    [Fact]
    public async Task TryGetAsync_ShouldReturnMessageId_WhenKeyExists()
    {
        // Arrange
        const string description = "key1,100;key2,200";

        _mockTelegramBotWrapper
            .Setup(b => b.GetChatDescriptionAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(description);

        // Act
        var result = await _messagesRegistryService.TryGetAsync("key2");

        // Assert
        Assert.Equal(200, result);
    }

    [Fact]
    public async Task TryGetAsync_ShouldReturnNull_WhenKeyDoesNotExist()
    {
        // Arrange
        const string description = "key1,100;key2,200";

        _mockTelegramBotWrapper
            .Setup(b => b.GetChatDescriptionAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(description);

        // Act
        var result = await _messagesRegistryService.TryGetAsync("key3");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task TryGetAsync_ShouldReturnNull_WhenDescriptionIsEmpty()
    {
        // Arrange
        const string description = "";

        _mockTelegramBotWrapper
            .Setup(b => b.GetChatDescriptionAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(description);

        // Act
        var result = await _messagesRegistryService.TryGetAsync("key1");

        // Assert
        Assert.Null(result);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task AddOrUpdateAsync_ShouldUpdateRegistry_WhenKeyExists()
    {
        // Arrange
        const string description = "key1,100;key2,200";

        _mockTelegramBotWrapper
            .Setup(b => b.GetChatDescriptionAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(description);

        // Act
        await _messagesRegistryService.AddOrUpdateAsync("key2", 300);

        // Assert
        _mockTelegramBotWrapper.Verify(
            b => b.SetChatDescriptionAsync(
                ChatId,
                "key1,100;key2,300",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task AddOrUpdateAsync_ShouldAddNewEntry_WhenKeyDoesNotExist()
    {
        // Arrange
        const string description = "key1,100;key2,200";

        _mockTelegramBotWrapper
            .Setup(b => b.GetChatDescriptionAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(description);

        // Act
        await _messagesRegistryService.AddOrUpdateAsync("key3", 300);

        // Assert
        _mockTelegramBotWrapper.Verify(
            b =>
                b.SetChatDescriptionAsync(ChatId, "key1,100;key2,200;key3,300", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task AddOrUpdateAsync_ShouldHandleEmptyDescription()
    {
        // Arrange
        const string description = "";

        _mockTelegramBotWrapper
            .Setup(b => b.GetChatDescriptionAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(description);

        // Act
        await _messagesRegistryService.AddOrUpdateAsync("key1", 100);

        // Assert
        _mockTelegramBotWrapper.Verify(
            b =>
                b.SetChatDescriptionAsync(ChatId, "key1,100", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task RetrieveMessagesRegistryAsync_ShouldReturnEmptyDictionary_WhenDescriptionIsNull()
    {
        // Arrange
        const string description = null;

        _mockTelegramBotWrapper
            .Setup(b => b.GetChatDescriptionAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(description);

        // Act
        var result = await _messagesRegistryService.TryGetAsync("key1");

        // Assert
        Assert.Null(result);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}