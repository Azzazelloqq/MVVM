using MVVM.MVVM.System.Base.View;
using UnityEngine;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;

namespace MVVM.MVVM.Example.Item
{
public class ItemView : ViewMonoBehavior<ItemViewModel>
{
	[SerializeField]
	private Text _itemName;

	protected override void OnInitialize()
	{
		base.OnInitialize();
		
		viewModel.ItemName.Subscribe(OnItemNameChanged);
	}

	private void OnItemNameChanged(string itemName)
	{
		_itemName.text = itemName;
	}
}
}