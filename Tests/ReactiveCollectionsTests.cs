#if !PROJECT_SUPPORT_R3
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Azzazelloqq.MVVM.ReactiveLibrary;
using Azzazelloqq.MVVM.ReactiveLibrary.Collections;

namespace Azzazelloqq.MVVM.Tests
{
    /// <summary>
    /// Tests for reactive collections to verify IReactive interface contract implementation
    /// </summary>
    [TestFixture]
    public class ReactiveCollectionsTests
    {
        /// <summary>
        /// Tests for IReactive interface contract across all reactive collections
        /// </summary>
        [TestFixture]
        public class IReactiveContractTests
        {
            /// <summary>
            /// Verifies that all reactive collections implement IReactive interface
            /// </summary>
            [Test]
            public void AllCollections_ShouldImplementIReactive()
            {
                // Arrange & Act
                IReactive list = new ReactiveList<int>();
                IReactive array = new ReactiveArray<string>(5);
                IReactive dictionary = new ReactiveDictionary<string, object>(10);
                IReactive queue = new ReactiveQueue<double>();
                IReactive stack = new ReactiveStack<bool>();

                // Assert
                Assert.IsNotNull(list, "ReactiveList should be assignable to IReactive");
                Assert.IsNotNull(array, "ReactiveArray should be assignable to IReactive");
                Assert.IsNotNull(dictionary, "ReactiveDictionary should be assignable to IReactive");
                Assert.IsNotNull(queue, "ReactiveQueue should be assignable to IReactive");
                Assert.IsNotNull(stack, "ReactiveStack should be assignable to IReactive");

                // Cleanup
                list.Dispose();
                array.Dispose();
                dictionary.Dispose();
                queue.Dispose();
                stack.Dispose();
            }

            /// <summary>
            /// Verifies IsDisposed property for all collections
            /// </summary>
            [Test]
            public void AllCollections_IsDisposed_ShouldBeFalseInitially()
            {
                // Arrange
                var collections = new List<IReactive>
                {
                    new ReactiveList<int>(),
                    new ReactiveArray<string>(5),
                    new ReactiveDictionary<string, object>(10),
                    new ReactiveQueue<double>(),
                    new ReactiveStack<bool>()
                };

                try
                {
                    // Act & Assert
                    foreach (var collection in collections)
                    {
                        Assert.IsFalse(collection.IsDisposed, 
                            $"{collection.GetType().Name} should have IsDisposed=false initially");
                    }
                }
                finally
                {
                    // Cleanup
                    foreach (var collection in collections)
                    {
                        collection.Dispose();
                    }
                }
            }

            /// <summary>
            /// Verifies Dispose method for all collections
            /// </summary>
            [Test]
            public void AllCollections_Dispose_ShouldSetIsDisposedTrue()
            {
                // Arrange
                var collections = new List<IReactive>
                {
                    new ReactiveList<int>(),
                    new ReactiveArray<string>(5),
                    new ReactiveDictionary<string, object>(10),
                    new ReactiveQueue<double>(),
                    new ReactiveStack<bool>()
                };

                // Act
                foreach (var collection in collections)
                {
                    collection.Dispose();
                }

                // Assert
                foreach (var collection in collections)
                {
                    Assert.IsTrue(collection.IsDisposed, 
                        $"{collection.GetType().Name} should have IsDisposed=true after Dispose()");
                }
            }

            /// <summary>
            /// Verifies that Dispose can be called multiple times without throwing
            /// </summary>
            [Test]
            public void AllCollections_DisposeTwice_ShouldNotThrow()
            {
                // Arrange
                var collections = new List<IReactive>
                {
                    new ReactiveList<int>(),
                    new ReactiveArray<string>(5),
                    new ReactiveDictionary<string, object>(10),
                    new ReactiveQueue<double>(),
                    new ReactiveStack<bool>()
                };

                // Act & Assert
                foreach (var collection in collections)
                {
                    Assert.DoesNotThrow(() =>
                    {
                        collection.Dispose();
                        collection.Dispose();
                    }, $"{collection.GetType().Name} should not throw when Dispose() is called twice");
                }
            }

