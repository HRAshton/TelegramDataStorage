using Telegram.Bot.Types;

namespace TelegramDataStorage.Interfaces;

/// <summary>
/// Serializes and deserializes data to and from a specific format for Telegram.
/// </summary>
public interface IDataConverter
{
    /// <summary>
    /// Prepares data to be sent to Telegram.
    /// </summary>
    /// <param name="data">Data to be serialized.</param>
    /// <param name="inputFileStream">Resulting file data.</param>
    /// <typeparam name="T">Type of the data to be serialized.</typeparam>
    /// <returns>Something that should be disposed after the data is sent.</returns>
    IDisposable? Serialize<T>(T data, out InputFileStream inputFileStream)
        where T : IStoredData;

    /// <summary>
    /// Extracts data from the file content.
    /// </summary>
    /// <param name="fileContent">Content of the file loaded from Telegram.</param>
    /// <typeparam name="T">Type of the data to be deserialized.</typeparam>
    /// <returns>Deserialized data.</returns>
    T Deserialize<T>(byte[] fileContent)
        where T : IStoredData;
}