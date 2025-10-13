using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NATSInternal.Desktop.ViewModels;

public class ObservableDictionary<TKey, TValue>
    :
        ObservableObject,
        IDictionary<TKey, TValue>,
        INotifyCollectionChanged
    where TKey : notnull
{
    #region Fields
    private readonly IDictionary<TKey, TValue> _inner = new Dictionary<TKey, TValue>();
    #endregion
    
    #region Properties
    public int Count => _inner.Count;
    public bool IsReadOnly => false;
    public ICollection<TKey> Keys => _inner.Keys;
    public ICollection<TValue> Values => _inner.Values;
    #endregion
    
    #region Events
    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    #endregion
    
    #region Indexers
    public TValue this[TKey key]
    {
        get
        {
            _inner.TryGetValue(key, out TValue? value);
            return value!;
        }
        set
        {
            if (!_inner.TryGetValue(key, out TValue? oldValue) || !Equals(oldValue, value))
            {
                _inner[key] = value;
                RaiseIndexerChanged(key);
            }
        }
    }
    #endregion
    
    #region Methods
    public void Add(KeyValuePair<TKey, TValue> item)
    {
        _inner.Add(item.Key, item.Value);
        RaiseIndexerChanged(item.Key);
        OnPropertyChanged(nameof(Count));
        RaiseNotifyCollectionAdded(item.Key, item.Value);
    }

    public void Add(TKey key, TValue value)
    {
        _inner.Add(key, value);
        RaiseIndexerChanged(key);
        OnPropertyChanged(nameof(Count));
        RaiseNotifyCollectionAdded(key, value);
    }

    public void Clear()
    {
        List<TKey> keys = _inner.Keys.ToList();
        _inner.Clear();
        foreach (TKey key in keys)
        {
            RaiseIndexerChanged(key);
        }

        OnPropertyChanged(nameof(Count));
        CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Reset));
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) => _inner.Contains(item);
    public bool ContainsKey(TKey key) => _inner.ContainsKey(key);
    public void CopyTo(KeyValuePair<TKey, TValue>[] items, int itemIndex)
    {
        _inner.CopyTo(items, itemIndex);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if (!(_inner).Remove(item))
        {
            return false;
        }
        
        RaiseIndexerChanged(item.Key);
        OnPropertyChanged(nameof(Count));
        RaiseNotifyCollectionRemoved(item.Key, item.Value);
        return true;
    }

    public bool Remove(TKey key)
    {
        if (!_inner.Remove(key, out TValue? removedValue))
        {
            return false;
        }
        
        RaiseIndexerChanged(key);
        OnPropertyChanged(nameof(Count));
        RaiseNotifyCollectionRemoved(key, removedValue);
        return true;
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        return _inner.TryGetValue(key, out value);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _inner.GetEnumerator();
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    #endregion
    
    #region PrivateMethods
    private void RaiseIndexerChanged(TKey key)
    {
        OnPropertyChanged($"Item[{key}]");
        OnPropertyChanged(nameof(Keys));
        OnPropertyChanged(nameof(Values));
    }

    private void RaiseNotifyCollectionAdded(TKey key, TValue value)
    {
        const NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Add;
        CollectionChanged?.Invoke(this, new(action, new KeyValuePair<TKey, TValue>(key, value)));
    }
    

    private void RaiseNotifyCollectionRemoved(TKey key, TValue value)
    {
        const NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Remove;
        CollectionChanged?.Invoke(this, new(action, new KeyValuePair<TKey, TValue>(key, value)));
    }
    #endregion
}