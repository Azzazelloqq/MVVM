using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azzazelloqq.MVVM.Core;
using Azzazelloqq.MVVM.ReactiveLibrary;
using Azzazelloqq.MVVM.ReactiveLibrary.Collections;
using Disposable;

namespace Azzazelloqq.MVVM.Example
{
/// <summary>
/// Example demonstrating the usage of AddTo extension method for managing disposables.
/// </summary>
public class AddToUsageExample : ViewModelBase<IModel>
{
	private readonly ReactiveProperty<int> _counter;
	private readonly ReactiveProperty<string> _status;
	private readonly ReactiveNotifier _updateNotifier;

	public AddToUsageExample(IModel model) : base(model)
	{
		_counter = new ReactiveProperty<int>(0);
		_status = new ReactiveProperty<string>("Ready");
		_updateNotifier = new ReactiveNotifier();
		
		// Add reactive properties to composite disposable for automatic cleanup
		_counter.AddTo(compositeDisposable);
		_status.AddTo(compositeDisposable);
		_updateNotifier.AddTo(compositeDisposable);
	}

	protected override void OnInitialize()
	{
		// Example 1: Subscribe to property changes and add to composite
		_counter
			.Subscribe(OnCounterChanged, withNotify: false)
			.AddTo(compositeDisposable);
		
		// Example 2: Subscribe once and add to composite
		_status
			.SubscribeOnce(OnFirstStatusChange)
			.AddTo(compositeDisposable);
		
		// Example 3: Chain multiple subscriptions
		_updateNotifier
			.Subscribe(OnUpdate)
			.AddTo(compositeDisposable);
			
		// Example 4: Multiple subscriptions to same source
		_counter
			.Subscribe(value => Console.WriteLine($"Counter value: {value}"), withNotify: false)
			.AddTo(compositeDisposable);
			
		_counter
			.Subscribe(value => _status.SetValue($"Count: {value}"), withNotify: false)
			.AddTo(compositeDisposable);
	}

	protected override ValueTask OnInitializeAsync(CancellationToken token)
	{
		return default;
	}

	protected override void OnDispose()
	{
		// All subscriptions added with AddTo will be automatically disposed
		// when compositeDisposable is disposed by the base class
		
		// No need to manually unsubscribe or dispose individual subscriptions
		Console.WriteLine("All subscriptions automatically cleaned up");
	}

	protected override ValueTask OnDisposeAsync(CancellationToken token)
	{
		return default;
	}

	private void OnCounterChanged(int value)
	{
		Console.WriteLine($"Counter changed to: {value}");
	}

	private void OnFirstStatusChange(string status)
	{
		Console.WriteLine($"First status change: {status}");
	}

	private void OnUpdate()
	{
		_counter.SetValue(_counter.Value + 1);
	}
	
	/// <summary>
	/// Example of using AddTo with reactive collections
	/// </summary>
	public class CollectionExample : ViewModelBase<IModel>
	{
		private readonly ReactiveList<string> _items;
		
		public CollectionExample(IModel model) : base(model)
		{
			_items = new ReactiveList<string>();
			
			// Add the collection itself to composite disposable
			_items.AddTo(compositeDisposable);
		}

		protected override void OnInitialize()
		{
			// Subscribe to collection changes using void methods (not returning Subscription)
			// These need manual unsubscription in OnDispose
			_items.SubscribeOnItemAdded(OnItemAdded);
			_items.SubscribeOnCollectionChanged(OnCollectionChanged, notifyOnSubscribe: false);
		}

		protected override ValueTask OnInitializeAsync(CancellationToken token)
		{
			return default;
		}

		protected override void OnDispose()
		{
			// Manual unsubscription for void subscriptions
			_items.UnsubscribeOnItemAdded(OnItemAdded);
			_items.UnsubscribeOnCollectionChanged(OnCollectionChanged);
			
			// The _items collection itself will be disposed through compositeDisposable
		}

		protected override ValueTask OnDisposeAsync(CancellationToken token)
		{
			return default;
		}

		private void OnItemAdded(string item)
		{
			Console.WriteLine($"Item added: {item}");
		}

		private void OnCollectionChanged(IEnumerable<string> collection)
		{
			Console.WriteLine($"Collection changed, now has {collection.Count()} items");
		}
	}
}
}
