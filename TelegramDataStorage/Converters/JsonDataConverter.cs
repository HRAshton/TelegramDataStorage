using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
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

        var jsonStream = new MemoryStream();
        JsonSerializer.Serialize(jsonStream, data);
        jsonStream.Position = 0;

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

        var result = JsonSerializer.Deserialize<T>(fileContent);
        return result ?? throw new InvalidOperationException("The file does not contain necessary data.");
    }
}