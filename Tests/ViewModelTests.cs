using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using Azzazelloqq.MVVM.Core;
#if PROJECT_SUPPORT_R3
using R3;
#else
using Azzazelloqq.MVVM.ReactiveLibrary;
#endif

namespace Azzazelloqq.MVVM.Tests
{
    /// <summary>
    /// Tests for ViewModel components (IViewModel, ViewModelBase)
    /// </summary>
    [TestFixture]
    public class ViewModelTests
    {
        private TestViewModel _testViewModel;
        private TestModel _testModel;

        [SetUp]
        public void SetUp()
        {
            _testModel = new TestModel();
            _testViewModel = new TestViewModel(_testModel);
        }

        [TearDown]
        public void TearDown()
        {
            _testViewModel?.Dispose();
        }

        [Test]
        public void Constructor_ShouldAddModelToCompositeDisposable()
        {
            // Assert
            Assert.IsNotNull(_testViewModel.Model, "Model should be assigned");
            Assert.AreSame(_testModel, _testViewModel.Model, "Model should be the same instance");
        }

        [Test]
        public void Initialize_ShouldCallModelInitializeAndOnInitialize()
        {
            // Act
            _testViewModel.Initialize();

            // Assert
            Assert.IsTrue(_testModel.IsOnInitializeCalled, "Model OnInitialize should be called");
            Assert.IsTrue(_testViewModel.IsOnInitializeCalled, "ViewModel OnInitialize should be called");
            Assert.IsTrue(_testViewModel.IsInitialized, "ViewModel should be marked as initialized");
        }

        [Test]
        public void Initialize_CalledTwice_ShouldThrowException()
        {
            // Arrange
            _testViewModel.Initialize();

            // Act & Assert
            Assert.Throws<Exception>(() => _testViewModel.Initialize(),
                "Should throw exception when Initialize is called twice");
        }

        [Test]
        public async Task InitializeAsync_ShouldCallModelInitializeAsyncAndOnInitializeAsync()
        {
            // Arrange
            using var cts = new CancellationTokenSource();

            // Act
            await _testViewModel.InitializeAsync(cts.Token);

            // Assert
            Assert.IsTrue(_testModel.IsOnInitializeAsyncCalled, "Model OnInitializeAsync should be called");
            Assert.IsTrue(_testViewModel.IsOnInitializeAsyncCalled, "ViewModel OnInitializeAsync should be called");
            Assert.IsTrue(_testViewModel.IsInitialized, "ViewModel should be marked as initialized");
        }

        [Test]
        public async Task InitializeAsync_CalledTwice_ShouldThrowException()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            await _testViewModel.InitializeAsync(cts.Token);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () =>
                await _testViewModel.InitializeAsync(cts.Token),
                "Should throw exception when InitializeAsync is called twice");
        }

        [Test]
        public void DisposeNotifier_ShouldNotBeNull()
        {
            // Assert
            Assert.IsNotNull(_testViewModel.DisposeNotifier, "DisposeNotifier should not be null");
        }

        [Test]
        public void Dispose_ShouldNotifyDisposeNotifierAndCallOnDispose()
        {
            // Arrange
            bool notifierCalled = false;
            using var subscription = SubscribeToDisposeNotifier(_testViewModel, () => notifierCalled = true);

            // Act
            _testViewModel.Dispose();

            // Assert
            Assert.IsTrue(notifierCalled, "DisposeNotifier should be called");
            Assert.IsTrue(_testViewModel.IsOnDisposeCalled, "OnDispose should be called");
            Assert.IsTrue(_testViewModel.IsDisposed, "ViewModel should be marked as disposed");
        }

        [Test]
        public async Task DisposeAsync_ShouldNotifyDisposeNotifierAndCallOnDisposeAsync()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            bool notifierCalled = false;
            using var subscription = SubscribeToDisposeNotifier(_testViewModel, () => notifierCalled = true);

            // Act
            await _testViewModel.DisposeAsync(cts.Token);

            // Assert
            Assert.IsTrue(notifierCalled, "DisposeNotifier should be called");
            Assert.IsTrue(_testViewModel.IsOnDisposeAsyncCalled, "OnDisposeAsync should be called");
            Assert.IsTrue(_testViewModel.IsDisposed, "ViewModel should be marked as disposed");
        }

