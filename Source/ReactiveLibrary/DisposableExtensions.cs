using System;
using Disposable;

namespace Azzazelloqq.MVVM.ReactiveLibrary
{
/// <summary>
/// Extension methods for managing disposable subscriptions with ICompositeDisposable.
/// </summary>
public static class DisposableExtensions
{
	/// <summary>
	/// Adds a disposable to the specified composite disposable container.
	/// </summary>
	/// <typeparam name="T">The type of disposable.</typeparam>
	/// <param name="disposable">The disposable to add.</param>
	/// <param name="compositeDisposable">The composite disposable container to add to.</param>
	/// <returns>The original disposable for method chaining.</returns>
	public static T AddTo<T>(this T disposable, ICompositeDisposable compositeDisposable) where T : IDisposable
	{
		if (compositeDisposable == null)
		{
			throw new ArgumentNullException(nameof(compositeDisposable));
		}

		compositeDisposable.AddDisposable(disposable);
		return disposable;
	}
}
}
