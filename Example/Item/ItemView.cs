﻿using UnityEngine;
using UnityEngine.UI;
using MVVM.MVVM.System.Base.View;

namespace MVVM.MVVM.Example.Item
{
/// <summary>
/// A Unity View for displaying a single item (name + remove button).
/// Binds to <see cref="ItemViewModel"/>.
/// </summary>
public class ItemView : ViewMonoBehavior<ItemViewModel>
{
	[SerializeField]
	private Text _itemNameText;

	[SerializeField]
	private Button _removeButton;

	protected override void OnInitialize()
	{
		base.OnInitialize();

		// Bind item name
		viewModel.ItemName.Subscribe(OnItemNameChanged);

		// Hook up remove button
		_removeButton.onClick.AddListener(OnRemoveButtonClicked);
	}

	protected override void OnDispose()
	{
		// Unsubscribe from item name changes
		viewModel.ItemName.Unsubscribe(OnItemNameChanged);

		// Remove listener on remove button
		_removeButton.onClick.RemoveListener(OnRemoveButtonClicked);

		base.OnDispose();
	}

	private void OnItemNameChanged(string itemName)
	{
		_itemNameText.text = itemName;
	}

	private void OnRemoveButtonClicked()
	{
		viewModel.RemoveItemCommand.Execute();
	}
}
}