#if PROJECT_SUPPORT_R3
        [Test]
        public void R3_IsInitializedReactiveProperty_ShouldReflectInitializationState()
        {
            var isInitialized = ((IViewModel)_testViewModel).IsInitialized;
            Assert.IsFalse(isInitialized.CurrentValue, "Initial state should be false");

            _testViewModel.Initialize();

            Assert.IsTrue(isInitialized.CurrentValue, "Reactive property should be true after initialization");
        }
#endif

        [Test]
        public void DisposeToken_WhenDisposed_ShouldBeCanceled()
        {
            // Arrange
            var token = _testViewModel.DisposeToken;
            
            // Act
            _testViewModel.Dispose();

            // Assert
            Assert.IsTrue(token.IsCancellationRequested,
                "Dispose token should be canceled when ViewModel is disposed");
        }

        [Test]
        public async Task DisposeTokenAsync_WhenDisposed_ShouldBeCanceled()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            var token = _testViewModel.DisposeToken;

            // Act
            await _testViewModel.DisposeAsync(cts.Token);

            // Assert
            Assert.IsTrue(token.IsCancellationRequested,
                "Dispose token should be canceled when ViewModel is disposed asynchronously");
        }

        /// <summary>
        /// Test implementation of ViewModelBase for testing purposes
        /// </summary>
        private class TestViewModel : ViewModelBase<TestModel>
        {
            public bool IsOnInitializeCalled { get; private set; }
            public bool IsOnInitializeAsyncCalled { get; private set; }
            public bool IsOnDisposeCalled { get; private set; }
            public bool IsOnDisposeAsyncCalled { get; private set; }
            public bool IsInitialized => _isInitialized;
            public CancellationToken DisposeToken => disposeToken;
            public TestModel Model => model;

            private bool _isInitialized;

            public TestViewModel(TestModel model) : base(model)
            {
            }

            protected override void OnInitialize()
            {
                IsOnInitializeCalled = true;
                _isInitialized = true;
            }

            protected override async ValueTask OnInitializeAsync(CancellationToken token)
            {
                await Task.Delay(10, token); // Simulate async work
                IsOnInitializeAsyncCalled = true;
                _isInitialized = true;
            }

            protected override void OnDispose()
            {
                IsOnDisposeCalled = true;
            }

            protected override async ValueTask OnDisposeAsync(CancellationToken token)
            {
                await Task.Delay(10, token); // Simulate async work
                IsOnDisposeAsyncCalled = true;
            }
        }

        /// <summary>
        /// Test implementation of ModelBase for testing purposes
        /// </summary>
        private class TestModel : ModelBase
        {
            public bool IsOnInitializeCalled { get; private set; }
            public bool IsOnInitializeAsyncCalled { get; private set; }
            public bool IsOnDisposeCalled { get; private set; }
            public bool IsOnDisposeAsyncCalled { get; private set; }

            protected override void OnInitialize()
            {
                IsOnInitializeCalled = true;
            }

            protected override async ValueTask OnInitializeAsync(CancellationToken token)
            {
                await Task.Delay(10, token); // Simulate async work
                IsOnInitializeAsyncCalled = true;
            }

            protected override void OnDispose()
            {
                IsOnDisposeCalled = true;
            }

            protected override async ValueTask OnDisposeAsync(CancellationToken token)
            {
                await Task.Delay(10, token); // Simulate async work
                IsOnDisposeAsyncCalled = true;
            }
        }

        private IDisposable? SubscribeToDisposeNotifier(IViewModel viewModel, Action onNotify)
        {
#if PROJECT_SUPPORT_R3
            return viewModel.DisposeNotifier.Subscribe(_ => onNotify());
#else
            viewModel.DisposeNotifier.Subscribe(onNotify);
            return null;
#endif
        }
    }
}