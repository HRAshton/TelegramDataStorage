namespace TelegramDataStorage.Interfaces;

/// <summary>
/// Represents a stored data.
/// </summary>
public interface IStoredData
{
    /// <summary>
    /// Gets the unique key of the stored data.
    /// </summary>
    public static abstract string Key { get; }
}