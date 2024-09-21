namespace MVVM.MVVM.ReactiveLibrary.Notifier
{
/// <summary>
/// Represents a reactive notifier that allows triggering notifications.
/// Inherits from <see cref="IReadOnlyNotifier"/>.
/// </summary>
public interface IReactiveNotifier : IReadOnlyNotifier
{
    /// <summary>
    /// Triggers a notification to all subscribed listeners.
    /// </summary>
    public void Notify();
}
}