            /// <summary>
            /// Tests polymorphic behavior with IReactive interface
            /// </summary>
            [Test]
            public void PolymorphicUsage_WithIReactive_ShouldWork()
            {
                // Arrange
                var reactiveObjects = new List<IReactive>
                {
                    new ReactiveList<int> { 1, 2, 3 },
                    new ReactiveArray<string>(new[] { "a", "b", "c" }),
                    new ReactiveDictionary<string, int>(10) { ["key1"] = 1 },
                    new ReactiveQueue<double>(new[] { 1.0, 2.0 }),
                    new ReactiveStack<bool>(new[] { true, false })
                };

                try
                {
                    // Act - Process all as IReactive
                    foreach (var reactive in reactiveObjects)
                    {
                        // Should be able to check IsDisposed
                        Assert.IsFalse(reactive.IsDisposed);
                        
                        // Should be able to work with them polymorphically
                        ProcessReactiveObject(reactive);
                    }

                    // Assert - all should still be functional
                    Assert.AreEqual(4, ((ReactiveList<int>)reactiveObjects[0]).Count);
                    Assert.AreEqual("modified", ((ReactiveArray<string>)reactiveObjects[1])[0]);
                    Assert.IsTrue(((ReactiveDictionary<string, int>)reactiveObjects[2]).ContainsKey("key2"));
                    Assert.AreEqual(3, ((ReactiveQueue<double>)reactiveObjects[3]).Count);
                    Assert.AreEqual(3, ((ReactiveStack<bool>)reactiveObjects[4]).Count);
                }
                finally
                {
                    // Cleanup
                    foreach (var reactive in reactiveObjects)
                    {
                        reactive.Dispose();
                    }
                }
            }

            private void ProcessReactiveObject(IReactive reactive)
            {
                // Example of polymorphic processing
                if (reactive is ReactiveList<int> list)
                {
                    list.Add(4);
                }
                else if (reactive is ReactiveArray<string> array)
                {
                    array[0] = "modified";
                }
                else if (reactive is ReactiveDictionary<string, int> dict)
                {
                    dict["key2"] = 2;
                }
                else if (reactive is ReactiveQueue<double> queue)
                {
                    queue.Enqueue(3.0);
                }
                else if (reactive is ReactiveStack<bool> stack)
                {
                    stack.Push(true);
                }
            }
        }

        /// <summary>
        /// Tests for ReactiveList<T> specific functionality
        /// </summary>
        [TestFixture]
        public class ReactiveListTests
        {
            private ReactiveList<int> _list;

            [SetUp]
            public void SetUp()
            {
                _list = new ReactiveList<int>();
            }

            [TearDown]
            public void TearDown()
            {
                _list?.Dispose();
            }

            [Test]
            public void ReactiveList_ImplementsIReactive()
            {
                // Assert
                Assert.IsTrue(_list is IReactive, "ReactiveList should implement IReactive");
                Assert.IsTrue(_list is IReadOnlyReactiveCollection<int>, 
                    "ReactiveList should implement IReadOnlyReactiveCollection");
                Assert.IsTrue(_list is IReactiveCollection<int>, 
                    "ReactiveList should implement IReactiveCollection");
            }

            [Test]
            public void Add_AfterDispose_ShouldThrow()
            {
                // Arrange
                _list.Dispose();

                // Act & Assert
                Assert.Throws<ObjectDisposedException>(() => _list.Add(42),
                    "Should throw ObjectDisposedException when adding to disposed list");
            }

            [Test]
            public void SubscribeOnItemAdded_ShouldReceiveNotifications()
            {
                // Arrange
                var addedItems = new List<int>();
                _list.SubscribeOnItemAdded(item => addedItems.Add(item));

                // Act
                _list.Add(1);
                _list.Add(2);
                _list.Add(3);

                // Assert
                CollectionAssert.AreEqual(new[] { 1, 2, 3 }, addedItems);
            }

            [Test]
            public void AsReadOnly_ShouldReturnIReadOnlyReactiveCollection()
            {
                // Act
                var readOnly = _list.AsReadOnly();

                // Assert
                Assert.IsNotNull(readOnly, "AsReadOnly should return a non-null collection");
                Assert.IsTrue(readOnly is IReadOnlyReactiveCollection<int>, 
                    "Should return IReadOnlyReactiveCollection");
                Assert.IsTrue(readOnly is IReactive, 
                    "ReadOnly collection should also implement IReactive");
            }
        }

        /// <summary>
        /// Tests for ReactiveArray<T> specific functionality
        /// </summary>
        [TestFixture]
        public class ReactiveArrayTests
        {
            private ReactiveArray<string> _array;

            [SetUp]
            public void SetUp()
            {
                _array = new ReactiveArray<string>(5);
            }

            [TearDown]
            public void TearDown()
            {
                _array?.Dispose();
            }

