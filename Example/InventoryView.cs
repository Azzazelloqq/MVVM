using System.Collections.Generic;
using Azzazelloqq.MVVM.Example.Item;
using Azzazelloqq.MVVM.Source.Core.View;
using UnityEngine;
using UnityEngine.UI;

namespace Azzazelloqq.MVVM.Example
{
public class InventoryView : ViewMonoBehavior<InventoryViewModel>
{
	[SerializeField]
	private InputField _newItemInput;

	[SerializeField]
	private Button _addItemButton;

	[SerializeField]
	private Button _loadInventoryButton;

	[SerializeField]
	private Transform _itemListParent;

	[SerializeField]
	private GameObject _itemPrefab;

	private readonly List<ItemView> _itemViews = new();

	protected override void OnInitialize()
	{
		base.OnInitialize();

		// Subscribe to Unity UI events
		_newItemInput.onValueChanged.AddListener(OnNewItemInputChanged);
		_addItemButton.onClick.AddListener(OnAddItemButtonClicked);

		if (_loadInventoryButton != null)
		{
			_loadInventoryButton.onClick.AddListener(OnLoadInventoryClicked);
		}

		// Populate existing items
		InitializeItemViews();

		// Subscribe to new item additions
		viewModel.Items.SubscribeOnItemAdded(CreateItemView);
		// You could also subscribeOnItemRemoved if you want to remove from UI automatically
	}

	protected override void OnDispose()
	{
		// Unsubscribe from Unity UI events
		_newItemInput.onValueChanged.RemoveListener(OnNewItemInputChanged);
		_addItemButton.onClick.RemoveListener(OnAddItemButtonClicked);

		if (_loadInventoryButton != null)
		{
			_loadInventoryButton.onClick.RemoveListener(OnLoadInventoryClicked);
		}

		// Unsubscribe from reactive list notifications
		viewModel.Items.UnsubscribeOnItemAdded(CreateItemView);

		// Dispose all item sub-views
		foreach (var itemView in _itemViews)
		{
			itemView.Dispose();
		}

		_itemViews.Clear();

		base.OnDispose();
	}

	private void InitializeItemViews()
	{
		foreach (var itemViewModel in viewModel.Items)
		{
			CreateItemView(itemViewModel);
		}
	}

	private void CreateItemView(ItemViewModel itemViewModel)
	{
		var itemObject = Instantiate(_itemPrefab, _itemListParent);
		var itemView = itemObject.GetComponent<ItemView>();
		itemView.Initialize(itemViewModel);

		_itemViews.Add(itemView);
	}

	private void OnNewItemInputChanged(string input)
	{
		viewModel.NewItemName.SetValue(input);
	}

	private void OnAddItemButtonClicked()
	{
		viewModel.AddItemCommand.Execute(_newItemInput.text);
	}

	private void OnLoadInventoryClicked()
	{
		// Provide a fake path or something meaningful
		const string fakePath = "FakeInventoryData.json";
		viewModel.LoadInventoryCommand.ExecuteAsync(fakePath);
	}
}
}