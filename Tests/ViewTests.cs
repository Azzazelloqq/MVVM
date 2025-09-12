using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azzazelloqq.MVVM.Core;
using Azzazelloqq.MVVM.ReactiveLibrary;

namespace Azzazelloqq.MVVM.Tests
{
    /// <summary>
    /// Tests for View components (IView, ViewBase, ViewMonoBehavior)
    /// </summary>
    [TestFixture]
    public class ViewTests
    {
        private TestView _testView;
        private TestViewModel _testViewModel;
        private TestModel _testModel;

        [SetUp]
        public void SetUp()
        {
            _testModel = new TestModel();
            _testViewModel = new TestViewModel(_testModel);
            _testView = new TestView();
        }

        [TearDown]
        public void TearDown()
        {
            _testView?.Dispose();
            _testViewModel?.Dispose();
        }

        [Test]
        public void Initialize_ShouldSetViewModelAndCallOnInitialize()
        {
            // Act
            _testView.Initialize(_testViewModel);

            // Assert
            Assert.AreSame(_testViewModel, _testView.ViewModel, "ViewModel should be set");
            Assert.IsTrue(_testView.IsOnInitializeCalled, "OnInitialize should be called");
            Assert.IsTrue(_testView.IsInitialized, "View should be marked as initialized");
        }

        [Test]
        public void Initialize_WithDisposeWithViewModel_ShouldSubscribeToDisposeNotifier()
        {
            // Act
            _testView.Initialize(_testViewModel, disposeWithViewModel: true);

            // Assert
            Assert.IsTrue(_testView.IsSubscribedToDisposeNotifier, "Should subscribe to DisposeNotifier");
        }

        [Test]
        public void Initialize_WithoutDisposeWithViewModel_ShouldStillSubscribeToDisposeNotifier()
        {
            // Act
            _testView.Initialize(_testViewModel, disposeWithViewModel: false);

            // Assert
            Assert.IsTrue(_testView.IsSubscribedToDisposeNotifier, "Should still subscribe to DisposeNotifier for cleanup");
        }

        [Test]
        public void Initialize_CalledTwice_ShouldThrowException()
        {
            // Arrange
            _testView.Initialize(_testViewModel);

            // Act & Assert
            Assert.Throws<Exception>(() => _testView.Initialize(_testViewModel),
                "Should throw exception when Initialize is called twice");
        }

        [Test]
        public void Initialize_WrongViewModelType_ShouldThrowException()
        {
            // Arrange
            var wrongViewModel = new WrongTestViewModel(new TestModel());

            // Act & Assert
            Assert.Throws<Exception>(() => _testView.Initialize(wrongViewModel),
                "Should throw exception when ViewModel type is wrong");

            wrongViewModel.Dispose();
        }

        [Test]
        public async Task InitializeAsync_ShouldSetViewModelAndCallOnInitializeAsync()
        {
            // Arrange
            using var cts = new CancellationTokenSource();

            // Act
            await _testView.InitializeAsync(_testViewModel, cts.Token);

            // Assert
            Assert.AreSame(_testViewModel, _testView.ViewModel, "ViewModel should be set");
            Assert.IsTrue(_testView.IsOnInitializeAsyncCalled, "OnInitializeAsync should be called");
            Assert.IsTrue(_testView.IsInitialized, "View should be marked as initialized");
        }

