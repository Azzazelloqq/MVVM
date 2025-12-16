#if !PROJECT_SUPPORT_R3
using System;
using System.Collections.Generic;

namespace Azzazelloqq.MVVM.ReactiveLibrary
{
/// <summary>
/// Represents a reactive notifier that allows for subscription to notifications and triggers those notifications
/// when the <see cref="Notify"/> method is called. Implements <see cref="IReactiveNotifier"/>.
/// </summary>
public class ReactiveNotifier : IReactiveNotifier
{
 public bool IsDisposed { get; private set; }

        private readonly object _lock = new();
        private readonly int _capacity;
        private List<Action> _callbacks;             // основное хранилище
        private Action[]     _cache = Array.Empty<Action>();  // буфер для быстрого Invoke

        private int _cacheVersion;   // увеличивается при каждой модификации списка
        private int _cachedVersion = -1; // версия, для которой собран _cache
        private int _cacheCount;     // сколько значимых элементов лежит в _cache

        public ReactiveNotifier(int listenersCapacity = 30)
        {
            _capacity = listenersCapacity;
        }

        /*------------------------------------------------------------------------*/
        /* IReadOnlyNotifier                                                      */
        /*------------------------------------------------------------------------*/

        public void Subscribe(Action onNotify)
        {
            if (onNotify == null) throw new ArgumentNullException(nameof(onNotify));
            ThrowIfDisposed();

            lock (_lock)
            {
                _callbacks ??= new List<Action>(_capacity);
                _callbacks.Add(onNotify);
                _cacheVersion++;
            }
        }

        public void Unsubscribe(Action onNotify)
        {
            if (onNotify == null) return;

            lock (_lock)
            {
                if (_callbacks != null && _callbacks.Remove(onNotify))
                    _cacheVersion++;
            }
        }

        public void Notify()
        {
            if (IsDisposed) return;
            if (_callbacks == null) return;      // ещё никто не подписывался

            Action[] local;
            int      count;

            lock (_lock)
            {
                if (_cacheVersion != _cachedVersion)
                {
                    count = _callbacks.Count;

                    // подгоняем размер кэша, чтобы не держать лишнюю память
                    if (_cache.Length > count * 2 || _cache.Length < count)
                        _cache = new Action[count];

                    _callbacks.CopyTo(_cache, 0);
                    _cacheCount    = count;
                    _cachedVersion = _cacheVersion;
                }
                else
                {
                    count = _cacheCount;
                }

                local = _cache; // после выхода из lock читаем из кеша без блокировок
            }

            for (var i = 0; i < count; i++)
            {
                local[i]?.Invoke();
            }
        }

        public void Dispose()
        {
            if (IsDisposed) return;

            lock (_lock)
            {
                _callbacks?.Clear();
                _cacheCount    = 0;
                _cacheVersion++;
                _cachedVersion = -1;
            }

            IsDisposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(ReactiveNotifier));
        }
}
}
#endif
