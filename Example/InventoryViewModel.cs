using System.Threading;
using System.Threading.Tasks;
using Azzazelloqq.MVVM.Example.Item;
using Azzazelloqq.MVVM.Source.Core.Command.Base;
using Azzazelloqq.MVVM.Source.Core.Command.Commands;
using Azzazelloqq.MVVM.Source.Core.ViewModel;
using Azzazelloqq.MVVM.Source.ReactiveLibrary.Collections.List;
using Azzazelloqq.MVVM.Source.ReactiveLibrary.Property;

namespace Azzazelloqq.MVVM.Example
{
    public class InventoryViewModel : ViewModelBase<InventoryModel>
    {
        public IReactiveProperty<string> NewItemName { get; }
        public IRelayCommand<string> AddItemCommand { get; }
        public IRelayCommand<string> RemoveItemCommand { get; }
        public IAsyncCommand<string> LoadInventoryCommand { get; }
        public IReactiveList<ItemViewModel> Items { get; }

        public InventoryViewModel(InventoryModel model) : base(model)
        {
            // Initialize the reactive list of item viewmodels
            Items = new ReactiveList<ItemViewModel>(model.Items.Count);

            // Property for name of new item
            NewItemName = new ReactiveProperty<string>();

            // A RelayCommand to add items, with a basic canExecute check that it's not empty
            AddItemCommand = new RelayCommand<string>(
                OnAddItemCommandExecute,
                CanAddItemCommandExecute
            );

            // A RelayCommand to remove items by name
            RemoveItemCommand = new RelayCommand<string>(model.RemoveItem);

            // An AsyncRelayCommand to load inventory from a path
            LoadInventoryCommand = new AsyncRelayCommand<string>(OnLoadInventoryCommandExecuteAsync);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            // Populate from the existing model list
            InitializeItemsFromModel();

            // Subscribe to model list changes
            model.Items.SubscribeOnItemAdded(OnItemAdded);
            model.Items.SubscribeOnItemRemoved(OnItemRemoved);

            // Also subscribe to NewItemName changes to refresh canExecute (optional)
            NewItemName.Subscribe(_ => RaiseAddItemCanExecuteChanged());
        }

        protected override async Task OnInitializeAsync(CancellationToken token)
        {
            // Optionally do something async here
            await base.OnInitializeAsync(token);
        }

        protected override void OnDispose()
        {
            // Unsubscribe from model changes
            model.Items.UnsubscribeOnItemAdded(OnItemAdded);
            model.Items.UnsubscribeOnItemRemoved(OnItemRemoved);

            // Dispose reactive properties and commands
            NewItemName.Dispose();
            AddItemCommand.Dispose();
            RemoveItemCommand.Dispose();
            LoadInventoryCommand.Dispose();
            Items.Dispose();

            base.OnDispose();
        }

        private void OnItemAdded(string itemName)
        {
            AddItemViewModel(itemName);
        }

        private void OnItemRemoved(string itemName)
        {
            Items.Remove(vm => vm.ItemName.Value == itemName);
        }

        private void InitializeItemsFromModel()
        {
            foreach (var modelItem in model.Items)
            {
                AddItemViewModel(modelItem);
            }
        }

        private void AddItemViewModel(string itemName)
        {
            // Create a new ItemModel
            var itemModel = new ItemModel(itemName);

            // Pass a callback for removing this item from Inventory
            var itemViewModel = new ItemViewModel(
                itemModel,
                RemoveItemFromItemViewModel // callback
            );

            itemViewModel.Initialize(); // Synchronous init, or you can do async if needed
            Items.Add(itemViewModel);
        }

        private void RemoveItemFromItemViewModel(string itemName)
        {
            model.RemoveItem(itemName);
        }

        private void OnAddItemCommandExecute(string itemName)
        {
            model.AddItem(itemName);
        }

        private bool CanAddItemCommandExecute()
        {
            return !string.IsNullOrEmpty(NewItemName.Value);
        }

        private void RaiseAddItemCanExecuteChanged()
        {
            // If your RelayCommand supports a method to notify the view about changes in can-execute, call it here.
            // Example:
            // (AddItemCommand as RelayCommand<string>)?.RaiseCanExecuteChanged();
        }

        private async Task OnLoadInventoryCommandExecuteAsync(string path)
        {
            await model.LoadInventoryAsync(path, disposeToken);
            // Model will trigger OnItemAdded for each new item, 
            // so the UI will be updated automatically.
        }
    }
}
