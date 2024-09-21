using System.Linq;
using MVVM.MVVM.Example.Item;
using MVVM.MVVM.ReactiveLibrary.Collections.List;
using MVVM.MVVM.ReactiveLibrary.Property;
using MVVM.MVVM.System.Base.Command.Base;
using MVVM.MVVM.System.Base.Command.Commands;
using MVVM.MVVM.System.Base.ViewModel;
using NotImplementedException = System.NotImplementedException;

namespace MVVM.MVVM.Example
{
public class InventoryViewModel : ViewModelBase<InventoryModel>
{
	public IReactiveProperty<string> NewItemName { get; }
	public IRelayCommand<string> AddItemCommand { get; }
	public IRelayCommand<string> RemoveItemCommand { get; }
	public IReactiveList<ItemViewModel> Items { get; }

	public InventoryViewModel(InventoryModel model) : base(model)
	{
		Items = new ReactiveList<ItemViewModel>(model.Items.Count);
		NewItemName = new ReactiveProperty<string>();
		AddItemCommand = new RelayCommand<string>(OnAddItemCommandExecute);
		RemoveItemCommand = new RelayCommand<string>(model.RemoveItem);
	}

	protected override void OnInitialize()
	{
		InitializeItems();
		
		model.Items.SubscribeOnItemAdded(OnItemAdded);
		model.Items.SubscribeOnItemRemoved(OnItemRemoved);
	}

	public override void Dispose()
	{
		base.Dispose();
		
		NewItemName.Dispose();
		AddItemCommand.Dispose();
		RemoveItemCommand.Dispose();
		Items.Dispose();
	}

	private void OnItemAdded(string item)
	{
		AddItem(item);
	}

	private void OnItemRemoved(string item)
	{
		RemoveItem(item);
	}

	private void InitializeItems()
	{
		foreach (var modelItem in model.Items)
		{
			AddItem(modelItem);
		}
	}

	private void AddItem(string item)
	{
		var itemModel = new ItemModel(item);
		var itemViewModel = new ItemViewModel(itemModel);
		itemViewModel.Initialize();
		
		Items.Add(itemViewModel);
	}

	private void RemoveItem(string item)
	{
		Items.Remove(x => x.ItemName.Value == item);
	}

	private void OnAddItemCommandExecute(string itemName)
	{
		model.AddItem(itemName);
	}
}
}