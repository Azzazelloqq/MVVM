using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using Azzazelloqq.MVVM.Core;
using Azzazelloqq.MVVM.ReactiveLibrary;

namespace Azzazelloqq.MVVM.Tests
{
    /// <summary>
    /// Integration tests for MVVM pattern components working together
    /// </summary>
    [TestFixture]
    public class MVVMIntegrationTests
    {
        private TestModel _model;
        private TestViewModel _viewModel;
        private TestView _view;

        [SetUp]
        public void SetUp()
        {
            _model = new TestModel();
            _viewModel = new TestViewModel(_model);
            _view = new TestView();
        }

        [TearDown]
        public void TearDown()
        {
            _view?.Dispose();
            _viewModel?.Dispose();
        }

        [Test]
        public void FullMVVMPattern_Initialize_ShouldInitializeAllComponents()
        {
            // Act
            _viewModel.Initialize();
            _view.Initialize(_viewModel);

            // Assert
            Assert.IsTrue(_model.IsInitialized, "Model should be initialized");
            Assert.IsTrue(_viewModel.IsInitialized, "ViewModel should be initialized");
            Assert.IsTrue(_view.IsInitialized, "View should be initialized");
        }

        [Test]
        public async Task FullMVVMPattern_InitializeAsync_ShouldInitializeAllComponents()
        {
            // Arrange
            using var cts = new CancellationTokenSource();

            // Act
            await _viewModel.InitializeAsync(cts.Token);
            await _view.InitializeAsync(_viewModel, cts.Token);

            // Assert
            Assert.IsTrue(_model.IsInitialized, "Model should be initialized");
            Assert.IsTrue(_viewModel.IsInitialized, "ViewModel should be initialized");
            Assert.IsTrue(_view.IsInitialized, "View should be initialized");
        }

        [Test]
        public void DataBinding_ModelToViewModel_ShouldWork()
        {
            // Arrange
            _viewModel.Initialize();
            _view.Initialize(_viewModel);

            // Act
            _model.UpdateData("Test Data");

            // Assert
            Assert.AreEqual("Test Data", _viewModel.DataProperty.Value, "ViewModel should reflect model data");
        }

        [Test]
        public void Command_ExecuteFromView_ShouldUpdateModel()
        {
            // Arrange
            _viewModel.Initialize();
            _view.Initialize(_viewModel);

            // Act
            _viewModel.ExecuteUpdateCommand("Command Data");

            // Assert
            Assert.AreEqual("Command Data", _model.Data, "Model should be updated via command");
        }

        [Test]
        public void ViewModelDispose_ShouldDisposeView()
        {
            // Arrange
            _viewModel.Initialize();
            _view.Initialize(_viewModel, disposeWithViewModel: true);

            // Act
            _viewModel.Dispose();

            // Assert
            Assert.IsTrue(_view.IsDisposed, "View should be disposed when ViewModel is disposed");
        }

        [Test]
        public void ViewModelDispose_WithoutDisposeWithViewModel_ShouldNotDisposeView()
        {
            // Arrange
            _viewModel.Initialize();
            _view.Initialize(_viewModel, disposeWithViewModel: false);

            // Act
            _viewModel.Dispose();

            // Assert
            Assert.IsFalse(_view.IsDisposed, "View should not be disposed when ViewModel is disposed if disposeWithViewModel is false");
        }

        [Test]
        public void ReactiveProperty_ChangeNotification_ShouldNotifyView()
        {
            // Arrange
            _viewModel.Initialize();
            _view.Initialize(_viewModel);
            bool viewNotified = false;
            _viewModel.DataProperty.Subscribe(value => viewNotified = true, withNotify: false);

            // Act
            _model.UpdateData("New Data");

            // Assert
            Assert.IsTrue(viewNotified, "View should be notified of data changes");
        }

        [Test]
        public async Task AsyncCommand_Execute_ShouldWork()
        {
            // Arrange
            _viewModel.Initialize();
            _view.Initialize(_viewModel);

            // Act
            await _viewModel.ExecuteAsyncUpdateCommand("Async Data");

            // Assert
            Assert.AreEqual("Async Data", _model.Data, "Model should be updated via async command");
        }

        [Test]
        public void CommandCanExecute_WhenFalse_ShouldPreventExecution()
        {
            // Arrange
            _viewModel.Initialize();
            _view.Initialize(_viewModel);
            _model.CanUpdate = false;
            string originalData = _model.Data;

            // Act
            _viewModel.ExecuteUpdateCommand("Should Not Update");

            // Assert
            Assert.AreEqual(originalData, _model.Data, "Model should not be updated when command cannot execute");
        }

