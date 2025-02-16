using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TelegramDataStorage.Core.Configuration;
using TelegramDataStorage.IntegrationTests.SharedData.Models;
using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage.IntegrationTests.SharedData;

/// <summary>
/// Integration tests for the Telegram data storage.
/// </summary>
public abstract class SharedIntegrationTests : IAsyncLifetime
{
    protected TelegramDataStorageConfig Config { get; } = GetConfig();

    /// <inheritdoc/>
    public async Task InitializeAsync()
    {
        await SetChatDescriptionAsync($"{Random.Shared.Next()}");
        await SetChatDescriptionAsync(null);
    }

    /// <inheritdoc/>
    public async Task DisposeAsync()
    {
        await SetChatDescriptionAsync(null);
    }

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
    protected virtual async Task SendComplexDataAndLoadIt()
    {
        var service = GetService(Config);
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

        Assert.NotNull(nestedData);
        var updatedData = nestedData with
        {
            Address = new Address
            {
                City = "Los Angeles",
            },
        };
        await service.SaveAsync(updatedData);
        nestedData = await service.LoadAsync<NestedData>();

        Assert.NotNull(nestedData);
        Assert.Equivalent(updatedData, nestedData);
    }

    protected abstract Task SetChatDescriptionAsync(string? description);

    protected abstract void RegisterServices(
        IServiceCollection services,
        TelegramDataStorageConfig telegramDataStorageConfig);

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

    private ITelegramDataStorage GetService(TelegramDataStorageConfig telegramDataStorageConfig)
    {
        var services = new ServiceCollection();
        RegisterServices(services, telegramDataStorageConfig);
        var serviceProvider = services.BuildServiceProvider();
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
                City = "New York", Street = "5th Avenue", PostalCode = "10001", Country = null,
            },
            Roles =
            [
                new Role
                {
                    RoleName = "Admin", Permissions = [ "Read", "Write" ],
                },
                new Role
                {
                    RoleName = "User", Permissions = [ "Read" ],
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