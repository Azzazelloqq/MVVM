using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azzazelloqq.MVVM.Source.Core.Model;
using Azzazelloqq.MVVM.Source.ReactiveLibrary.Collections.List;

namespace Azzazelloqq.MVVM.Example
{
public class InventoryModel : ModelBase
{
	/// <summary>
	/// Reactive list of item names. 
	/// Changes to this list will trigger notifications to subscribers.
	/// </summary>
	public IReactiveList<string> Items { get; }

	public InventoryModel()
	{
		Items = new ReactiveList<string>();
	}

	public InventoryModel(IEnumerable<string> items)
	{
		Items = new ReactiveList<string>(items);
	}

	/// <summary>
	/// Asynchronously loads inventory data (for demonstration).
	/// In a real scenario, it might load from a file or a server.
	/// </summary>
	/// <param name="path">A string that might represent a file path or URL.</param>
	/// <param name="token">A cancellation token for this async operation.</param>
	/// <returns></returns>
	public async Task LoadInventoryAsync(string path, CancellationToken token)
	{
		// Fake asynchronous delay to simulate IO operation.
		await Task.Delay(1000, token);

		// Example of adding a few items after "loading".
		Items.Add("Sword");
		Items.Add("Helmet");
		Items.Add("Healing Potion");
	}

	/// <summary>
	/// Adds a new item to the inventory. 
	/// </summary>
	/// <param name="itemName">Name of the item to add.</param>
	public void AddItem(string itemName)
	{
		if (string.IsNullOrEmpty(itemName))
		{
			return;
		}

		Items.Add(itemName);
	}

	/// <summary>
	/// Removes an item from the inventory if it exists.
	/// </summary>
	/// <param name="item">Name of the item to remove.</param>
	public void RemoveItem(string item)
	{
		Items.Remove(item);
	}

	/// <summary>
	/// Called when the model is disposed. Dispose reactive lists and other resources here.
	/// </summary>
	protected override void OnDispose()
	{
		base.OnDispose();
		Items.Dispose();
	}
}
}