using System;
using UnityEngine.Events;
using UnityEngine.UI;
#if PROJECT_SUPPORT_R3
using R3;
#else
using Azzazelloqq.MVVM.ReactiveLibrary;
#endif

namespace Azzazelloqq.MVVM.Unity
{
/// <summary>
/// Subscriptions and one-way bindings for Unity UI controls.
/// </summary>
public static class UguiReactiveBindings
{
	public static IDisposable SubscribeClick(this Button button, Action handler)
	{
		if (button == null) throw new ArgumentNullException(nameof(button));
		if (handler == null) throw new ArgumentNullException(nameof(handler));
		UnityAction listener = handler.Invoke;
		button.onClick.AddListener(listener);
		return new UnityEventSubscription(() => button.onClick.RemoveListener(listener));
	}

	public static IDisposable SubscribeValueChanged(this Toggle toggle, Action<bool> handler)
	{
		if (toggle == null) throw new ArgumentNullException(nameof(toggle));
		if (handler == null) throw new ArgumentNullException(nameof(handler));
		UnityAction<bool> listener = handler.Invoke;
		toggle.onValueChanged.AddListener(listener);
		return new UnityEventSubscription(() => toggle.onValueChanged.RemoveListener(listener));
	}

	public static IDisposable SubscribeValueChanged(this Slider slider, Action<float> handler)
	{
		if (slider == null) throw new ArgumentNullException(nameof(slider));
		if (handler == null) throw new ArgumentNullException(nameof(handler));
		UnityAction<float> listener = handler.Invoke;
		slider.onValueChanged.AddListener(listener);
		return new UnityEventSubscription(() => slider.onValueChanged.RemoveListener(listener));
	}

	public static IDisposable SubscribeValueChanged(this InputField inputField, Action<string> handler)
	{
		if (inputField == null) throw new ArgumentNullException(nameof(inputField));
		if (handler == null) throw new ArgumentNullException(nameof(handler));
		UnityAction<string> listener = handler.Invoke;
		inputField.onValueChanged.AddListener(listener);
		return new UnityEventSubscription(() => inputField.onValueChanged.RemoveListener(listener));
	}

	public static IDisposable SubscribeEndEdit(this InputField inputField, Action<string> handler)
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
		Text target,
		Func<TValue, string> formatter = null)
	{
		if (source == null) throw new ArgumentNullException(nameof(source));
		if (target == null) throw new ArgumentNullException(nameof(target));
		return source.Subscribe(value => target.text = Format(value, formatter));
	}

	public static IDisposable BindInteractable<TSelectable>(
#if PROJECT_SUPPORT_R3
		this Observable<bool> source,
#else
		this IReadOnlyReactiveProperty<bool> source,
#endif
		TSelectable target)
		where TSelectable : Selectable
	{
		if (source == null) throw new ArgumentNullException(nameof(source));
		if (target == null) throw new ArgumentNullException(nameof(target));
		return source.Subscribe(value => target.interactable = value);
	}

	private static string Format<TValue>(TValue value, Func<TValue, string> formatter)
	{
		return formatter != null ? formatter(value) : value is null ? string.Empty : value.ToString();
	}
}
}
