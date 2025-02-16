using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TelegramDataStorage.Configuration;
using TelegramDataStorage.Interfaces;
using TelegramDataStorage.Services;

namespace TelegramDataStorage.Tests.Services;

public class LoadingServiceTests
{
    private readonly Mock<ILogger<LoadingService>> _mockLogger;
    private readonly Mock<ITelegramBotWrapper> _mockBotClient;
    private readonly Mock<IMessagesRegistryService> _mockMessagesRegistryService;
    private readonly Mock<IDataConverter> _mockDataConverter;
    private readonly LoadingService _loadingService;

    public LoadingServiceTests()
    {
        Mock<IOptions<TelegramDataStorageConfig>> mockConfig = new();

        _mockLogger = new Mock<ILogger<LoadingService>>();
        _mockLogger.Setup(l => l.IsEnabled(It.IsAny<LogLevel>())).Returns(true);

        _mockBotClient = new Mock<ITelegramBotWrapper>();
        _mockMessagesRegistryService = new Mock<IMessagesRegistryService>();
        _mockDataConverter = new Mock<IDataConverter>();

        mockConfig.Setup(c => c.Value).Returns(new TelegramDataStorageConfig("asd", 1));

        _loadingService = new LoadingService(
            _mockLogger.Object,
            mockConfig.Object,
            _mockBotClient.Object,
            _mockMessagesRegistryService.Object,
            _mockDataConverter.Object);
    }

    [Fact]
    public async Task LoadAsync_ShouldReturnNull_WhenMessageIdIsNull()
    {
        // Arrange
        _mockMessagesRegistryService
            .Setup(s => s.TryGetAsync(It.IsAny<string>()))
            .ReturnsAsync((int?)null);

        // Act
        var result = await _loadingService.LoadAsync<TestStoredData>();

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
    public async Task LoadAsync_ShouldThrowInvalidOperationException_WhenMessageHasNoFile()
    {
        // Arrange
        const int messageId = 123;

        _mockMessagesRegistryService
            .Setup(s => s.TryGetAsync(It.IsAny<string>()))
            .ReturnsAsync(messageId);

        _mockBotClient
            .Setup(
                b => b.ForwardFileMessageAsync(
                    It.IsAny<long>(),
                    It.IsAny<long>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync((messageId, null));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _loadingService.LoadAsync<TestStoredData>());
    }

    [Fact]
    public async Task LoadAsync_ShouldThrowInvalidOperationException_WhenFileCannotBeDeserialized()
    {
        // Arrange
        const int messageId = 123;
        const string fileId = "file-id";

        _mockMessagesRegistryService
            .Setup(s => s.TryGetAsync(It.IsAny<string>()))
            .ReturnsAsync(messageId);

        _mockBotClient
            .Setup(
                b => b.ForwardFileMessageAsync(
                    It.IsAny<long>(),
                    It.IsAny<long>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync((messageId, fileId));

        _mockBotClient
            .Setup(
                b => b.DownloadFileAsync(
                    It.IsAny<string>(),
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockDataConverter
            .Setup(d => d.Deserialize<TestStoredData>(It.IsAny<byte[]>()))
            .Returns((TestStoredData)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _loadingService.LoadAsync<TestStoredData>());
    }

    [Fact]
    public async Task LoadAsync_ShouldReturnDeserializedObject_WhenFileIsValid()
    {
        // Arrange
        const int messageId = 123;
        const string fileId = "file-id";

        var expectedData = new TestStoredData();

        _mockMessagesRegistryService
            .Setup(s => s.TryGetAsync(It.IsAny<string>()))
            .ReturnsAsync(messageId);

        _mockBotClient
            .Setup(
                b => b.ForwardFileMessageAsync(
                    It.IsAny<long>(),
                    It.IsAny<long>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync((messageId, fileId));

        _mockBotClient
            .Setup(
                b => b.DownloadFileAsync(
                    It.IsAny<string>(),
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()))
            .Callback<string, Stream, CancellationToken>(
                (_, stream, _) =>
                {
                    // Simulate file content download
                    var content = "{\"Key\":\"TestKey\"}"u8.ToArray();
                    stream.Write(content, 0, content.Length);
                    stream.Position = 0;
                })
            .Returns(Task.CompletedTask);

        _mockDataConverter
            .Setup(d => d.Deserialize<TestStoredData>(It.IsAny<byte[]>()))
            .Returns(expectedData);

        // Act
        var result = await _loadingService.LoadAsync<TestStoredData>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedData, result);
    }

    private class TestStoredData : IStoredData
    {
        public static string Key => "TestKey";
    }
}