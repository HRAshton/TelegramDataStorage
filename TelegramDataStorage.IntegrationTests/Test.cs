using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using TelegramDataStorage.Configuration;
using TelegramDataStorage.IntegrationTests.Models;
using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage.IntegrationTests;

/// <summary>
/// Integration tests for the Telegram data storage.
/// </summary>
public class Test
{
    /// <summary>
    /// Tests the sending of complex data and loading it.
    /// Workflow:
    /// 1. Clears the chat description.
    /// 2. Checks that the library cannot load the data.
    /// 3. Sends nested data and bytes data.
    /// 4. Loads the data.
    /// 5. Checks that the loaded data is equal to the expected data.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task SendComplexDataAndLoadIt()
    {
        var config = GetConfig();
        var telegramClient = new TelegramBotClient(config.BotToken);
        await telegramClient.SetChatDescriptionAsync(config.ChatId);

        var service = GetService(config);
        var (expectedNestedData, expectedBytesData) = GenerateData();

        var nestedData = await service.LoadAsync<NestedData>();
        var bytesData = await service.LoadAsync<BytesData>();
        Assert.Null(nestedData);
        Assert.Null(bytesData);

        await service.SaveAsync(expectedNestedData);
        await service.SaveAsync(expectedBytesData);
        nestedData = await service.LoadAsync<NestedData>();
        bytesData = await service.LoadAsync<BytesData>();

        Assert.NotNull(nestedData);
        Assert.NotNull(bytesData);
        Assert.Equivalent(expectedNestedData, nestedData);
        Assert.Equivalent(expectedBytesData, bytesData);
    }

    private static TelegramDataStorageConfig GetConfig()
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var telegramDataStorageConfig = config
            .GetRequiredSection("TelegramDataStorage")
            .Get<TelegramDataStorageConfig>();

        return telegramDataStorageConfig
               ?? throw new InvalidOperationException("TelegramDataStorageConfig is not set");
    }

    private static ITelegramDataStorage GetService(TelegramDataStorageConfig telegramDataStorageConfig)
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging(configure => configure.AddConsole())
            .AddTelegramDataStorage(telegramDataStorageConfig)
            .BuildServiceProvider();

        return serviceProvider.GetRequiredService<ITelegramDataStorage>();
    }

    private static (NestedData NestedData, BytesData BytesData) GenerateData()
    {
        var nestedData = new NestedData
        {
            Name = "John Doe",
            Age = 42,
            Address = new Address
            {
                City = "New York",
                Street = "5th Avenue",
                PostalCode = "10001",
                Country = null,
            },
            Roles =
            [
                new Role
                {
                    RoleName = "Admin",
                    Permissions = [ "Read", "Write" ],
                },
                new Role
                {
                    RoleName = "User",
                    Permissions = [ "Read" ],
                },
            ],
            Preferences = new Dictionary<string, string>
            {
                {
                    "Theme", "Dark"
                },
                {
                    "Language", "English"
                },
            },
            Status = Status.Suspended,
        };

        var bytesData = new BytesData
        {
            Data = [ 0x01, 0x02, 0x03, 0x04, 0x05 ],
        };

        return (nestedData, bytesData);
    }
}