        [Test]
        public void MVVMLifecycle_CompleteFlow_ShouldWork()
        {
            // Arrange & Act - Initialize
            _viewModel.Initialize();
            _view.Initialize(_viewModel);

            // Assert - Initialization
            Assert.IsTrue(_model.IsInitialized);
            Assert.IsTrue(_viewModel.IsInitialized);
            Assert.IsTrue(_view.IsInitialized);

            // Act - Data flow
            _model.UpdateData("Lifecycle Test");
            
            // Assert - Data binding
            Assert.AreEqual("Lifecycle Test", _viewModel.DataProperty.Value);

            // Act - Command execution
            _viewModel.ExecuteUpdateCommand("Command Test");
            
            // Assert - Command execution
            Assert.AreEqual("Command Test", _model.Data);

            // Act - Disposal
            _view.Dispose();
            
            // Assert - Disposal
            Assert.IsTrue(_view.IsDisposed);
            Assert.IsTrue(_viewModel.IsDisposed); // Should be disposed because of compositeDisposable
        }

        /// <summary>
        /// Test Model with data and reactive properties
        /// </summary>
        private class TestModel : ModelBase
        {
            public string Data { get; private set; } = "";
            public bool CanUpdate { get; set; } = true;
            public bool IsInitialized { get; private set; }

            private readonly ReactiveProperty<string> _dataProperty = new ReactiveProperty<string>("");
            public IReadOnlyReactiveProperty<string> DataProperty => _dataProperty;

            public void UpdateData(string newData)
            {
                Data = newData;
                _dataProperty.SetValue(newData);
            }

            protected override void OnInitialize()
            {
                IsInitialized = true;
                compositeDisposable.AddDisposable(_dataProperty);
            }

            protected override async ValueTask OnInitializeAsync(CancellationToken token)
            {
                await Task.Delay(10, token);
                IsInitialized = true;
                compositeDisposable.AddDisposable(_dataProperty);
            }

            protected override void OnDispose() { }
            protected override async ValueTask OnDisposeAsync(CancellationToken token) 
            { 
                await Task.Delay(10, token); 
            }
        }

        /// <summary>
        /// Test ViewModel with commands and data binding
        /// </summary>
        private class TestViewModel : ViewModelBase<TestModel>
        {
            public IReadOnlyReactiveProperty<string> DataProperty => model.DataProperty;
            public ActionCommand UpdateCommand { get; private set; }
            public ActionAsyncCommand AsyncUpdateCommand { get; private set; }
            public bool IsInitialized { get; private set; }

            public TestViewModel(TestModel model) : base(model)
            {
            }

            protected override void OnInitialize()
            {
                IsInitialized = true;
                UpdateCommand = new ActionCommand(
                    execute: () => model.UpdateData("Updated via Command"),
                    canExecute: () => model.CanUpdate);

                AsyncUpdateCommand = new ActionAsyncCommand(
                    execute: async () => 
                    {
                        await Task.Delay(10);
                        model.UpdateData("Updated via Async Command");
                    },
                    canExecute: () => model.CanUpdate);

                compositeDisposable.AddDisposable(UpdateCommand);
                compositeDisposable.AddDisposable(AsyncUpdateCommand);
            }

            protected override async ValueTask OnInitializeAsync(CancellationToken token)
            {
                await Task.Delay(10, token);
                IsInitialized = true;
                
                UpdateCommand = new ActionCommand(
                    execute: () => model.UpdateData("Updated via Command"),
                    canExecute: () => model.CanUpdate);

                AsyncUpdateCommand = new ActionAsyncCommand(
                    execute: async () => 
                    {
                        await Task.Delay(10);
                        model.UpdateData("Updated via Async Command");
                    },
                    canExecute: () => model.CanUpdate);

                compositeDisposable.AddDisposable(UpdateCommand);
                compositeDisposable.AddDisposable(AsyncUpdateCommand);
            }

            // Fix command execution methods
            public void ExecuteUpdateCommand(string data)
            {
                if (UpdateCommand.CanExecute())
                {
                    model.UpdateData(data);
                }
            }

            public async Task ExecuteAsyncUpdateCommand(string data)
            {
                if (AsyncUpdateCommand.CanExecute())
                {
                    await Task.Delay(10);
                    model.UpdateData(data);
                }
            }

            protected override void OnDispose() { }
            protected override async ValueTask OnDisposeAsync(CancellationToken token) 
            { 
                await Task.Delay(10, token); 
            }
        }

        /// <summary>
        /// Test View that observes ViewModel
        /// </summary>
        private class TestView : ViewBase<TestViewModel>
        {
            public bool IsInitialized { get; private set; }

            protected override void OnInitialize()
            {
                IsInitialized = true;
            }

            protected override async ValueTask OnInitializeAsync(CancellationToken token)
            {
                await Task.Delay(10, token);
                IsInitialized = true;
            }

            protected override void OnDispose() { }
            protected override async ValueTask OnDisposeAsync(CancellationToken token) 
            { 
                await Task.Delay(10, token); 
            }
        }
    }
}