            [Test]
            public void ReactiveArray_ImplementsIReactive()
            {
                // Assert
                Assert.IsTrue(_array is IReactive, "ReactiveArray should implement IReactive");
                Assert.IsTrue(_array is IReadOnlyReactiveCollection<string>, 
                    "ReactiveArray should implement IReadOnlyReactiveCollection");
                Assert.IsTrue(_array is IReactiveCollection<string>, 
                    "ReactiveArray should implement IReactiveCollection");
            }

            [Test]
            public void Indexer_AfterDispose_ShouldThrow()
            {
                // Arrange
                _array[0] = "test";
                _array.Dispose();

                // Act & Assert
                Assert.Throws<ObjectDisposedException>(() => _array[0] = "new value",
                    "Should throw ObjectDisposedException when setting value in disposed array");
            }

            [Test]
            public void SubscribeOnItemChangedByIndex_ShouldReceiveNotifications()
            {
                // Arrange
                var changedItems = new List<(string, int)>();
                _array.SubscribeOnItemChangedByIndex(item => changedItems.Add(item));

                // Act
                _array[0] = "first";
                _array[2] = "third";
                _array[4] = "fifth";

                // Assert
                Assert.AreEqual(3, changedItems.Count);
                Assert.AreEqual(("first", 0), changedItems[0]);
                Assert.AreEqual(("third", 2), changedItems[1]);
                Assert.AreEqual(("fifth", 4), changedItems[2]);
            }

            [Test]
            public void Length_ShouldReturnCorrectValue()
            {
                // Assert
                Assert.AreEqual(5, _array.Length, "Array length should be 5");
                Assert.AreEqual(5, _array.Count, "Array count should equal length");
            }
        }

        /// <summary>
        /// Tests for ReactiveDictionary<TKey, TValue> specific functionality
        /// </summary>
        [TestFixture]
        public class ReactiveDictionaryTests
        {
            private ReactiveDictionary<string, int> _dictionary;

            [SetUp]
            public void SetUp()
            {
                _dictionary = new ReactiveDictionary<string, int>(10);
            }

            [TearDown]
            public void TearDown()
            {
                _dictionary?.Dispose();
            }

            [Test]
            public void ReactiveDictionary_ImplementsIReactive()
            {
                // Assert
                Assert.IsTrue(_dictionary is IReactive, "ReactiveDictionary should implement IReactive");
                Assert.IsTrue(_dictionary is IReadOnlyReactiveCollection<KeyValuePair<string, int>>, 
                    "ReactiveDictionary should implement IReadOnlyReactiveCollection");
                Assert.IsTrue(_dictionary is IReactiveCollection<KeyValuePair<string, int>>, 
                    "ReactiveDictionary should implement IReactiveCollection");
            }

            [Test]
            public void Add_AfterDispose_ShouldThrow()
            {
                // Arrange
                _dictionary.Dispose();

                // Act & Assert
                Assert.Throws<ObjectDisposedException>(() => _dictionary.Add("key", 42),
                    "Should throw ObjectDisposedException when adding to disposed dictionary");
            }

            [Test]
            public void SubscribeOnValueChangedByKey_ShouldReceiveNotifications()
            {
                // Arrange
                var changedPairs = new List<KeyValuePair<string, int>>();
                _dictionary.SubscribeOnValueChangedByKey(pair => changedPairs.Add(pair));

                // Act
                _dictionary["key1"] = 1;
                _dictionary["key2"] = 2;
                _dictionary["key1"] = 10; // Update existing

                // Assert
                Assert.AreEqual(3, changedPairs.Count);
                Assert.AreEqual(new KeyValuePair<string, int>("key1", 1), changedPairs[0]);
                Assert.AreEqual(new KeyValuePair<string, int>("key2", 2), changedPairs[1]);
                Assert.AreEqual(new KeyValuePair<string, int>("key1", 10), changedPairs[2]);
            }

            [Test]
            public void TryAdd_ShouldReturnCorrectResult()
            {
                // Act & Assert
                Assert.IsTrue(_dictionary.TryAdd("key1", 1), "Should successfully add new key");
                Assert.IsFalse(_dictionary.TryAdd("key1", 2), "Should fail to add duplicate key");
                Assert.AreEqual(1, _dictionary["key1"], "Value should remain unchanged");
            }
        }

        /// <summary>
        /// Tests for ReactiveQueue<T> specific functionality
        /// </summary>
        [TestFixture]
        public class ReactiveQueueTests
        {
            private ReactiveQueue<double> _queue;

            [SetUp]
            public void SetUp()
            {
                _queue = new ReactiveQueue<double>();
            }

            [TearDown]
            public void TearDown()
            {
                _queue?.Dispose();
            }

