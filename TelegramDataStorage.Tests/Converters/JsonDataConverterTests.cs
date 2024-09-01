using System.Text;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using TelegramDataStorage.Converters;
using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage.Tests.Converters;

public class JsonDataConverterTests
{
    private readonly JsonDataConverter _jsonDataConverter = new();

    [Fact]
    public void Serialize_ShouldSerializeObjectToJsonAndReturnInputFileStream()
    {
        // Arrange
        var mockStoredData = new StoredDataStub
        {
            SomeField = "test",
        };

        // Act
        IDisposable disposable = _jsonDataConverter.Serialize(mockStoredData, out InputFileStream inputFileStream);

        // Assert
        Assert.NotNull(disposable);
        Assert.NotNull(inputFileStream);
        Assert.Equal("test.json", inputFileStream.FileName);

        using (var reader = new StreamReader(inputFileStream.Content))
        {
            string jsonContent = reader.ReadToEnd();
            Assert.Contains("\"SomeField\":\"test\"", jsonContent);
        }

        disposable.Dispose();
    }

    [Fact]
    public void Serialize_NullData_ShouldThrowArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            _jsonDataConverter.Serialize<StoredDataStub>(null, out _)
        );
    }

    [Fact]
    public void Deserialize_ShouldDeserializeJsonToObject()
    {
        // Arrange
        const string json = "{\"SomeField\":\"Some Field Value\"}";
        var jsonBytes = Encoding.UTF8.GetBytes(json);

        // Act
        var result = _jsonDataConverter.Deserialize<StoredDataStub>(jsonBytes);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Some Field Value", result.SomeField);
    }

    [Fact]
    public void Deserialize_NullFileContent_ShouldThrowArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            _jsonDataConverter.Deserialize<StoredDataStub>(null)
        );
    }

    [Fact]
    public void Deserialize_InvalidJson_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var invalidJsonBytes = "invalid json"u8.ToArray();

        // Act & Assert
        Assert.Throws<JsonReaderException>(() =>
            _jsonDataConverter.Deserialize<StoredDataStub>(invalidJsonBytes));
    }

    private class StoredDataStub : IStoredData
    {
        public static string Key => "test";

        public string SomeField { get; init; } = "Some Field Value";
    }
}