using System;
using System.Collections.Generic;

namespace TelegramDataStorage.Interfaces;

/// <summary>
/// Service that allows to subscribe to the data changes.
/// </summary>
public interface ISubscriptionService
{
    /// <summary>
    /// Subscribes to the data changes.
    /// Calls the specified action with the changed data keys.
    /// </summary>
    /// <param name="onChanges">Action that should be executed when the data changes.</param>
    /// <returns>Guid of the subscription.</returns>
    Guid Subscribe(Action<HashSet<string>> onChanges);

    /// <summary>
    /// Unsubscribes from the data changes.
    /// </summary>
    void Unsubscribe(Guid subscriptionId);
}