            [Test]
            public void ReactiveQueue_ImplementsIReactive()
            {
                // Assert
                Assert.IsTrue(_queue is IReactive, "ReactiveQueue should implement IReactive");
                Assert.IsTrue(_queue is IReadOnlyReactiveCollection<double>, 
                    "ReactiveQueue should implement IReadOnlyReactiveCollection");
                Assert.IsTrue(_queue is IReactiveCollection<double>, 
                    "ReactiveQueue should implement IReactiveCollection");
            }

            [Test]
            public void Enqueue_AfterDispose_ShouldThrow()
            {
                // Arrange
                _queue.Dispose();

                // Act & Assert
                Assert.Throws<ObjectDisposedException>(() => _queue.Enqueue(3.14),
                    "Should throw ObjectDisposedException when enqueuing to disposed queue");
            }

            [Test]
            public void EnqueueDequeue_ShouldWorkInFIFOOrder()
            {
                // Arrange
                _queue.Enqueue(1.0);
                _queue.Enqueue(2.0);
                _queue.Enqueue(3.0);

                // Act & Assert
                Assert.AreEqual(1.0, _queue.Dequeue());
                Assert.AreEqual(2.0, _queue.Dequeue());
                Assert.AreEqual(3.0, _queue.Dequeue());
            }

            [Test]
            public void TryDequeue_WhenEmpty_ShouldReturnFalse()
            {
                // Act & Assert
                Assert.IsFalse(_queue.TryDequeue(out var result));
                Assert.AreEqual(default(double), result);
            }

            [Test]
            public void Clone_ShouldCreateIndependentCopy()
            {
                // Arrange
                _queue.Enqueue(1.0);
                _queue.Enqueue(2.0);

                // Act
                var clone = (ReactiveQueue<double>)_queue.Clone();

                // Assert
                Assert.AreEqual(2, clone.Count);
                _queue.Enqueue(3.0);
                Assert.AreEqual(3, _queue.Count);
                Assert.AreEqual(2, clone.Count, "Clone should be independent");

                // Cleanup
                clone.Dispose();
            }
        }

        /// <summary>
        /// Tests for ReactiveStack<T> specific functionality
        /// </summary>
        [TestFixture]
        public class ReactiveStackTests
        {
            private ReactiveStack<bool> _stack;

            [SetUp]
            public void SetUp()
            {
                _stack = new ReactiveStack<bool>();
            }

            [TearDown]
            public void TearDown()
            {
                _stack?.Dispose();
            }

            [Test]
            public void ReactiveStack_ImplementsIReactive()
            {
                // Assert
                Assert.IsTrue(_stack is IReactive, "ReactiveStack should implement IReactive");
                Assert.IsTrue(_stack is IReadOnlyReactiveCollection<bool>, 
                    "ReactiveStack should implement IReadOnlyReactiveCollection");
                Assert.IsTrue(_stack is IReactiveCollection<bool>, 
                    "ReactiveStack should implement IReactiveCollection");
            }

            [Test]
            public void Push_AfterDispose_ShouldThrow()
            {
                // Arrange
                _stack.Dispose();

                // Act & Assert
                Assert.Throws<ObjectDisposedException>(() => _stack.Push(true),
                    "Should throw ObjectDisposedException when pushing to disposed stack");
            }

            [Test]
            public void PushPop_ShouldWorkInLIFOOrder()
            {
                // Arrange
                _stack.Push(true);
                _stack.Push(false);
                _stack.Push(true);

                // Act & Assert
                Assert.AreEqual(true, _stack.Pop(), "Should pop last pushed item first");
                Assert.AreEqual(false, _stack.Pop());
                Assert.AreEqual(true, _stack.Pop());
            }

            [Test]
            public void TryPop_WhenEmpty_ShouldReturnFalse()
            {
                // Act & Assert
                Assert.IsFalse(_stack.TryPop(out var result));
                Assert.AreEqual(default(bool), result);
            }

            [Test]
            public void ToArray_ShouldReturnCorrectOrder()
            {
                // Arrange
                _stack.Push(true);
                _stack.Push(false);
                _stack.Push(true);

                // Act
                var array = _stack.ToArray();

                // Assert
                Assert.AreEqual(3, array.Length);
                // Stack ToArray returns in pop order (LIFO)
                Assert.AreEqual(true, array[0]);
                Assert.AreEqual(false, array[1]);
                Assert.AreEqual(true, array[2]);
            }
        }

