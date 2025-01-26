using MVVM.MVVM.ReactiveLibrary.Property;
using MVVM.MVVM.System.Base.Model;

namespace MVVM.MVVM.Example.Item
{
/// <summary>
/// The data model for a single item in the inventory.
/// Holds a reactive property for the item's name.
/// </summary>
public class ItemModel : ModelBase
{
	public IReadOnlyReactiveProperty<string> ItemName { get; }

	public ItemModel(string itemName)
	{
		ItemName = new ReactiveProperty<string>(itemName);
	}

	protected override void OnDispose()
	{
		ItemName.Dispose();
		base.OnDispose();
	}
}
}