using System.Threading;
using System.Threading.Tasks;
using TelegramDataStorage.Exceptions;

namespace TelegramDataStorage.Interfaces;

/// <summary>
/// Service that stores data in the storage.
/// </summary>
public interface ITelegramDataStorage
{
    /// <summary>
    /// Tries to get the message identifier by the specified data key.
    /// </summary>
    /// <param name="data">Data to be saved.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <typeparam name="T">Type of the data to be saved.</typeparam>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="TelegramDataStorageSaveException">Thrown when the data cannot be saved.</exception>
    Task SaveAsync<T>(T data, CancellationToken cancellationToken = default)
        where T : class, IStoredData, new();

    /// <summary>
    /// Tries to get the message identifier by the specified data key.
    /// </summary>
    /// <param name="cancellationToken">Token that can be used to cancel the operation.</param>
    /// <typeparam name="T">Type of the data to be loaded.</typeparam>
    /// <returns>Loaded data or <see langword="null"/> if the data is not found.</returns>
    /// <exception cref="TelegramDataStorageLoadException">Thrown when the data cannot be loaded.</exception>
    Task<T?> LoadAsync<T>(CancellationToken cancellationToken = default)
        where T : class, IStoredData, new();
}