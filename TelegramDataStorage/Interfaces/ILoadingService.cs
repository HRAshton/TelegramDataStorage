namespace TelegramDataStorage.Interfaces;

/// <summary>
/// Service that loads data from the storage.
/// </summary>
public interface ILoadingService
{
    /// <summary>
    /// Loads data from the storage.
    /// </summary>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <typeparam name="T">Type of the data to be loaded.</typeparam>
    /// <returns>Loaded data or <see langword="null"/> if the data is not found.</returns>
    Task<T?> LoadAsync<T>(CancellationToken cancellationToken = default)
        where T : class, IStoredData, new();
}