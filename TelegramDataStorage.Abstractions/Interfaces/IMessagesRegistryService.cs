using System.Collections.Generic;
using System.Threading.Tasks;

namespace TelegramDataStorage.Interfaces;

/// <summary>
/// Service that stores message identifiers by data keys.
/// </summary>
public interface IMessagesRegistryService
{
    /// <summary>
    /// Tries to get the message identifier by the specified data key.
    /// </summary>
    /// <param name="key">Key of the data.</param>
    /// <returns>Message identifier or <see langword="null"/> if the data is not found.</returns>
    Task<int?> TryGetAsync(string key);

    /// <summary>
    /// Adds or updates the message identifier for the specified data key.
    /// </summary>
    /// <param name="key">Key of the data.</param>
    /// <param name="messageId">Identifier of the message.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddOrUpdateAsync(string key, int messageId);

    /// <summary>
    /// Lists all stored data keys with their message identifiers.
    /// </summary>
    /// <returns>A dictionary with data keys as keys and message identifiers as values.</returns>
    Task<IDictionary<string, int>> ListAsync();
}