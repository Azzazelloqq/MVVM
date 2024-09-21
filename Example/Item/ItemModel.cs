using MVVM.MVVM.ReactiveLibrary.Property;
using MVVM.MVVM.System.Base.Model;

namespace MVVM.MVVM.Example.Item
{
public class ItemModel : ModelBase
{
	public IReadOnlyReactiveProperty<string> ItemName { get; }

	public ItemModel(string itemName)
	{
		ItemName = new ReactiveProperty<string>(itemName);
	}
}
}