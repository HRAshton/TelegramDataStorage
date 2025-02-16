using Moq;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace TelegramDataStorage.TelegramBot.Tests;

public class TelegramBotWrapperTests
{
    private readonly Mock<ITelegramBotClient> _mockBotClient;
    private readonly TelegramBotWrapper _telegramBotWrapper;

    public TelegramBotWrapperTests()
    {
        _mockBotClient = new Mock<ITelegramBotClient>();
        _telegramBotWrapper = new TelegramBotWrapper(_mockBotClient.Object);
    }

    [Fact]
    public async Task SendDocumentAsync_ShouldPassParametersWithoutChanges()
    {
        // Arrange
        const int chatId = 123;
        const string filename = "test.pdf";
        await using var documentStream = Mock.Of<Stream>();

        _mockBotClient
            .Setup(b => b.SendRequest(It.IsAny<SendDocumentRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Message());

        // Act
        await _telegramBotWrapper.SendDocumentAsync(chatId, filename, documentStream);

        // Assert
        var invocation = Assert.Single(_mockBotClient.Invocations);
        Assert.Equal(nameof(ITelegramBotClient.SendRequest), invocation.Method.Name);

        var request = Assert.IsType<SendDocumentRequest>(invocation.Arguments[0]);
        Assert.Equal(chatId, request.ChatId);

        var fileStream = Assert.IsType<InputFileStream>(request.Document);
        Assert.Equal(filename, fileStream.FileName);
        Assert.Equal(documentStream, fileStream.Content);
    }

    [Fact]
    public async Task ForwardFileMessageAsync_ShouldPassParametersWithoutChanges()
    {
        // Arrange
        const int chatId = 123;
        const int messageId = 789;
        const int fromChatId = 456;

        _mockBotClient
            .Setup(b => b.SendRequest(It.IsAny<ForwardMessageRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Message());

        // Act
        await _telegramBotWrapper.ForwardFileMessageAsync(chatId, fromChatId, messageId);

        // Assert
        Assert.Single(_mockBotClient.Invocations);
        Assert.Equal(nameof(ITelegramBotClient.SendRequest), _mockBotClient.Invocations[0].Method.Name);
        var request = Assert.IsType<ForwardMessageRequest>(_mockBotClient.Invocations[0].Arguments[0]);
        Assert.Equal(chatId, request.ChatId);
        Assert.Equal(fromChatId, request.FromChatId);
        Assert.Equal(messageId, request.MessageId);
    }

    [Fact]
    public async Task EditMessageMediaAsync_ShouldPassParametersWithoutChanges()
    {
        // Arrange
        const int chatId = 123;
        const int messageId = 789;
        const string filename = "test.pdf";
        await using var documentStream = Mock.Of<Stream>();

        _mockBotClient
            .Setup(b => b.SendRequest(It.IsAny<EditMessageMediaRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Message());

        // Act
        await _telegramBotWrapper.EditMessageMediaAsync(chatId, messageId, filename, documentStream);

        // Assert
        var invocation = Assert.Single(_mockBotClient.Invocations);
        Assert.Equal(nameof(ITelegramBotClient.SendRequest), invocation.Method.Name);

        var request = Assert.IsType<EditMessageMediaRequest>(invocation.Arguments[0]);
        Assert.Equal(chatId, request.ChatId);
        Assert.Equal(messageId, request.MessageId);

        var mediaDocument = Assert.IsType<InputMediaDocument>(request.Media);
        var fileStream = Assert.IsType<InputFileStream>(mediaDocument.Media);
        Assert.Equal(filename, fileStream.FileName);
        Assert.Equal(documentStream, fileStream.Content);
    }

    [Fact]
    public async Task DeleteMessageAsync_ShouldPassParametersWithoutChanges()
    {
        // Arrange
        const int chatId = 123;
        const int messageId = 789;

        _mockBotClient
            .Setup(b => b.SendRequest(It.IsAny<DeleteMessageRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _telegramBotWrapper.DeleteMessageAsync(chatId, messageId);

        // Assert
        var invocation = Assert.Single(_mockBotClient.Invocations);
        Assert.Equal(nameof(ITelegramBotClient.SendRequest), invocation.Method.Name);
        var request = Assert.IsType<DeleteMessageRequest>(invocation.Arguments[0]);
        Assert.Equal(chatId, request.ChatId);
        Assert.Equal(messageId, request.MessageId);
    }

    [Fact]
    public async Task DownloadFileAsync_ShouldPassParametersWithoutChanges()
    {
        // Arrange
        const string fileId = "file-id";
        await using var destination = Mock.Of<Stream>();

        _mockBotClient
            .Setup(b => b.SendRequest(It.IsAny<GetFileRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new TGFile
                {
                    FileId = fileId,
                });

        _mockBotClient
            .Setup(b => b.DownloadFile(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _telegramBotWrapper.DownloadFileAsync(fileId, destination);

        // Assert
        Assert.Equal(2, _mockBotClient.Invocations.Count);

        Assert.Equal(nameof(ITelegramBotClient.SendRequest), _mockBotClient.Invocations[0].Method.Name);
        var fileRequest = Assert.IsType<GetFileRequest>(_mockBotClient.Invocations[0].Arguments[0]);
        Assert.Equal(fileId, fileRequest.FileId);

        Assert.Equal(nameof(ITelegramBotClient.DownloadFile), _mockBotClient.Invocations[1].Method.Name);
        Assert.Equal(destination, _mockBotClient.Invocations[1].Arguments[1]);
    }

    [Fact]
    public async Task GetChatDescriptionAsync_ShouldPassParametersWithoutChanges()
    {
        // Arrange
        const int chatId = 123;

        _mockBotClient
            .Setup(b => b.SendRequest(It.IsAny<GetChatRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ChatFullInfo());

        // Act
        await _telegramBotWrapper.GetChatDescriptionAsync(chatId);

        // Assert
        Assert.Single(_mockBotClient.Invocations);
        Assert.Equal(nameof(ITelegramBotClient.SendRequest), _mockBotClient.Invocations[0].Method.Name);
        var request = Assert.IsType<GetChatRequest>(_mockBotClient.Invocations[0].Arguments[0]);
        Assert.Equal(chatId, request.ChatId);
    }
}