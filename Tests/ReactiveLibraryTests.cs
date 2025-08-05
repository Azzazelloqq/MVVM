using NUnit.Framework;
using System;
using Azzazelloqq.MVVM.ReactiveLibrary;

namespace Azzazelloqq.MVVM.Tests
{
    /// <summary>
    /// Tests for ReactiveLibrary components (ReactiveProperty, ReactiveNotifier)
    /// </summary>
    [TestFixture]
    public class ReactiveLibraryTests
    {
        /// <summary>
        /// Tests for ReactiveProperty
        /// </summary>
        [TestFixture]
        public class ReactivePropertyTests
        {
            private ReactiveProperty<int> _reactiveProperty;

            [SetUp]
            public void SetUp()
            {
                _reactiveProperty = new ReactiveProperty<int>(0);
            }

            [TearDown]
            public void TearDown()
            {
                _reactiveProperty?.Dispose();
            }

            [Test]
            public void Constructor_WithInitialValue_ShouldSetValue()
            {
                // Arrange & Act
                var property = new ReactiveProperty<string>("test");

                // Assert
                Assert.AreEqual("test", property.Value, "Initial value should be set");

                property.Dispose();
            }

            [Test]
            public void Constructor_WithoutInitialValue_ShouldHaveDefaultValue()
            {
                // Arrange & Act
                var property = new ReactiveProperty<int>();

                // Assert
                Assert.AreEqual(default(int), property.Value, "Default value should be set");

                property.Dispose();
            }

            [Test]
            public void SetValue_ShouldUpdateValue()
            {
                // Act
                _reactiveProperty.SetValue(42);

                // Assert
                Assert.AreEqual(42, _reactiveProperty.Value, "Value should be updated");
            }

            [Test]
            public void SetValue_SameValue_ShouldNotNotifySubscribers()
            {
                // Arrange
                int notificationCount = 0;
                _reactiveProperty.Subscribe(value => notificationCount++, withNotify: false);

                // Act
                _reactiveProperty.SetValue(0); // Same as initial value
                _reactiveProperty.SetValue(0); // Same as current value

                // Assert
                Assert.AreEqual(0, notificationCount, "Should not notify subscribers when value doesn't change");
            }

            [Test]
            public void SetValue_DifferentValue_ShouldNotifySubscribers()
            {
                // Arrange
                int notificationCount = 0;
                int lastValue = 0;
                _reactiveProperty.Subscribe(value => 
                {
                    notificationCount++;
                    lastValue = value;
                }, withNotify: false);

                // Act
                _reactiveProperty.SetValue(42);

                // Assert
                Assert.AreEqual(1, notificationCount, "Should notify subscribers when value changes");
                Assert.AreEqual(42, lastValue, "Subscriber should receive new value");
            }

            [Test]
            public void Subscribe_WithNotify_ShouldImmediatelyNotifyWithCurrentValue()
            {
                // Arrange
                _reactiveProperty.SetValue(100);
                int receivedValue = 0;

                // Act
                _reactiveProperty.Subscribe(value => receivedValue = value, withNotify: true);

                // Assert
                Assert.AreEqual(100, receivedValue, "Should immediately notify with current value");
            }

            [Test]
            public void Subscribe_WithoutNotify_ShouldNotImmediatelyNotify()
            {
                // Arrange
                _reactiveProperty.SetValue(100);
                int receivedValue = -1;

                // Act
                _reactiveProperty.Subscribe(value => receivedValue = value, withNotify: false);

                // Assert
                Assert.AreEqual(-1, receivedValue, "Should not immediately notify without withNotify");
            }

            [Test]
            public void SubscribeOnce_ShouldUnsubscribeAfterFirstNotification()
            {
                // Arrange
                int notificationCount = 0;
                _reactiveProperty.SubscribeOnce(value => notificationCount++);

                // Act
                _reactiveProperty.SetValue(1);
                _reactiveProperty.SetValue(2);

                // Assert
                Assert.AreEqual(1, notificationCount, "Should only notify once");
            }

            [Test]
            public void Unsubscribe_ShouldStopNotifications()
            {
                // Arrange
                int notificationCount = 0;
                Action<int> callback = value => notificationCount++;
                _reactiveProperty.Subscribe(callback, withNotify: false);

                // Act
                _reactiveProperty.SetValue(1);
                _reactiveProperty.Unsubscribe(callback);
                _reactiveProperty.SetValue(2);

                // Assert
                Assert.AreEqual(1, notificationCount, "Should stop notifications after unsubscribe");
            }

            [Test]
            public void SetValue_WhenDisposed_ShouldThrowObjectDisposedException()
            {
                // Arrange
                _reactiveProperty.Dispose();

                // Act & Assert
                Assert.Throws<ObjectDisposedException>(() => _reactiveProperty.SetValue(42),
                    "Should throw ObjectDisposedException when setting value on disposed property");
            }

            [Test]
            public void Subscribe_WhenDisposed_ShouldThrowObjectDisposedException()
            {
                // Arrange
                _reactiveProperty.Dispose();

                // Act & Assert
                Assert.Throws<ObjectDisposedException>(() => _reactiveProperty.Subscribe(value => { }),
                    "Should throw ObjectDisposedException when subscribing to disposed property");
            }

