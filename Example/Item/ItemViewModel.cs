using System;
using System.Threading;
using System.Threading.Tasks;
using Azzazelloqq.MVVM.Core;
using Azzazelloqq.MVVM.ReactiveLibrary;

namespace Azzazelloqq.MVVM.Example.Item
{
/// <summary>
/// The ViewModel for a single item. 
/// Demonstrates a callback approach to remove this item from the parent Inventory.
/// </summary>
internal class ItemViewModel : ViewModelBase<ItemModel>
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

	protected override void OnInitialize()
	{
	}

	protected override ValueTask OnInitializeAsync(CancellationToken token)
	{
		return default;
	}

	protected override void OnDispose()
	{
		RemoveItemCommand.Dispose();
	}

	protected override ValueTask OnDisposeAsync(CancellationToken token)
	{
		return default;
	}

	private void OnRemoveItem()
	{
		_removeItemCallback?.Invoke(ItemName.Value);
	}
}
}