        /// <summary>
        /// Integration tests for IReactive collections
        /// </summary>
        [TestFixture]
        public class IReactiveIntegrationTests
        {
            [Test]
            public void MixedCollections_CanBeProcessedPolymorphically()
            {
                // Arrange
                var collections = CreateMixedReactiveCollections();
                
                try
                {
                    // Act - Process all collections through IReactive interface
                    foreach (var reactive in collections)
                    {
                        ProcessAndVerifyReactive(reactive);
                    }

                    // Assert - All should pass processing
                    Assert.Pass("All reactive collections successfully processed as IReactive");
                }
                finally
                {
                    // Cleanup
                    foreach (var reactive in collections)
                    {
                        reactive.Dispose();
                    }
                }
            }

            private List<IReactive> CreateMixedReactiveCollections()
            {
                return new List<IReactive>
                {
                    new ReactiveList<string> { "a", "b", "c" },
                    new ReactiveArray<int>(new[] { 1, 2, 3, 4, 5 }),
                    new ReactiveDictionary<int, string>(5) 
                    { 
                        [1] = "one", 
                        [2] = "two" 
                    },
                    new ReactiveQueue<object>(new object[] { new object(), "test", 123 }),
                    new ReactiveStack<decimal>(new[] { 1.1m, 2.2m, 3.3m })
                };
            }

            private void ProcessAndVerifyReactive(IReactive reactive)
            {
                // Verify initial state
                Assert.IsFalse(reactive.IsDisposed, "Should not be disposed initially");
                
                // Test that we can subscribe to collection changes
                if (reactive is IReadOnlyReactiveCollection<object> collection)
                {
                    bool notified = false;
                    collection.SubscribeOnCollectionChanged(items => notified = true, false);
                    
                    // Trigger a change based on specific collection type
                    if (reactive is ReactiveList<object> list)
                    {
                        list.Add(new object());
                        Assert.IsTrue(notified, "Should receive notification on list change");
                    }
                    else if (reactive is ReactiveQueue<object> queue)
                    {
                        queue.Enqueue(new object());
                        Assert.IsTrue(notified, "Should receive notification on queue change");
                    }
                    else if (reactive is ReactiveStack<object> stack)
                    {
                        stack.Push(new object());
                        Assert.IsTrue(notified, "Should receive notification on stack change");
                    }
                    else if (reactive is ReactiveDictionary<object, object> dict)
                    {
                        dict.Add(new object(), new object());
                        Assert.IsTrue(notified, "Should receive notification on dictionary change");
                    }
                    else if (reactive is ICollection<object> mutableCollection)
                    {
                        try
                        {
                            mutableCollection.Add(new object());
                            Assert.IsTrue(notified, "Should receive notification on collection change");
                        }
                        catch (NotImplementedException)
                        {
                            // Some collections don't support Add, which is fine
                        }
                    }
                }
                
                // Verify disposal doesn't throw
                Assert.DoesNotThrow(() =>
                {
                    reactive.Dispose();
                    reactive.Dispose(); // Double dispose should not throw
                });
                
                // Verify disposed state
                Assert.IsTrue(reactive.IsDisposed, "Should be disposed after calling Dispose()");
            }

            [Test]
            public void ReactiveCollections_CanBeStoredInCommonContainer()
            {
                // Arrange
                var reactiveContainer = new Dictionary<string, IReactive>
                {
                    ["list"] = new ReactiveList<int> { 1, 2, 3 },
                    ["array"] = new ReactiveArray<string>(3),
                    ["dictionary"] = new ReactiveDictionary<string, int>(10),
                    ["queue"] = new ReactiveQueue<double>(),
                    ["stack"] = new ReactiveStack<bool>()
                };

                try
                {
                    // Act - Verify all can be accessed as IReactive
                    foreach (var kvp in reactiveContainer)
                    {
                        Assert.IsNotNull(kvp.Value, $"{kvp.Key} should not be null");
                        Assert.IsFalse(kvp.Value.IsDisposed, $"{kvp.Key} should not be disposed");
                    }

                    // Dispose all through IReactive interface
                    foreach (var kvp in reactiveContainer)
                    {
                        kvp.Value.Dispose();
                    }

                    // Assert - All should be disposed
                    foreach (var kvp in reactiveContainer)
                    {
                        Assert.IsTrue(kvp.Value.IsDisposed, $"{kvp.Key} should be disposed");
                    }
                }
                finally
                {
                    // Ensure cleanup
                    foreach (var kvp in reactiveContainer)
                    {
                        kvp.Value?.Dispose();
                    }
                }
            }
        }
    }
}
#endif
