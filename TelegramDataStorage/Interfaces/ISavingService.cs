namespace TelegramDataStorage.Interfaces;

/// <summary>
/// Service that saves data to the storage.
/// </summary>
public interface ISavingService
{
    /// <summary>
    /// Saves the specified data to the storage.
    /// </summary>
    /// <param name="data">Data to be saved.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <typeparam name="T">Type of the data to be saved.</typeparam>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SaveAsync<T>(T data, CancellationToken cancellationToken = default)
        where T : class, IStoredData, new();
}