#if !PROJECT_SUPPORT_R3
using System.Threading;
using System.Threading.Tasks;
using Azzazelloqq.MVVM.Core;
using Azzazelloqq.MVVM.ReactiveLibrary;

namespace Azzazelloqq.MVVM.Example.Item
{
/// <summary>
/// The data model for a single item in the inventory.
/// Holds a reactive property for the item's name.
/// </summary>
internal class ItemModel : ModelBase
{
	public IReadOnlyReactiveProperty<string> ItemName { get; }

	public ItemModel(string itemName)
	{
		ItemName = new ReactiveProperty<string>(itemName);
	}

	protected override ValueTask OnInitializeAsync(CancellationToken token)
	{
		return default;
	}

	protected override void OnInitialize()
	{
	}

	protected override void OnDispose()
	{
		ItemName.Dispose();
	}

	protected override ValueTask OnDisposeAsync(CancellationToken token)
	{
		return default;
	}
}
}
#endif
