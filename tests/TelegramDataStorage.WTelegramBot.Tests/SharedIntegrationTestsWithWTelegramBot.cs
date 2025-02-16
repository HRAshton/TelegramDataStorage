using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using TelegramDataStorage.Core.Configuration;
using TelegramDataStorage.IntegrationTests.SharedData;

namespace TelegramDataStorage.WTelegramBot.Tests;

public class SharedIntegrationTestsWithWTelegramBot : SharedIntegrationTests
{
    private readonly ITelegramBotClient _telegramClient;

    public SharedIntegrationTestsWithWTelegramBot()
    {
        _telegramClient = new TelegramBotClient(Config.BotToken);
    }

    protected override Task SetChatDescriptionAsync(string? description)
    {
        return _telegramClient.SetChatDescription(Config.ChatId, description);
    }

    protected override void RegisterServices(
        IServiceCollection services,
        TelegramDataStorageConfig telegramDataStorageConfig)
    {
        services
            .AddLogging()
            .AddTelegramDataStorageForWTelegramBot(telegramDataStorageConfig);
    }

    [Fact]
    protected override Task SendComplexDataAndLoadIt()
    {
        return base.SendComplexDataAndLoadIt();
    }
}