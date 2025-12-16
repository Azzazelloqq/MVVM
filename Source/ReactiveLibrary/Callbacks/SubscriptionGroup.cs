#if !PROJECT_SUPPORT_R3
using System;

namespace Azzazelloqq.MVVM.ReactiveLibrary.Callbacks
{
/// <summary>
/// Utility factory for creating combined subscription tokens without allocations.
/// </summary>
public static class CombinedDisposable
{
	/// <summary>
	/// Creates a combined disposable value that disposes both <paramref name="first"/> and <paramref name="second"/>.
	/// </summary>
	/// <param name="first">The first subscription token.</param>
	/// <param name="second">The second subscription token.</param>
	/// <typeparam name="TFirst">The type of the first token.</typeparam>
	/// <typeparam name="TSecond">The type of the second token.</typeparam>
	/// <returns>A value-type disposable wrapping both tokens.</returns>
	public static CombinedDisposable<TFirst, TSecond> Create<TFirst, TSecond>(TFirst first, TSecond second)
		where TFirst : struct, IDisposable
		where TSecond : struct, IDisposable
	{
		return new CombinedDisposable<TFirst, TSecond>(first, second);
	}
}

/// <summary>
/// Represents two subscription disposables grouped together without allocations.
/// </summary>
/// <typeparam name="TFirst">The type of the first subscription token.</typeparam>
/// <typeparam name="TSecond">The type of the second subscription token.</typeparam>
public struct CombinedDisposable<TFirst, TSecond> : IDisposable
	where TFirst : struct, IDisposable
	where TSecond : struct, IDisposable
{
	private TFirst _first;
	private TSecond _second;
	private bool _disposed;

	internal CombinedDisposable(TFirst first, TSecond second)
	{
		_first = first;
		_second = second;
		_disposed = false;
	}

	/// <summary>
	/// Disposes both subscription tokens exactly once.
	/// </summary>
	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}

		_disposed = true;
		_first.Dispose();
		_second.Dispose();
	}
}
}
#endif

