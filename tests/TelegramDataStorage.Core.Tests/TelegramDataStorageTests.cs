using Microsoft.Extensions.Logging;
using Moq;
using TelegramDataStorage.Exceptions;
using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage.Core.Tests;

public class TelegramDataStorageTests
{
    private readonly Mock<ILogger<Core.TelegramDataStorage>> _mockLogger;
    private readonly Mock<ISavingService> _mockSavingService;
    private readonly Mock<ILoadingService> _mockLoadingService;
    private readonly Core.TelegramDataStorage _telegramDataStorage;

    public TelegramDataStorageTests()
    {
        _mockLogger = new Mock<ILogger<Core.TelegramDataStorage>>();
        _mockLogger.Setup(l => l.IsEnabled(It.IsAny<LogLevel>())).Returns(true);

        _mockSavingService = new Mock<ISavingService>();
        _mockLoadingService = new Mock<ILoadingService>();
        _telegramDataStorage = new Core.TelegramDataStorage(
            _mockLogger.Object,
            _mockSavingService.Object,
            _mockLoadingService.Object);
    }

    [Fact]
    public async Task SaveAsync_ShouldCallSaveServiceAndLogInformation()
    {
        // Arrange
        var testData = new TestStoredData();

        // Act
        await _telegramDataStorage.SaveAsync(testData);

        // Assert
        _mockSavingService.Verify(s => s.SaveAsync(testData, It.IsAny<CancellationToken>()), Times.Once);
        _mockLogger.Verify(
            l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }

    [Fact]
    public async Task SaveAsync_ShouldLogErrorAndThrowCustomException_OnFailure()
    {
        // Arrange
        var testData = new TestStoredData();
        var exception = new Exception("Test exception");
        _mockSavingService.Setup(s => s.SaveAsync(testData, It.IsAny<CancellationToken>())).ThrowsAsync(exception);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<TelegramDataStorageSaveException>(
            () => _telegramDataStorage.SaveAsync(testData));
        Assert.Equal("Failed to save data", ex.Message);
        Assert.Equal(exception, ex.InnerException);

        _mockLogger.Verify(
            l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                null,
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }

    [Fact]
    public async Task LoadAsync_ShouldCallLoadServiceAndReturnData()
    {
        // Arrange
        var expectedData = new TestStoredData();
        _mockLoadingService
            .Setup(l => l.LoadAsync<TestStoredData>(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedData);

        // Act
        var result = await _telegramDataStorage.LoadAsync<TestStoredData>();

        // Assert
        Assert.Equal(expectedData, result);
        _mockLogger.Verify(
            l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }

    [Fact]
    public async Task LoadAsync_ShouldLogErrorAndThrowCustomException_OnFailure()
    {
        // Arrange
        var exception = new Exception("Test exception");
        _mockLoadingService
            .Setup(l => l.LoadAsync<TestStoredData>(It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<TelegramDataStorageLoadException>(
            () => _telegramDataStorage.LoadAsync<TestStoredData>());
        Assert.Equal("Failed to load data", ex.Message);
        Assert.Equal(exception, ex.InnerException);

        _mockLogger.Verify(
            l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                null,
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }

    private class TestStoredData : IStoredData
    {
        public static string Key => "TestKey";
    }
}