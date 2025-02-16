using Microsoft.Extensions.Logging;
using TelegramDataStorage.Exceptions;
using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage.Core;

/// <inheritdoc />
public partial class TelegramDataStorage(
    ILogger<TelegramDataStorage> logger,
    ISavingService savingService,
    ILoadingService loadingService)
    : ITelegramDataStorage
{
    /// <inheritdoc cref="ISavingService.SaveAsync{T}(T, CancellationToken)" />
    public async Task SaveAsync<T>(T data, CancellationToken cancellationToken = default)
        where T : class, IStoredData, new()
    {
        try
        {
            Log.SavingDataForKey(logger, T.Key);
            await savingService.SaveAsync(data, cancellationToken);
            Log.DataSavedForKey(logger, T.Key);
        }
        catch (Exception e)
        {
            Log.FailedToSaveDataForKey(logger, T.Key);
            throw new TelegramDataStorageSaveException("Failed to save data", e);
        }
    }

    /// <inheritdoc cref="ILoadingService.LoadAsync{T}(CancellationToken)" />
    public async Task<T?> LoadAsync<T>(CancellationToken cancellationToken = default)
        where T : class, IStoredData, new()
    {
        try
        {
            Log.LoadingDataForKey(logger, T.Key);
            return await loadingService.LoadAsync<T>(cancellationToken);
        }
        catch (Exception e)
        {
            Log.FailedToLoadDataForKey(logger, T.Key);
            throw new TelegramDataStorageLoadException("Failed to load data", e);
        }
    }
}