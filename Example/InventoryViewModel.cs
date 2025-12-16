#if !PROJECT_SUPPORT_R3
using System;
using System.Threading;
using System.Threading.Tasks;
using Azzazelloqq.MVVM.Core;
using Azzazelloqq.MVVM.Example.Item;
using Azzazelloqq.MVVM.ReactiveLibrary;
using Azzazelloqq.MVVM.ReactiveLibrary.Collections;

namespace Azzazelloqq.MVVM.Example
{
internal class InventoryViewModel : ViewModelBase<InventoryModel>
{
	public IReactiveProperty<string> NewItemName { get; }
	public IRelayCommand<string> AddItemCommand { get; }
	public IRelayCommand<string> RemoveItemCommand { get; }
	public IAsyncCommand<string> LoadInventoryCommand { get; }
	public IReactiveList<ItemViewModel> Items { get; }

	public InventoryViewModel(InventoryModel model) : base(model)
	{
		// Initialize the reactive list of item viewmodels and add to composite for disposal
		Items = new ReactiveList<ItemViewModel>(model.Items.Count);
		Items.AddTo(compositeDisposable);

		// Property for name of new item
		NewItemName = new ReactiveProperty<string>();
		NewItemName.AddTo(compositeDisposable);

		// A RelayCommand to add items, with a basic canExecute check that it's not empty
		AddItemCommand = new RelayCommand<string>(
			OnAddItemCommandExecute,
			CanAddItemCommandExecute
		);
		AddItemCommand.AddTo(compositeDisposable);

		// A RelayCommand to remove items by name
		RemoveItemCommand = new RelayCommand<string>(model.RemoveItem);
		RemoveItemCommand.AddTo(compositeDisposable);

		// An AsyncRelayCommand to load inventory from a path
		LoadInventoryCommand = new AsyncRelayCommand<string>(OnLoadInventoryCommandExecuteAsync);
		LoadInventoryCommand.AddTo(compositeDisposable);
	}

	protected override void OnInitialize()
	{
		// Populate from the existing model list
		InitializeItemsFromModel();

		// Subscribe to model list changes and add to composite disposable using AddTo extension
		model.Items.SubscribeOnItemAdded(OnItemAdded);
		model.Items.SubscribeOnItemRemoved(OnItemRemoved);

		// Subscribe to NewItemName changes using AddTo for automatic disposal
		NewItemName.Subscribe(_ => RaiseAddItemCanExecuteChanged())
			.AddTo(compositeDisposable);
	}

	protected override async ValueTask OnInitializeAsync(CancellationToken token)
	{
		// Optionally do something async here
	}

	protected override void OnDispose()
	{
		// Unsubscribe from model changes
		model.Items.UnsubscribeOnItemAdded(OnItemAdded);
		model.Items.UnsubscribeOnItemRemoved(OnItemRemoved);

		// All disposables added with AddTo are automatically disposed by compositeDisposable in base class
	}

	protected override ValueTask OnDisposeAsync(CancellationToken token)
	{
		throw new NotImplementedException();
	}

	private void OnItemAdded(string itemName)
	{
		AddItemViewModel(itemName);
	}

	private void OnItemRemoved(string itemName)
	{
		Items.Remove(vm => vm.ItemName.Value == itemName);
	}

	private void InitializeItemsFromModel()
	{
		foreach (var modelItem in model.Items)
		{
			AddItemViewModel(modelItem);
		}
	}

	private void AddItemViewModel(string itemName)
	{
		// Create a new ItemModel
		var itemModel = new ItemModel(itemName);

		// Pass a callback for removing this item from Inventory
		var itemViewModel = new ItemViewModel(
			itemModel,
			RemoveItemFromItemViewModel // callback
		);

		itemViewModel.Initialize(); // Synchronous init, or you can do async if needed
		Items.Add(itemViewModel);
	}

	private void RemoveItemFromItemViewModel(string itemName)
	{
		model.RemoveItem(itemName);
	}

	private void OnAddItemCommandExecute(string itemName)
	{
		model.AddItem(itemName);
	}

	private bool CanAddItemCommandExecute()
	{
		return !string.IsNullOrEmpty(NewItemName.Value);
	}

	private void RaiseAddItemCanExecuteChanged()
	{
		// If your RelayCommand supports a method to notify the view about changes in can-execute, call it here.
		// Example:
		// (AddItemCommand as RelayCommand<string>)?.RaiseCanExecuteChanged();
	}

	private async Task OnLoadInventoryCommandExecuteAsync(string path)
	{
		await model.LoadInventoryAsync(path, disposeToken);
		// Model will trigger OnItemAdded for each new item, 
		// so the UI will be updated automatically.
	}
}
}
#endif
