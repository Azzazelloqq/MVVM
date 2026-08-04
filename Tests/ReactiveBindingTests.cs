using System;
using Azzazelloqq.MVVM.ReactiveLibrary;
using Azzazelloqq.MVVM.Unity;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#if PROJECT_SUPPORT_R3
using R3;
#endif

namespace Azzazelloqq.MVVM.Tests
{
public sealed class ReactiveBindingTests
{
	[Test]
	public void SubscribeSelected_NotifiesOnlyWhenSelectedValueChanges()
	{
		using var source = new ReactiveProperty<ViewState>(new ViewState(1, "first"));
		var notifications = 0;
		var selectedValue = 0;
		using var subscription = source.SubscribeSelected(
			state => state.Count,
			value =>
			{
				notifications++;
				selectedValue = value;
			});

		SetValue(source, new ViewState(1, "second"));
		SetValue(source, new ViewState(2, "second"));

		Assert.AreEqual(2, notifications);
		Assert.AreEqual(2, selectedValue);
	}

	[Test]
	public void SubscribeClick_DisposeRemovesListener()
	{
		var gameObject = new GameObject("Button", typeof(RectTransform), typeof(Button));
		try
		{
			var button = gameObject.GetComponent<Button>();
			var calls = 0;
			var subscription = button.SubscribeClick(() => calls++);

			button.onClick.Invoke();
			subscription.Dispose();
			button.onClick.Invoke();

			Assert.AreEqual(1, calls);
		}
		finally
		{
			UnityEngine.Object.DestroyImmediate(gameObject);
		}
	}

	[Test]
	public void SubscribeValueChanged_DisposeRemovesToggleListener()
	{
		var gameObject = new GameObject("Toggle", typeof(RectTransform), typeof(Toggle));
		try
		{
			var toggle = gameObject.GetComponent<Toggle>();
			var calls = 0;
			var subscription = toggle.SubscribeValueChanged(_ => calls++);

			toggle.isOn = true;
			subscription.Dispose();
			toggle.isOn = false;

			Assert.AreEqual(1, calls);
		}
		finally
		{
			UnityEngine.Object.DestroyImmediate(gameObject);
		}
	}

	[Test]
	public void BindText_UpdatesUnityAndTmpText()
	{
		var unityTextObject = new GameObject("Text", typeof(RectTransform), typeof(Text));
		var tmpTextObject = new GameObject("TMP", typeof(RectTransform), typeof(TextMeshProUGUI));
		using var source = new ReactiveProperty<int>(1);
		try
		{
			var unityText = unityTextObject.GetComponent<Text>();
			var tmpText = tmpTextObject.GetComponent<TextMeshProUGUI>();
			using var unitySubscription = source.BindText(unityText, value => $"U:{value}");
			using var tmpSubscription = source.BindText(tmpText, value => $"T:{value}");

			SetValue(source, 2);

			Assert.AreEqual("U:2", unityText.text);
			Assert.AreEqual("T:2", tmpText.text);
		}
		finally
		{
			UnityEngine.Object.DestroyImmediate(unityTextObject);
			UnityEngine.Object.DestroyImmediate(tmpTextObject);
		}
	}

	[Test]
	public void BindActive_TracksReactiveValueUntilDisposed()
	{
		var target = new GameObject("Target");
		using var source = new ReactiveProperty<bool>(false);
		try
		{
			var subscription = source.BindActive(target);
			Assert.IsFalse(target.activeSelf);

			SetValue(source, true);
			Assert.IsTrue(target.activeSelf);

			subscription.Dispose();
			SetValue(source, false);
			Assert.IsTrue(target.activeSelf);
		}
		finally
		{
			UnityEngine.Object.DestroyImmediate(target);
		}
	}

	private static void SetValue<TValue>(ReactiveProperty<TValue> property, TValue value)
	{
#if PROJECT_SUPPORT_R3
		property.Value = value;
#else
		property.SetValue(value);
#endif
	}

	private readonly struct ViewState
	{
		public ViewState(int count, string label)
		{
			Count = count;
			Label = label;
		}

		public int Count { get; }
		public string Label { get; }
	}
}
}
