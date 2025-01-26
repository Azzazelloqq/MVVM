using System;
using MVVM.MVVM.ReactiveLibrary.Property;
using MVVM.MVVM.System.Base.Command.Base;
using MVVM.MVVM.System.Base.Command.Commands;
using MVVM.MVVM.System.Base.ViewModel;

namespace MVVM.MVVM.Example.Item
{
/// <summary>
/// The ViewModel for a single item. 
/// Demonstrates a callback approach to remove this item from the parent Inventory.
/// </summary>
public class ItemViewModel : ViewModelBase<ItemModel>
{
	public IReadOnlyReactiveProperty<string> ItemName => model.ItemName;
	public IActionCommand RemoveItemCommand { get; }

	private readonly Action<string> _removeItemCallback;

	public ItemViewModel(ItemModel model, Action<string> removeItemCallback) : base(model)
	{
		_removeItemCallback = removeItemCallback;

		// We define a RemoveItemCommand which calls the callback with our current item name
		RemoveItemCommand = new ActionCommand(OnRemoveItem);
	}

	protected override void OnDispose()
	{
		RemoveItemCommand.Dispose();
		base.OnDispose();
	}

	private void OnRemoveItem()
	{
		_removeItemCallback?.Invoke(ItemName.Value);
	}
}
}