            [Test]
            public void Subscribe_NullCallback_ShouldThrowArgumentNullException()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => _reactiveProperty.Subscribe(null),
                    "Should throw ArgumentNullException when callback is null");
            }

            [Test]
            public void SubscribeOnce_NullCallback_ShouldThrowArgumentNullException()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => _reactiveProperty.SubscribeOnce(null),
                    "Should throw ArgumentNullException when callback is null");
            }

            [Test]
            public void Dispose_ShouldSetIsDisposedTrue()
            {
                // Act
                _reactiveProperty.Dispose();

                // Assert
                Assert.IsTrue(_reactiveProperty.IsDisposed, "IsDisposed should be true after dispose");
            }

            [Test]
            public void Dispose_CalledTwice_ShouldNotThrow()
            {
                // Act & Assert
                Assert.DoesNotThrow(() =>
                {
                    _reactiveProperty.Dispose();
                    _reactiveProperty.Dispose();
                }, "Should not throw when Dispose is called twice");
            }
        }

        /// <summary>
        /// Tests for ReactiveNotifier
        /// </summary>
        [TestFixture]
        public class ReactiveNotifierTests
        {
            private ReactiveNotifier _reactiveNotifier;

            [SetUp]
            public void SetUp()
            {
                _reactiveNotifier = new ReactiveNotifier();
            }

            [TearDown]
            public void TearDown()
            {
                _reactiveNotifier?.Dispose();
            }

            [Test]
            public void Subscribe_ShouldAddCallback()
            {
                // Arrange
                bool callbackCalled = false;

                // Act
                _reactiveNotifier.Subscribe(() => callbackCalled = true);
                _reactiveNotifier.Notify();

                // Assert
                Assert.IsTrue(callbackCalled, "Callback should be called after notification");
            }

            [Test]
            public void Subscribe_MultipleCallbacks_ShouldCallAll()
            {
                // Arrange
                int callbackCount = 0;

                // Act
                _reactiveNotifier.Subscribe(() => callbackCount++);
                _reactiveNotifier.Subscribe(() => callbackCount++);
                _reactiveNotifier.Subscribe(() => callbackCount++);
                _reactiveNotifier.Notify();

                // Assert
                Assert.AreEqual(3, callbackCount, "All callbacks should be called");
            }

            [Test]
            public void Unsubscribe_ShouldRemoveCallback()
            {
                // Arrange
                int callbackCount = 0;
                Action callback = () => callbackCount++;
                _reactiveNotifier.Subscribe(callback);

                // Act
                _reactiveNotifier.Notify();
                _reactiveNotifier.Unsubscribe(callback);
                _reactiveNotifier.Notify();

                // Assert
                Assert.AreEqual(1, callbackCount, "Callback should only be called once before unsubscribe");
            }

            [Test]
            public void Unsubscribe_NullCallback_ShouldNotThrow()
            {
                // Act & Assert
                Assert.DoesNotThrow(() => _reactiveNotifier.Unsubscribe(null),
                    "Should not throw when unsubscribing null callback");
            }

            [Test]
            public void Subscribe_NullCallback_ShouldThrowArgumentNullException()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => _reactiveNotifier.Subscribe(null),
                    "Should throw ArgumentNullException when subscribing null callback");
            }

            [Test]
            public void Notify_WhenDisposed_ShouldNotCallCallbacks()
            {
                // Arrange
                bool callbackCalled = false;
                _reactiveNotifier.Subscribe(() => callbackCalled = true);
                _reactiveNotifier.Dispose();

                // Act
                _reactiveNotifier.Notify();

                // Assert
                Assert.IsFalse(callbackCalled, "Callbacks should not be called after dispose");
            }

            [Test]
            public void Subscribe_WhenDisposed_ShouldThrowObjectDisposedException()
            {
                // Arrange
                _reactiveNotifier.Dispose();

                // Act & Assert
                Assert.Throws<ObjectDisposedException>(() => _reactiveNotifier.Subscribe(() => { }),
                    "Should throw ObjectDisposedException when subscribing to disposed notifier");
            }

            [Test]
            public void Dispose_ShouldSetIsDisposedTrue()
            {
                // Act
                _reactiveNotifier.Dispose();

                // Assert
                Assert.IsTrue(_reactiveNotifier.IsDisposed, "IsDisposed should be true after dispose");
            }

            [Test]
            public void Dispose_CalledTwice_ShouldNotThrow()
            {
                // Act & Assert
                Assert.DoesNotThrow(() =>
                {
                    _reactiveNotifier.Dispose();
                    _reactiveNotifier.Dispose();
                }, "Should not throw when Dispose is called twice");
            }

            [Test]
            public void Notify_WithoutSubscribers_ShouldNotThrow()
            {
                // Act & Assert
                Assert.DoesNotThrow(() => _reactiveNotifier.Notify(),
                    "Should not throw when notifying without subscribers");
            }

            [Test]
            public void Constructor_WithCapacity_ShouldWork()
            {
                // Act & Assert
                Assert.DoesNotThrow(() =>
                {
                    using var notifier = new ReactiveNotifier(100);
                }, "Should not throw when creating with specific capacity");
            }
        }
    }
}