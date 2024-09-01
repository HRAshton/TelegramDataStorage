namespace TelegramDataStorage.Exceptions;

/// <summary>
/// Exception that is thrown when data cannot be saved to the storage.
/// </summary>
public class TelegramDataStorageSaveException
    : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TelegramDataStorageSaveException"/> class.
    /// </summary>
    /// <param name="message">Message that describes the error.</param>
    [Obsolete("Preserve original exception")]
    public TelegramDataStorageSaveException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TelegramDataStorageSaveException"/> class.
    /// </summary>
    /// <param name="message">Message that describes the error.</param>
    /// <param name="innerException">Exception that caused this exception.</param>
    public TelegramDataStorageSaveException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}