using System;
using UnityEngine;
#if PROJECT_SUPPORT_R3
using R3;
#else
using Azzazelloqq.MVVM.ReactiveLibrary;
#endif

namespace Azzazelloqq.MVVM.Unity
{
/// <summary>
/// Reactive bindings for common Unity object state.
/// </summary>
public static class UnityReactiveBindings
{
	public static IDisposable BindActive(
#if PROJECT_SUPPORT_R3
		this Observable<bool> source,
#else
		this IReadOnlyReactiveProperty<bool> source,
#endif
		GameObject target)
	{
		if (source == null) throw new ArgumentNullException(nameof(source));
		if (target == null) throw new ArgumentNullException(nameof(target));
		return source.Subscribe(target.SetActive);
	}

	public static IDisposable BindEnabled<TBehaviour>(
#if PROJECT_SUPPORT_R3
		this Observable<bool> source,
#else
		this IReadOnlyReactiveProperty<bool> source,
#endif
		TBehaviour target)
		where TBehaviour : Behaviour
	{
		if (source == null) throw new ArgumentNullException(nameof(source));
		if (target == null) throw new ArgumentNullException(nameof(target));
		return source.Subscribe(value => target.enabled = value);
	}
}
}
