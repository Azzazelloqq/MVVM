using System;
using System.Collections.Generic;
#if PROJECT_SUPPORT_R3
using R3;
#endif

namespace Azzazelloqq.MVVM.ReactiveLibrary
{
/// <summary>
/// Helpers for observing a selected part of a reactive value.
/// </summary>
public static class SelectedValueExtensions
{
	/// <summary>
	/// Invokes <paramref name="onChanged"/> only when the selected value changes.
	/// The current selected value is emitted on subscription.
	/// </summary>
	public static IDisposable SubscribeSelected<TSource, TValue>(
#if PROJECT_SUPPORT_R3
		this Observable<TSource> source,
#else
		this IReadOnlyReactiveProperty<TSource> source,
#endif
		Func<TSource, TValue> selector,
		Action<TValue> onChanged,
		IEqualityComparer<TValue> comparer = null)
	{
		if (source == null)
		{
			throw new ArgumentNullException(nameof(source));
		}

		if (selector == null)
		{
			throw new ArgumentNullException(nameof(selector));
		}

		if (onChanged == null)
		{
			throw new ArgumentNullException(nameof(onChanged));
		}

		comparer ??= EqualityComparer<TValue>.Default;
		var hasValue = false;
		var previousValue = default(TValue);

		return source.Subscribe(value =>
		{
			var selectedValue = selector(value);
			if (hasValue && comparer.Equals(previousValue, selectedValue))
			{
				return;
			}

			hasValue = true;
			previousValue = selectedValue;
			onChanged(selectedValue);
		});
	}
}
}
