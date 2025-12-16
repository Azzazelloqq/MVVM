using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using Azzazelloqq.MVVM.Core;
#if PROJECT_SUPPORT_R3
using R3;
#endif

namespace Azzazelloqq.MVVM.Tests
{
#if PROJECT_SUPPORT_R3
    [TestFixture]
    public class R3MvvmBridgeTests
    {
        [Test]
        public void DisposeNotifier_ShouldEmitUnit_WhenViewModelDisposed()
        {
            var model = new DummyModel();
            var viewModel = new DummyViewModel(model);

            try
            {
                bool notified = false;
                using var subscription = viewModel.DisposeNotifier.Subscribe(_ => notified = true);

                viewModel.Dispose();

                Assert.IsTrue(notified, "R3 notifier should emit a value when ViewModel is disposed");
            }
            finally
            {
                viewModel.Dispose();
            }
        }

        [Test]
        public void IsInitializedReactiveProperty_ShouldFlipToTrue_AfterInitialize()
        {
            var viewModel = new DummyViewModel(new DummyModel());
            try
            {
                var isInitialized = ((IViewModel)viewModel).IsInitialized;
                Assert.IsFalse(isInitialized.CurrentValue, "Initial value should be false");

                viewModel.Initialize();

                Assert.IsTrue(isInitialized.CurrentValue, "Value should become true after Initialize");
            }
            finally
            {
                viewModel.Dispose();
            }
        }

        private sealed class DummyModel : ModelBase
        {
            protected override void OnInitialize() { }
            protected override ValueTask OnInitializeAsync(CancellationToken token) => default;
            protected override void OnDispose() { }
            protected override ValueTask OnDisposeAsync(CancellationToken token) => default;
        }

        private sealed class DummyViewModel : ViewModelBase<DummyModel>
        {
            public DummyViewModel(DummyModel model) : base(model) { }

            protected override void OnInitialize() { }
            protected override ValueTask OnInitializeAsync(CancellationToken token) => default;
            protected override void OnDispose() { }
            protected override ValueTask OnDisposeAsync(CancellationToken token) => default;
        }
    }
#endif
}