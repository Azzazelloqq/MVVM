using System;

namespace Azzazelloqq.MVVM.Unity
{
internal sealed class UnityEventSubscription : IDisposable
{
	private Action _unsubscribe;

	public UnityEventSubscription(Action unsubscribe)
	{
		_unsubscribe = unsubscribe ?? throw new ArgumentNullException(nameof(unsubscribe));
	}

	public void Dispose()
	{
		var unsubscribe = _unsubscribe;
		if (unsubscribe == null)
		{
			return;
		}

		_unsubscribe = null;
		unsubscribe();
	}
}
}
