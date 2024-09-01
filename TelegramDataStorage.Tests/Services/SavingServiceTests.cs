using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Telegram.Bot.Types;
using TelegramDataStorage.Configuration;
using TelegramDataStorage.Interfaces;
using TelegramDataStorage.Services;

namespace TelegramDataStorage.Tests.Services;

public class SavingServiceTests
{
    private readonly Mock<ILogger<SavingService>> _mockLogger;
    private readonly Mock<ITelegramBotWrapper> _mockBotClient;
    private readonly Mock<IMessagesRegistryService> _mockMessagesRegistryService;
    private readonly SavingService _savingService;

    public SavingServiceTests()
    {
        Mock<IOptions<TelegramDataStorageConfig>> mockConfig = new();

        _mockLogger = new Mock<ILogger<SavingService>>();
        _mockLogger.Setup(l => l.IsEnabled(It.IsAny<LogLevel>())).Returns(true);

        _mockBotClient = new Mock<ITelegramBotWrapper>();
        _mockMessagesRegistryService = new Mock<IMessagesRegistryService>();
        Mock<IDataConverter> mockDataConverter = new();

        mockConfig.Setup(c => c.Value).Returns(new TelegramDataStorageConfig("asd", 1));

        _savingService = new SavingService(
            _mockLogger.Object,
            mockConfig.Object,
            _mockBotClient.Object,
            _mockMessagesRegistryService.Object,
            mockDataConverter.Object);
    }

    [Fact]
    public async Task SaveAsync_ShouldThrowArgumentNullException_WhenDataIsNull()
    {
        // Arrange & Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _savingService.SaveAsync<TestStoredData>(null));
    }

    [Fact]
    public async Task SaveAsync_ShouldLogWarningAndAddNewEntry_WhenMessageIdIsNull()
    {
        // Arrange
        _mockMessagesRegistryService
            .Setup(s => s.TryGetAsync(It.IsAny<string>()))
            .ReturnsAsync((int?)null);

        _mockBotClient
            .Setup(b => b.SendDocumentAsync(
                It.IsAny<ChatId>(),
                It.IsAny<InputFileStream>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Message
            {
                MessageId = 456,
            });

        // Act
        await _savingService.SaveAsync(new TestStoredData());

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        _mockMessagesRegistryService.Verify(s => s.AddOrUpdateAsync(It.IsAny<string>(), 456), Times.Once);
    }

    [Fact]
    public async Task SaveAsync_ShouldLogDebugAndUpdateEntry_WhenMessageIdIsNotNull()
    {
        // Arrange
        const int messageId = 123;

        _mockMessagesRegistryService
            .Setup(s => s.TryGetAsync(It.IsAny<string>()))
            .ReturnsAsync(messageId);

        _mockBotClient
            .Setup(b => b.EditMessageMediaAsync(
                It.IsAny<ChatId>(),
                It.IsAny<int>(),
                It.IsAny<InputMediaDocument>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _savingService.SaveAsync(new TestStoredData());

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        _mockMessagesRegistryService.Verify(s => s.AddOrUpdateAsync(TestStoredData.Key, messageId), Times.Once);
    }

    [Fact]
    public async Task AddNewEntryAsync_ShouldSendDocumentAndUpdateRegistry()
    {
        // Arrange
        const int messageId = 789;

        _mockBotClient
            .Setup(b => b.SendDocumentAsync(
                It.IsAny<ChatId>(),
                It.IsAny<InputFileStream>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Message
            {
                MessageId = messageId,
            });

        // Act
        await _savingService.SaveAsync(new TestStoredData());

        // Assert
        _mockMessagesRegistryService.Verify(s => s.AddOrUpdateAsync(TestStoredData.Key, messageId), Times.Once);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateEntryAsync_ShouldEditMessageMedia()
    {
        // Arrange
        _mockMessagesRegistryService
            .Setup(s => s.TryGetAsync(It.IsAny<string>()))
            .ReturnsAsync(123);

        _mockBotClient
            .Setup(b => b.EditMessageMediaAsync(
                It.IsAny<ChatId>(),
                It.IsAny<int>(),
                It.IsAny<InputMediaDocument>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _savingService.SaveAsync(new TestStoredData());

        // Assert
        _mockBotClient.Verify(
            b => b.EditMessageMediaAsync(
                It.IsAny<ChatId>(),
                123,
                It.IsAny<InputMediaDocument>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task SaveAsync_ShouldThrowInvalidOperationException_WhenKeyContainsComma()
    {
        // Arrange
        var data = new TestStoredDataWithComma();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _savingService.SaveAsync(data));
    }

    [Fact]
    public async Task SaveAsync_ShouldThrowInvalidOperationException_WhenKeyContainsSemicolon()
    {
        // Arrange
        var data = new TestStoredDataWithSemicolon();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _savingService.SaveAsync(data));
    }

    private class TestStoredData : IStoredData
    {
        public static string Key => "TestKey";
    }

    private class TestStoredDataWithComma : IStoredData
    {
        public static string Key => "TestKey,";
    }

    private class TestStoredDataWithSemicolon : IStoredData
    {
        public static string Key => "TestKey;";
    }
}