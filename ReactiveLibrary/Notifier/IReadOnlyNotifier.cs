﻿using System;
using MVVM.MVVM.ReactiveLibrary.Base;

namespace MVVM.MVVM.ReactiveLibrary.Notifier
{
/// <summary>
/// Represents a read-only notifier that allows subscription to notifications without providing a mechanism to trigger them.
/// Inherits from <see cref="IReactive"/>.
/// </summary>
public interface IReadOnlyNotifier : IReactive
{
    /// <summary>
    /// Subscribes to receive notifications.
    /// </summary>
    /// <param name="onNotify">The action to invoke when a notification is triggered.</param>
    public void Subscribe(Action onNotify);
    
    /// <summary>
    /// Unsubscribes from receiving notifications.
    /// </summary>
    /// <param name="onNotify">The action that was previously subscribed.</param>
    public void Unsubscribe(Action onNotify);
}
}