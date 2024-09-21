using MVVM.MVVM.ReactiveLibrary.Property;
using MVVM.MVVM.System.Base.ViewModel;

namespace MVVM.MVVM.Example.Item
{
public class ItemViewModel : ViewModelBase<ItemModel>
{
	public IReadOnlyReactiveProperty<string> ItemName => model.ItemName;
	
	public ItemViewModel(ItemModel model) : base(model)
	{
	}
}
}