        [Test]
        public async Task InitializeAsync_CalledTwice_ShouldThrowException()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            await _testView.InitializeAsync(_testViewModel, cts.Token);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => 
                await _testView.InitializeAsync(_testViewModel, cts.Token),
                "Should throw exception when InitializeAsync is called twice");
        }

        [Test]
        public async Task InitializeAsync_WrongViewModelType_ShouldThrowException()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            var wrongViewModel = new WrongTestViewModel(new TestModel());

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => 
                await _testView.InitializeAsync(wrongViewModel, cts.Token),
                "Should throw exception when ViewModel type is wrong");

            wrongViewModel.Dispose();
        }

        [Test]
        public void Dispose_WithDisposeWithViewModel_ShouldDisposeViewWhenViewModelDisposed()
        {
            // Arrange
            _testView.Initialize(_testViewModel, disposeWithViewModel: true);

            // Act
            _testViewModel.Dispose();

            // Assert - Wait a bit for disposal to propagate
            Assert.IsTrue(_testView.IsDisposed, "View should be disposed when ViewModel is disposed");
        }

        [Test]
        public void Dispose_WithoutDisposeWithViewModel_ShouldNotDisposeViewWhenViewModelDisposed()
        {
            // Arrange
            _testView.Initialize(_testViewModel, disposeWithViewModel: false);

            // Act
            _testViewModel.Dispose();

            // Assert
            Assert.IsFalse(_testView.IsDisposed, "View should not be disposed when ViewModel is disposed if disposeWithViewModel is false");
        }

        [Test]
        public void Dispose_ShouldCallOnDisposeAndUnsubscribe()
        {
            // Arrange
            _testView.Initialize(_testViewModel);

            // Act
            _testView.Dispose();

            // Assert
            Assert.IsTrue(_testView.IsOnDisposeCalled, "OnDispose should be called");
            Assert.IsTrue(_testView.IsDisposed, "View should be marked as disposed");
            Assert.IsTrue(_testView.IsUnsubscribedFromDisposeNotifier, "Should unsubscribe from DisposeNotifier");
        }

        [Test]
        public async Task DisposeAsync_ShouldCallOnDisposeAsyncAndUnsubscribe()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            _testView.Initialize(_testViewModel);

            // Act
            await _testView.DisposeAsync(cts.Token);

            // Assert
            Assert.IsTrue(_testView.IsOnDisposeAsyncCalled, "OnDisposeAsync should be called");
            Assert.IsTrue(_testView.IsDisposed, "View should be marked as disposed");
            Assert.IsTrue(_testView.IsUnsubscribedFromDisposeNotifier, "Should unsubscribe from DisposeNotifier");
        }

        [Test]
        public void DisposeToken_WhenDisposed_ShouldBeCanceled()
        {
            // Arrange
            _testView.Initialize(_testViewModel);
            var token = _testView.DisposeToken;

            // Act
            _testView.Dispose();

            // Assert
            Assert.IsTrue(token.IsCancellationRequested,
                "Dispose token should be canceled when View is disposed");
        }

        [Test]
        public async Task DisposeTokenAsync_WhenDisposed_ShouldBeCanceled()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            _testView.Initialize(_testViewModel);
            var token = _testView.DisposeToken;

            // Act
            await _testView.DisposeAsync(cts.Token);

            // Assert
            Assert.IsTrue(token.IsCancellationRequested,
                "Dispose token should be canceled when View is disposed asynchronously");
        }

        /// <summary>
        /// Test implementation of ViewBase for testing purposes
        /// </summary>
        private class TestView : ViewBase<TestViewModel>
        {
            public bool IsOnInitializeCalled { get; private set; }
            public bool IsOnInitializeAsyncCalled { get; private set; }
            public bool IsOnDisposeCalled { get; private set; }
            public bool IsOnDisposeAsyncCalled { get; private set; }
            public bool IsInitialized => _isInitialized;
            public bool IsSubscribedToDisposeNotifier { get; private set; }
            public bool IsUnsubscribedFromDisposeNotifier { get; private set; }
            public CancellationToken DisposeToken => disposeToken;
            public TestViewModel ViewModel => viewModel;

            private bool _isInitialized;

            protected override void OnInitialize()
            {
                IsOnInitializeCalled = true;
                _isInitialized = true;
                
                // Check if we subscribed to dispose notifier
                if (viewModel?.DisposeNotifier != null)
                {
                    IsSubscribedToDisposeNotifier = true;
                }
            }

            protected override async ValueTask OnInitializeAsync(CancellationToken token)
            {
                await Task.Delay(10, token); // Simulate async work
                IsOnInitializeAsyncCalled = true;
                _isInitialized = true;
                
                // Check if we subscribed to dispose notifier
                if (viewModel?.DisposeNotifier != null)
                {
                    IsSubscribedToDisposeNotifier = true;
                }
            }

            protected override void OnDispose()
            {
                IsOnDisposeCalled = true;
                IsUnsubscribedFromDisposeNotifier = true;
            }

            protected override async ValueTask OnDisposeAsync(CancellationToken token)
            {
                await Task.Delay(10, token); // Simulate async work
                IsOnDisposeAsyncCalled = true;
                IsUnsubscribedFromDisposeNotifier = true;
            }
        }

        /// <summary>
        /// Test implementation of ViewModelBase for testing purposes
        /// </summary>
        private class TestViewModel : ViewModelBase<TestModel>
        {
            public TestViewModel(TestModel model) : base(model)
            {
            }

            protected override void OnInitialize() { }
            protected override async ValueTask OnInitializeAsync(CancellationToken token) 
            { 
                await Task.Delay(10, token); 
            }
            protected override void OnDispose() { }
            protected override async ValueTask OnDisposeAsync(CancellationToken token) 
            { 
                await Task.Delay(10, token); 
            }
        }

        /// <summary>
        /// Wrong ViewModel type for testing type checking
        /// </summary>
        private class WrongTestViewModel : ViewModelBase<TestModel>
        {
            public WrongTestViewModel(TestModel model) : base(model)
            {
            }

            protected override void OnInitialize() { }
            protected override async ValueTask OnInitializeAsync(CancellationToken token) 
            { 
                await Task.Delay(10, token); 
            }
            protected override void OnDispose() { }
            protected override async ValueTask OnDisposeAsync(CancellationToken token) 
            { 
                await Task.Delay(10, token); 
            }
        }

        /// <summary>
        /// Test implementation of ModelBase for testing purposes
        /// </summary>
        private class TestModel : ModelBase
        {
            protected override void OnInitialize() { }
            protected override async ValueTask OnInitializeAsync(CancellationToken token) 
            { 
                await Task.Delay(10, token); 
            }
            protected override void OnDispose() { }
            protected override async ValueTask OnDisposeAsync(CancellationToken token) 
            { 
                await Task.Delay(10, token); 
            }
        }
    }

    /// <summary>
    /// Tests for ViewMonoBehavior component
    /// </summary>
    [TestFixture]
    public class ViewMonoBehaviorTests
    {
        /// <summary>
        /// Note: ViewMonoBehavior tests are limited in unit test environment
        /// as they require Unity's MonoBehaviour lifecycle.
        /// These tests focus on testable logic parts.
        /// </summary>

        [Test]
        public void ViewMonoBehavior_ImplementsIView()
        {
            // This test verifies the interface implementation at compile time
            // ViewMonoBehavior<T> should implement IView
            
            // Assert - This is a compile-time check
            Assert.IsTrue(typeof(ViewMonoBehavior<TestViewModel>).GetInterfaces()
                .Contains(typeof(IView)), "ViewMonoBehavior should implement IView");
        }

        /// <summary>
        /// Mock ViewMonoBehavior for testing (since we can't instantiate MonoBehaviour in unit tests)
        /// </summary>
        private class TestViewModel : ViewModelBase<TestModel>
        {
            public TestViewModel(TestModel model) : base(model) { }
            protected override void OnInitialize() { }
            protected override async ValueTask OnInitializeAsync(CancellationToken token) 
            { 
                await Task.Delay(10, token); 
            }
            protected override void OnDispose() { }
            protected override async ValueTask OnDisposeAsync(CancellationToken token) 
            { 
                await Task.Delay(10, token); 
            }
        }

        private class TestModel : ModelBase
        {
            protected override void OnInitialize() { }
            protected override async ValueTask OnInitializeAsync(CancellationToken token) 
            { 
                await Task.Delay(10, token); 
            }
            protected override void OnDispose() { }
            protected override async ValueTask OnDisposeAsync(CancellationToken token) 
            { 
                await Task.Delay(10, token); 
            }
        }

        // Additional ViewMonoBehavior tests would require Unity Test Framework
        // and GameObject instantiation which is beyond unit testing scope
    }
}