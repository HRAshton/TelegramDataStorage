using Moq;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using TelegramDataStorage.Utils;
using File = Telegram.Bot.Types.File;

namespace TelegramDataStorage.Tests.Utils;

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
        var chatId = new ChatId(123);
        var document = new InputFileStream(new MemoryStream(), "test.pdf");

        _mockBotClient
            .Setup(b => b.SendRequest(It.IsAny<SendDocumentRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Message());

        // Act
        await _telegramBotWrapper.SendDocumentAsync(chatId, document);

        // Assert
        Assert.Single(_mockBotClient.Invocations);
        Assert.Equal(nameof(ITelegramBotClient.SendRequest), _mockBotClient.Invocations[0].Method.Name);
        var request = Assert.IsType<SendDocumentRequest>(_mockBotClient.Invocations[0].Arguments[0]);
        Assert.Equal(chatId, request.ChatId);
        Assert.Equal(document, request.Document);
    }

    [Fact]
    public async Task ForwardMessageAsync_ShouldPassParametersWithoutChanges()
    {
        // Arrange
        const int messageId = 789;
        var chatId = new ChatId(123);
        var fromChatId = new ChatId(456);

        _mockBotClient
            .Setup(b => b.SendRequest(It.IsAny<ForwardMessageRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Message());

        // Act
        await _telegramBotWrapper.ForwardMessageAsync(chatId, fromChatId, messageId);

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
        const int messageId = 789;
        var chatId = new ChatId(123);
        var inputMediaDocument = new InputMediaDocument(new InputFileStream(new MemoryStream(), "test.pdf"));

        _mockBotClient
            .Setup(b => b.SendRequest(It.IsAny<EditMessageMediaRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Message());

        // Act
        await _telegramBotWrapper.EditMessageMediaAsync(chatId, messageId, inputMediaDocument);

        // Assert
        Assert.Single(_mockBotClient.Invocations);
        Assert.Equal(nameof(ITelegramBotClient.SendRequest), _mockBotClient.Invocations[0].Method.Name);
        var request = Assert.IsType<EditMessageMediaRequest>(_mockBotClient.Invocations[0].Arguments[0]);
        Assert.Equal(chatId, request.ChatId);
        Assert.Equal(messageId, request.MessageId);
        Assert.Equal(inputMediaDocument, request.Media);
    }

    [Fact]
    public async Task DeleteMessageAsync_ShouldPassParametersWithoutChanges()
    {
        // Arrange
        const int messageId = 789;
        var chatId = new ChatId(123);

        _mockBotClient
            .Setup(b => b.SendRequest(It.IsAny<DeleteMessageRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _telegramBotWrapper.DeleteMessageAsync(chatId, messageId);

        // Assert
        Assert.Single(_mockBotClient.Invocations);
        Assert.Equal(nameof(ITelegramBotClient.SendRequest), _mockBotClient.Invocations[0].Method.Name);
        var request = Assert.IsType<DeleteMessageRequest>(_mockBotClient.Invocations[0].Arguments[0]);
        Assert.Equal(chatId, request.ChatId);
        Assert.Equal(messageId, request.MessageId);
    }

    [Fact]
    public async Task GetInfoAndDownloadFileAsync_ShouldPassParametersWithoutChanges()
    {
        // Arrange
        const string fileId = "file-id";
        var destination = new MemoryStream();

        _mockBotClient
            .Setup(b => b.SendRequest(It.IsAny<GetFileRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new File
                {
                    FileId = fileId,
                });

        _mockBotClient
            .Setup(b => b.DownloadFile(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _telegramBotWrapper.GetInfoAndDownloadFileAsync(fileId, destination);

        // Assert
        Assert.Equal(2, _mockBotClient.Invocations.Count);
        Assert.Equal(nameof(ITelegramBotClient.SendRequest), _mockBotClient.Invocations[0].Method.Name);
        var request = Assert.IsType<GetFileRequest>(_mockBotClient.Invocations[0].Arguments[0]);
        Assert.Equal(fileId, request.FileId);

        Assert.Equal(nameof(ITelegramBotClient.DownloadFile), _mockBotClient.Invocations[1].Method.Name);
        Assert.Equal(destination, _mockBotClient.Invocations[1].Arguments[1]);
    }

    [Fact]
    public async Task GetChatAsync_ShouldPassParametersWithoutChanges()
    {
        // Arrange
        var chatId = new ChatId(123);

        _mockBotClient
            .Setup(b => b.SendRequest(It.IsAny<GetChatRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ChatFullInfo());

        // Act
        await _telegramBotWrapper.GetChatAsync(chatId);

        // Assert
        Assert.Single(_mockBotClient.Invocations);
        Assert.Equal(nameof(ITelegramBotClient.SendRequest), _mockBotClient.Invocations[0].Method.Name);
        var request = Assert.IsType<GetChatRequest>(_mockBotClient.Invocations[0].Arguments[0]);
        Assert.Equal(chatId, request.ChatId);
    }
}