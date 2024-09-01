using System.Text;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage.Converters;

/// <summary>
/// Serializes and deserializes data to and from JSON format.
/// </summary>
public class JsonDataConverter : IDataConverter
{
    /// <inheritdoc />
    public IDisposable Serialize<T>(T data, out InputFileStream inputFileStream)
        where T : IStoredData
    {
        ArgumentNullException.ThrowIfNull(data);

        var json = JsonConvert.SerializeObject(data);
        var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));

        inputFileStream = new InputFileStream(jsonStream, $"{T.Key}.json");
        return jsonStream;
    }

    /// <inheritdoc />
    public T Deserialize<T>(byte[] fileContent)
        where T : IStoredData
    {
        ArgumentNullException.ThrowIfNull(fileContent);

        var result = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(fileContent));
        return result ?? throw new InvalidOperationException("The file does not contain necessary data.");
    }
}