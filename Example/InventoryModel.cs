using System.Collections.Generic;
using MVVM.MVVM.ReactiveLibrary.Collections.List;
using MVVM.MVVM.System.Base.Model;

namespace MVVM.MVVM.Example
{
public class InventoryModel : ModelBase
{
	public IReactiveList<string> Items { get; }

	public InventoryModel()
	{
		Items = new ReactiveList<string>();
	}

	public InventoryModel(IEnumerable<string> items)
	{
		Items = new ReactiveList<string>(items);
	}

	public void AddItem(string itemName)
	{
		if (string.IsNullOrEmpty(itemName))
		{
			return;
		}
		
		Items.Add(itemName);
	}

	public void RemoveItem(string item)
	{
		Items.Remove(item);
	}

	protected override void OnDispose()
	{
		base.OnDispose();
		
		Items.Dispose();
	}
}
}