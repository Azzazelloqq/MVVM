#if !PROJECT_SUPPORT_R3
using System.Threading;
using System.Threading.Tasks;
using Azzazelloqq.MVVM.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Azzazelloqq.MVVM.Example.Item
{
/// <summary>
/// A Unity View for displaying a single item (name + remove button).
/// Binds to <see cref="ItemViewModel"/>.
/// </summary>
internal class ItemView : ViewMonoBehavior<ItemViewModel>
{
	[SerializeField]
	private Text _itemNameText;

	[SerializeField]
	private Button _removeButton;

	protected override void OnInitialize()
	{
		// Bind item name
		viewModel.ItemName.Subscribe(OnItemNameChanged);

		// Hook up remove button
		_removeButton.onClick.AddListener(OnRemoveButtonClicked);
	}

	protected override ValueTask OnInitializeAsync(CancellationToken token)
	{
		return default;
	}

	protected override ValueTask OnDisposeAsync(CancellationToken token)
	{
		return default;
	}

	protected override void OnDispose()
	{
		// Unsubscribe from item name changes
		viewModel.ItemName.Unsubscribe(OnItemNameChanged);

		// Remove listener on remove button
		_removeButton.onClick.RemoveListener(OnRemoveButtonClicked);
	}

	private void OnItemNameChanged(string itemName)
	{
		_itemNameText.text = itemName;
	}

	private void OnRemoveButtonClicked()
	{
		viewModel.RemoveItemCommand.Execute();
	}
}
}
#endif
