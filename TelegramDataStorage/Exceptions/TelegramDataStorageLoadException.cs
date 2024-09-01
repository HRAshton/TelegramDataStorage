namespace TelegramDataStorage.Exceptions;

/// <summary>
/// Exception that is thrown when data cannot be loaded from the storage.
/// </summary>
public class TelegramDataStorageLoadException
    : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TelegramDataStorageLoadException"/> class.
    /// </summary>
    /// <param name="message">Message that describes the error.</param>
    [Obsolete("Preserve original exception")]
    public TelegramDataStorageLoadException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TelegramDataStorageLoadException"/> class.
    /// </summary>
    /// <param name="message">Message that describes the error.</param>
    /// <param name="innerException">Exception that caused this exception.</param>
    public TelegramDataStorageLoadException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}