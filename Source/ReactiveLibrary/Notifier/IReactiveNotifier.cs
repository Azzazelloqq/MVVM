#if !PROJECT_SUPPORT_R3
namespace Azzazelloqq.MVVM.ReactiveLibrary
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
#endif
