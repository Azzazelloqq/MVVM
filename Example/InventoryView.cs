using MVVM.MVVM.Example.Item;
using MVVM.MVVM.System.Base.View;
using UnityEngine;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;

namespace MVVM.MVVM.Example
{
public class InventoryView : ViewMonoBehavior<InventoryViewModel>
{
	[SerializeField]
	private InputField _newItemInput;

	[SerializeField]
	private Button _addItemButton;

	[SerializeField]
	private Transform _itemListParent;

	[SerializeField]
	private GameObject _itemPrefab;
	
	protected override void OnInitialize()
	{
		base.OnInitialize();

		_newItemInput.onValueChanged.AddListener(OnInputChanged);
		_addItemButton.onClick.AddListener(AddItemButtonClicked);

		InitializeItemViews();

		viewModel.Items.SubscribeOnItemAdded(CreateItemView);
	}

	protected override void OnDispose()
	{
		base.OnDispose();
		
		viewModel.Items.UnsubscribeOnItemAdded(CreateItemView);

		_newItemInput.onValueChanged.RemoveListener(OnInputChanged);
		_addItemButton.onClick.RemoveListener(AddItemButtonClicked);
	}

	private void OnInputChanged(string input)
	{
		viewModel.NewItemName.SetValue(input);
	}

	private void AddItemButtonClicked()
	{
		viewModel.AddItemCommand.Execute(_newItemInput.text);
	}

	private void InitializeItemViews()
	{
		foreach (var itemViewModel in viewModel.Items)
		{
			CreateItemView(itemViewModel);
		}
	}

	private void CreateItemView(ItemViewModel item)
	{
		var itemObject = Instantiate(_itemPrefab, _itemListParent);
		var itemView = itemObject.GetComponent<ItemView>();
		itemView.Initialize(item);
	}
}
}