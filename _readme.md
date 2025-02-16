# Telegram Data Storage

Store your data (config, session, etc.) in a telegram chat.

## Description

This library allows you to store your data in Telegram itself.

It stores the serialized data in a Telegram chat message as a JSON file.
The list of the stored files is stored in the channel's description.

When you save data, the library will create a new message with the data in the channel.

When you load data, the library will find the message, download the file, and deserialize the data.
To retrieve the data, the library will forward the message to the same channel and delete it after downloading the file.

## Usage

```csharp
using TelegramDataStorage;

class SomeData : IStoredData
{
    public static string Key => "MyBeautifulData";

    public string SomeProperty { get; set; }
}

var serviceCollection = new ServiceCollection();

var config = new TelegramDataStorageConfig(
    BotToken: "1234567890:ABCdefGhIjKlmnOpqrStuVwXyz1234567890",
    ChatId: -1001234567890
);

serviceCollection.AddTelegramDataStorageForTelegramBot(config); // for TelegramBot package
// or serviceCollection.AddTelegramDataStorageForWTelegramBot(config); // for WTelegramBot package

var serviceProvider = serviceCollection.BuildServiceProvider();

var storage = serviceProvider.GetRequiredService<ITelegramDataStorage>();

var someData = new SomeData
{
    SomeProperty = "Some value"
};

await storage.SaveAsync(someData);

var loadedData = await storage.LoadAsync<SomeData>();

Console.WriteLine(loadedData.SomeProperty);
```

### Advanced usage

Basically, you only need to implement the `IStoredData` interface and use the `ITelegramDataStorage` service.
However, all classes are public and can be used directly or replaced with your own implementation.

For example, you can replace `IDataConverter` with your own implementation to add encryption or compression.

## Testing

To run integration tests, you need to create a new Telegram bot and a new channel.
Then, you need to set them in the `appsettings.json` file and run `dotnet test`.
[CHANGELOG.md](CHANGELOG.md)
To run them in docker, you need to pass the `BOT_TOKEN` and `CHAT_ID` arguments to `docker build`.

## Limitations:

- The data size of the data JSON string should be less than 20MB (limitation of Telegram file size).
- The size of all key-messageId pairs should be less than 256 characters (limitation of the channel description).
- Commas and semicolons are not allowed in the data keys (they are used as separators).
- The library does not support multiple data with the same key.
