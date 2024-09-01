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
            .Setup(b => b.MakeRequestAsync(It.IsAny<SendDocumentRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Message());

        // Act
        await _telegramBotWrapper.SendDocumentAsync(chatId, document);

        // Assert
        Assert.Single(_mockBotClient.Invocations);
        Assert.Equal(nameof(ITelegramBotClient.MakeRequestAsync), _mockBotClient.Invocations[0].Method.Name);
        Assert.IsType<SendDocumentRequest>(_mockBotClient.Invocations[0].Arguments[0]);

        Assert.Equivalent(
            new SendDocumentRequest(chatId, document),
            (SendDocumentRequest)_mockBotClient.Invocations[0].Arguments[0]);
    }

    [Fact]
    public async Task ForwardMessageAsync_ShouldPassParametersWithoutChanges()
    {
        // Arrange
        const int messageId = 789;
        var chatId = new ChatId(123);
        var fromChatId = new ChatId(456);

        _mockBotClient
            .Setup(b => b.MakeRequestAsync(It.IsAny<ForwardMessageRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Message());

        // Act
        await _telegramBotWrapper.ForwardMessageAsync(chatId, fromChatId, messageId);

        // Assert
        Assert.Single(_mockBotClient.Invocations);
        Assert.Equal(nameof(ITelegramBotClient.MakeRequestAsync), _mockBotClient.Invocations[0].Method.Name);
        Assert.IsType<ForwardMessageRequest>(_mockBotClient.Invocations[0].Arguments[0]);

        Assert.Equivalent(
            new ForwardMessageRequest(chatId, fromChatId, messageId),
            (ForwardMessageRequest)_mockBotClient.Invocations[0].Arguments[0]);
    }

    [Fact]
    public async Task EditMessageMediaAsync_ShouldPassParametersWithoutChanges()
    {
        // Arrange
        const int messageId = 789;
        var chatId = new ChatId(123);
        var inputMediaDocument = new InputMediaDocument(new InputFileStream(new MemoryStream(), "test.pdf"));

        _mockBotClient
            .Setup(b => b.MakeRequestAsync(It.IsAny<EditMessageMediaRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Message());

        // Act
        await _telegramBotWrapper.EditMessageMediaAsync(chatId, messageId, inputMediaDocument);

        // Assert
        Assert.Single(_mockBotClient.Invocations);
        Assert.Equal(nameof(ITelegramBotClient.MakeRequestAsync), _mockBotClient.Invocations[0].Method.Name);
        Assert.IsType<EditMessageMediaRequest>(_mockBotClient.Invocations[0].Arguments[0]);

        Assert.Equivalent(
            new EditMessageMediaRequest(chatId, messageId, inputMediaDocument),
            (EditMessageMediaRequest)_mockBotClient.Invocations[0].Arguments[0]);
    }

    [Fact]
    public async Task DeleteMessageAsync_ShouldPassParametersWithoutChanges()
    {
        // Arrange
        const int messageId = 789;
        var chatId = new ChatId(123);

        _mockBotClient
            .Setup(b => b.MakeRequestAsync(It.IsAny<DeleteMessageRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _telegramBotWrapper.DeleteMessageAsync(chatId, messageId);

        // Assert
        Assert.Single(_mockBotClient.Invocations);
        Assert.Equal(nameof(ITelegramBotClient.MakeRequestAsync), _mockBotClient.Invocations[0].Method.Name);
        Assert.IsType<DeleteMessageRequest>(_mockBotClient.Invocations[0].Arguments[0]);

        Assert.Equivalent(
            new DeleteMessageRequest(chatId, messageId),
            (DeleteMessageRequest)_mockBotClient.Invocations[0].Arguments[0]);
    }

    [Fact]
    public async Task GetInfoAndDownloadFileAsync_ShouldPassParametersWithoutChanges()
    {
        // Arrange
        const string fileId = "file-id";
        var destination = new MemoryStream();

        _mockBotClient
            .Setup(b => b.MakeRequestAsync(It.IsAny<GetFileRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new File { FileId = fileId });

        _mockBotClient
            .Setup(b => b.DownloadFileAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _telegramBotWrapper.GetInfoAndDownloadFileAsync(fileId, destination);

        // Assert
        Assert.Equal(2, _mockBotClient.Invocations.Count);
        Assert.Equal(nameof(ITelegramBotClient.MakeRequestAsync), _mockBotClient.Invocations[0].Method.Name);
        Assert.IsType<GetFileRequest>(_mockBotClient.Invocations[0].Arguments[0]);

        Assert.Equivalent(
            new GetFileRequest(fileId),
            (GetFileRequest)_mockBotClient.Invocations[0].Arguments[0]);

        Assert.Equal(nameof(ITelegramBotClient.DownloadFileAsync), _mockBotClient.Invocations[1].Method.Name);
        Assert.Equal(destination, _mockBotClient.Invocations[1].Arguments[1]);
    }

    [Fact]
    public async Task GetChatAsync_ShouldPassParametersWithoutChanges()
    {
        // Arrange
        var chatId = new ChatId(123);

        _mockBotClient
            .Setup(b => b.MakeRequestAsync(It.IsAny<GetChatRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Chat());

        // Act
        await _telegramBotWrapper.GetChatAsync(chatId);

        // Assert
        Assert.Single(_mockBotClient.Invocations);
        Assert.Equal(nameof(ITelegramBotClient.MakeRequestAsync), _mockBotClient.Invocations[0].Method.Name);
        Assert.IsType<GetChatRequest>(_mockBotClient.Invocations[0].Arguments[0]);

        Assert.Equivalent(
            new GetChatRequest(chatId),
            (GetChatRequest)_mockBotClient.Invocations[0].Arguments[0]);
    }
}