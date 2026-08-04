using System;
using TMPro;
using UnityEngine.Events;
#if PROJECT_SUPPORT_R3
using R3;
#else
using Azzazelloqq.MVVM.ReactiveLibrary;
#endif

namespace Azzazelloqq.MVVM.Unity
{
/// <summary>
/// Subscriptions and one-way bindings for TextMesh Pro controls.
/// </summary>
public static class TmpReactiveBindings
{
	public static IDisposable SubscribeValueChanged(this TMP_InputField inputField, Action<string> handler)
	{
		if (inputField == null) throw new ArgumentNullException(nameof(inputField));
		if (handler == null) throw new ArgumentNullException(nameof(handler));
		UnityAction<string> listener = handler.Invoke;
		inputField.onValueChanged.AddListener(listener);
		return new UnityEventSubscription(() => inputField.onValueChanged.RemoveListener(listener));
	}

	public static IDisposable SubscribeEndEdit(this TMP_InputField inputField, Action<string> handler)
	{
		if (inputField == null) throw new ArgumentNullException(nameof(inputField));
		if (handler == null) throw new ArgumentNullException(nameof(handler));
		UnityAction<string> listener = handler.Invoke;
		inputField.onEndEdit.AddListener(listener);
		return new UnityEventSubscription(() => inputField.onEndEdit.RemoveListener(listener));
	}

	public static IDisposable BindText<TValue>(
#if PROJECT_SUPPORT_R3
		this Observable<TValue> source,
#else
		this IReadOnlyReactiveProperty<TValue> source,
#endif
		TMP_Text target,
		Func<TValue, string> formatter = null)
	{
		if (source == null) throw new ArgumentNullException(nameof(source));
		if (target == null) throw new ArgumentNullException(nameof(target));
		return source.Subscribe(value =>
			target.text = formatter != null
				? formatter(value)
				: value is null ? string.Empty : value.ToString());
	}
}
}
