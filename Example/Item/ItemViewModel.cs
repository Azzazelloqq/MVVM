using System;
using Azzazelloqq.MVVM.Source.Core.Command.Base;
using Azzazelloqq.MVVM.Source.Core.Command.Commands;
using Azzazelloqq.MVVM.Source.Core.ViewModel;
using Azzazelloqq.MVVM.Source.ReactiveLibrary.Property;

namespace Azzazelloqq.MVVM.Example.Item
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