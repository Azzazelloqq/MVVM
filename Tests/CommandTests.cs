using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Azzazelloqq.MVVM.Core;

namespace Azzazelloqq.MVVM.Tests
{
    /// <summary>
    /// Tests for Command components
    /// </summary>
    [TestFixture]
    public class CommandTests
    {
        [Test]
        public void ActionCommand_Execute_ShouldCallAction()
        {
            // Arrange
            bool actionCalled = false;
            var command = new ActionCommand(() => actionCalled = true);

            // Act
            command.Execute();

            // Assert
            Assert.IsTrue(actionCalled, "Action should be called when Execute is invoked");
        }

        [Test]
        public void ActionCommand_CanExecute_DefaultTrue()
        {
            // Arrange
            var command = new ActionCommand(() => { });

            // Act & Assert
            Assert.IsTrue(command.CanExecute(), "CanExecute should return true by default");
        }

        [Test]
        public void ActionCommand_CanExecute_WithPredicateFalse()
        {
            // Arrange
            var command = new ActionCommand(() => { }, () => false);

            try
            {
                // Act & Assert
                Assert.IsFalse(command.CanExecute(), "CanExecute should return false when predicate returns false");
            }
            finally
            {
                command.Dispose();
            }
        }

        [Test]
        public void ActionCommand_CanExecute_WithPredicateTrue()
        {
            // Arrange
            var command = new ActionCommand(() => { }, () => true);

            try
            {
                // Act & Assert
                Assert.IsTrue(command.CanExecute(), "CanExecute should return true when predicate returns true");
            }
            finally
            {
                command.Dispose();
            }
        }

        [Test]
        public void ActionCommand_Execute_WhenCannotExecute_ShouldNotCallAction()
        {
            // Arrange
            bool actionCalled = false;
            var command = new ActionCommand(() => actionCalled = true, () => false);

            // Act
            command.Execute();

            // Assert
            Assert.IsFalse(actionCalled, "Action should not be called when CanExecute returns false");
        }

        [Test]
        public void ActionCommand_Constructor_NullAction_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ActionCommand(null),
                "Should throw ArgumentNullException when action is null");
        }

        [Test]
        public void ActionCommand_Dispose_ShouldDispose()
        {
            // Arrange
            var command = new ActionCommand(() => { });

            // Act
            command.Dispose();

            // Assert - No exception should be thrown
            Assert.DoesNotThrow(() => command.Dispose(), "Dispose should not throw exception");
        }

        [Test]
        public void RelayCommand_Execute_ShouldCallAction()
        {
            // Arrange
            bool actionCalled = false;
            object receivedParameter = null;
            var command = new RelayCommand<string>(param => 
            {
                actionCalled = true;
                receivedParameter = param;
            });

            // Act
            command.Execute("test");

            // Assert
            Assert.IsTrue(actionCalled, "Action should be called when Execute is invoked");
            Assert.AreEqual("test", receivedParameter, "Parameter should be passed to action");
        }

        [Test]
        public void RelayCommand_CanExecute_DefaultTrue()
        {
            // Arrange
            var command = new RelayCommand<string>(param => { });

            // Act & Assert
            Assert.IsTrue(command.CanExecute(), "CanExecute should return true by default");
        }

        [Test]
        public void RelayCommand_CanExecute_WithPredicateFalse()
        {
            // Arrange
            var command = new RelayCommand<string>(param => { }, () => false);

            try
            {
                // Act & Assert
                Assert.IsFalse(command.CanExecute(), "CanExecute should return false when predicate returns false");
            }
            finally
            {
                command.Dispose();
            }
        }

        [Test]
        public void RelayCommand_CanExecute_WithPredicateTrue()
        {
            // Arrange
            var command = new RelayCommand<string>(param => { }, () => true);

            try
            {
                // Act & Assert
                Assert.IsTrue(command.CanExecute(), "CanExecute should return true when predicate returns true");
            }
            finally
            {
                command.Dispose();
            }
        }

        [Test]
        public void RelayCommand_Execute_WhenCannotExecute_ShouldNotCallAction()
        {
            // Arrange
            bool actionCalled = false;
            var command = new RelayCommand<string>(
                param => actionCalled = true, 
                () => false);

            // Act
            command.Execute("test");

            // Assert
            Assert.IsFalse(actionCalled, "Action should not be called when CanExecute returns false");
        }

        [Test]
        public async Task ActionAsyncCommand_ExecuteAsync_ShouldCallAsyncAction()
        {
            // Arrange
            bool actionCalled = false;
            var command = new ActionAsyncCommand(async () =>
            {
                await Task.Delay(10);
                actionCalled = true;
            });

            // Act
            await command.ExecuteAsync();

            // Assert
            Assert.IsTrue(actionCalled, "Async action should be called when ExecuteAsync is invoked");
        }

        [Test]
        public void ActionAsyncCommand_CanExecute_DefaultTrue()
        {
            // Arrange
            var command = new ActionAsyncCommand(async () => await Task.Delay(10));

            // Act & Assert
            Assert.IsTrue(command.CanExecute(), "CanExecute should return true by default");
        }

        [Test]
        public void ActionAsyncCommand_CanExecute_WithPredicateFalse()
        {
            // Arrange
            var command = new ActionAsyncCommand(async () => await Task.Delay(10), () => false);

            try
            {
                // Act & Assert
                Assert.IsFalse(command.CanExecute(), "CanExecute should return false when predicate returns false");
            }
            finally
            {
                command.Dispose();
            }
        }

        [Test]
        public void ActionAsyncCommand_CanExecute_WithPredicateTrue()
        {
            // Arrange
            var command = new ActionAsyncCommand(async () => await Task.Delay(10), () => true);

            try
            {
                // Act & Assert
                Assert.IsTrue(command.CanExecute(), "CanExecute should return true when predicate returns true");
            }
            finally
            {
                command.Dispose();
            }
        }

        [Test]
        public async Task AsyncRelayCommand_ExecuteAsync_ShouldCallAsyncAction()
        {
            // Arrange
            bool actionCalled = false;
            object receivedParameter = null;
            var command = new AsyncRelayCommand<string>(async param =>
            {
                await Task.Delay(10);
                actionCalled = true;
                receivedParameter = param;
            });

            // Act
            await command.ExecuteAsync("test");

            // Assert
            Assert.IsTrue(actionCalled, "Async action should be called when ExecuteAsync is invoked");
            Assert.AreEqual("test", receivedParameter, "Parameter should be passed to async action");
        }

        [Test]
        public void AsyncRelayCommand_CanExecute_WithPredicateFalse()
        {
            // Arrange
            var command = new AsyncRelayCommand<string>(
                async param => await Task.Delay(10), () => false);

            try
            {
                // Act & Assert
                Assert.IsFalse(command.CanExecute(), "CanExecute should return false when predicate returns false");
            }
            finally
            {
                command.Dispose();
            }
        }

        [Test]
        public void AsyncRelayCommand_CanExecute_WithPredicateTrue()
        {
            // Arrange
            var command = new AsyncRelayCommand<string>(
                async param => await Task.Delay(10), () => true);

            try
            {
                // Act & Assert
                Assert.IsTrue(command.CanExecute(), "CanExecute should return true when predicate returns true");
            }
            finally
            {
                command.Dispose();
            }
        }
    }
}