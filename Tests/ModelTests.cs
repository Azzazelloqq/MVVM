using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using Azzazelloqq.MVVM.Core;

namespace Azzazelloqq.MVVM.Tests
{
    /// <summary>
    /// Tests for Model components (IModel, ModelBase)
    /// </summary>
    [TestFixture]
    public class ModelTests
    {
        private TestModel _testModel;

        [SetUp]
        public void SetUp()
        {
            _testModel = new TestModel();
        }

        [TearDown]
        public void TearDown()
        {
            _testModel?.Dispose();
        }

        [Test]
        public void Initialize_ShouldCallOnInitialize()
        {
            // Act
            ((IModel)_testModel).Initialize();

            // Assert
            Assert.IsTrue(_testModel.IsOnInitializeCalled, "OnInitialize should be called");
            Assert.IsTrue(_testModel.IsInitialized, "Model should be marked as initialized");
        }

        [Test]
        public void Initialize_CalledTwice_ShouldThrowException()
        {
            // Arrange
            ((IModel)_testModel).Initialize();

            // Act & Assert
            Assert.Throws<Exception>(() => ((IModel)_testModel).Initialize(), 
                "Should throw exception when Initialize is called twice");
        }

        [Test]
        public async Task InitializeAsync_ShouldCallOnInitializeAsync()
        {
            // Arrange
            using var cts = new CancellationTokenSource();

            // Act
            await ((IModel)_testModel).InitializeAsync(cts.Token);

            // Assert
            Assert.IsTrue(_testModel.IsOnInitializeAsyncCalled, "OnInitializeAsync should be called");
            Assert.IsTrue(_testModel.IsInitialized, "Model should be marked as initialized");
        }

        [Test]
        public async Task InitializeAsync_CalledTwice_ShouldThrowException()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            await ((IModel)_testModel).InitializeAsync(cts.Token);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => 
                await ((IModel)_testModel).InitializeAsync(cts.Token),
                "Should throw exception when InitializeAsync is called twice");
        }

        [Test]
        public void Dispose_ShouldCallOnDispose()
        {
            // Act
            _testModel.Dispose();

            // Assert
            Assert.IsTrue(_testModel.IsOnDisposeCalled, "OnDispose should be called");
            Assert.IsTrue(_testModel.IsDisposed, "Model should be marked as disposed");
        }

        [Test]
        public async Task DisposeAsync_ShouldCallOnDisposeAsync()
        {
            // Arrange
            using var cts = new CancellationTokenSource();

            // Act
            await _testModel.DisposeAsync(cts.Token);

            // Assert
            Assert.IsTrue(_testModel.IsOnDisposeAsyncCalled, "OnDisposeAsync should be called");
            Assert.IsTrue(_testModel.IsDisposed, "Model should be marked as disposed");
        }

        [Test]
        public void DisposeToken_WhenDisposed_ShouldBeCanceled()
        {
            // Act
            _testModel.Dispose();

            // Assert
            Assert.IsTrue(_testModel.DisposeToken.IsCancellationRequested, 
                "Dispose token should be canceled when model is disposed");
        }

        [Test]
        public async Task DisposeTokenAsync_WhenDisposed_ShouldBeCanceled()
        {
            // Arrange
            using var cts = new CancellationTokenSource();

            // Act
            await _testModel.DisposeAsync(cts.Token);

            // Assert
            Assert.IsTrue(_testModel.DisposeToken.IsCancellationRequested, 
                "Dispose token should be canceled when model is disposed asynchronously");
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
            public bool IsInitialized => _isInitialized;
            public CancellationToken DisposeToken => disposeToken;

            private bool _isInitialized;

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
    }
}