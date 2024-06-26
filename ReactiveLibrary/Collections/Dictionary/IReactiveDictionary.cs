﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MVVM.MVVM.ReactiveLibrary.Collections.Base;

namespace MVVM.MVVM.ReactiveLibrary.Collections.Dictionary
{
public interface IReactiveDictionary<TKey, TValue> :
    IReactiveCollection<KeyValuePair<TKey, TValue>>,
    IDictionary<TKey, TValue>
{
    public bool TryAdd(TKey key, TValue value);
    public void TrimExcess();
    public void TrimExcess(int capacity);
    public bool ContainsValue(TValue value);
    public int EnsureCapacity(int capacity);
    public void GetObjectDataGetObjectData(SerializationInfo info, StreamingContext context);

    public void OnDeserialization(object sender);
    
    public void SubscribeOnValueChangedByKey(Action<KeyValuePair<TKey, TValue>> action);
    public void UnsubscribeOnValueChangedByKey(Action<KeyValuePair<TKey, TValue>> action);
}
}