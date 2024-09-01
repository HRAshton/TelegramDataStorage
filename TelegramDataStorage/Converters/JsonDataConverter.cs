using System.Diagnostics.CodeAnalysis;
using System.Text;
using Newtonsoft.Json;
using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage.Converters;

/// <summary>
/// Serializes and deserializes data to and from JSON format.
/// </summary>
public class JsonDataConverter : IDataConverter
{
    /// <inheritdoc cref="IDataConverter.Serialize{T}(T, out string)" />
    public Stream Serialize<T>(T data, out string filename)
        where T : IStoredData
    {
        ArgumentNullException.ThrowIfNull(data);

        var json = JsonConvert.SerializeObject(data);
        var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));

        filename = $"{T.Key}.json";
        return jsonStream;
    }

    /// <inheritdoc cref="IDataConverter.Deserialize{T}(byte[])" />
    [SuppressMessage(
        "Performance",
        "CA1822:Mark members as static",
        Justification = "The method is an interface implementation.")]
    public T Deserialize<T>(byte[] fileContent)
        where T : IStoredData
    {
        ArgumentNullException.ThrowIfNull(fileContent);

        var result = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(fileContent));
        return result ?? throw new InvalidOperationException("The file does not contain necessary data.");
    }
}