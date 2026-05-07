using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace System.Collections.Concurrent;

/// <summary>Represents a thread-safe collection of unique keys and unique values that provides access to an inverse dictionary.</summary>
/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
/// <remarks>
/// All public and protected members of <see cref="ConcurrentBidirectionalDictionary{TKey,TValue}"/> are thread-safe and may be used
/// concurrently from multiple threads.
/// </remarks>
[DebuggerDisplay("Count = {Count}")]
public class ConcurrentBidirectionalDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary, IReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
    where TValue : notnull
{
    private const int DefaultCapacity = 31;

    private readonly Dictionary<TKey, TValue> _forward;
    private readonly Dictionary<TValue, TKey> _reverse;
    private readonly ReaderWriterLockSlim _lock;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConcurrentBidirectionalDictionary{TKey,TValue}"/> class that is empty,
    /// has the default concurrency level and capacity, and uses the default equality comparers.
    /// </summary>
    public ConcurrentBidirectionalDictionary()
        : this(DefaultConcurrencyLevel, DefaultCapacity, null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConcurrentBidirectionalDictionary{TKey,TValue}"/> class that is empty,
    /// has the specified concurrency level and capacity, and uses the default equality comparers.
    /// </summary>
    /// <param name="concurrencyLevel">The estimated number of threads that will update the dictionary concurrently, or -1 to indicate a default value.</param>
    /// <param name="capacity">The initial number of elements that the dictionary can contain.</param>
    public ConcurrentBidirectionalDictionary(int concurrencyLevel, int capacity)
        : this(concurrencyLevel, capacity, null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConcurrentBidirectionalDictionary{TKey,TValue}"/> class that contains
    /// elements copied from the specified collection and uses the default equality comparers.
    /// </summary>
    /// <param name="collection">The collection whose elements are copied to the new dictionary.</param>
    public ConcurrentBidirectionalDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        : this(DefaultConcurrencyLevel, GetCapacityFromCollection(collection), null, null)
    {
        InitializeFromCollection(collection);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConcurrentBidirectionalDictionary{TKey,TValue}"/> class that is empty
    /// and uses the specified equality comparers.
    /// </summary>
    /// <param name="keyComparer">The equality comparer to use when comparing keys.</param>
    /// <param name="valueComparer">The equality comparer to use when comparing values.</param>
    public ConcurrentBidirectionalDictionary(IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer)
        : this(DefaultConcurrencyLevel, DefaultCapacity, keyComparer, valueComparer)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConcurrentBidirectionalDictionary{TKey,TValue}"/> class that contains
    /// elements copied from the specified collection and uses the specified equality comparers.
    /// </summary>
    /// <param name="collection">The collection whose elements are copied to the new dictionary.</param>
    /// <param name="keyComparer">The equality comparer to use when comparing keys.</param>
    /// <param name="valueComparer">The equality comparer to use when comparing values.</param>
    public ConcurrentBidirectionalDictionary(
        IEnumerable<KeyValuePair<TKey, TValue>> collection,
        IEqualityComparer<TKey>? keyComparer,
        IEqualityComparer<TValue>? valueComparer)
        : this(DefaultConcurrencyLevel, GetCapacityFromCollection(collection), keyComparer, valueComparer)
    {
        InitializeFromCollection(collection);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConcurrentBidirectionalDictionary{TKey,TValue}"/> class that contains
    /// elements copied from the specified collection, has the specified concurrency level, and uses the specified equality comparers.
    /// </summary>
    /// <param name="concurrencyLevel">The estimated number of threads that will update the dictionary concurrently, or -1 to indicate a default value.</param>
    /// <param name="collection">The collection whose elements are copied to the new dictionary.</param>
    /// <param name="keyComparer">The equality comparer to use when comparing keys.</param>
    /// <param name="valueComparer">The equality comparer to use when comparing values.</param>
    public ConcurrentBidirectionalDictionary(
        int concurrencyLevel,
        IEnumerable<KeyValuePair<TKey, TValue>> collection,
        IEqualityComparer<TKey>? keyComparer,
        IEqualityComparer<TValue>? valueComparer)
        : this(concurrencyLevel, GetCapacityFromCollection(collection), keyComparer, valueComparer)
    {
        InitializeFromCollection(collection);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConcurrentBidirectionalDictionary{TKey,TValue}"/> class that is empty,
    /// has the specified concurrency level and capacity, and uses the specified equality comparers.
    /// </summary>
    /// <param name="concurrencyLevel">The estimated number of threads that will update the dictionary concurrently, or -1 to indicate a default value.</param>
    /// <param name="capacity">The initial number of elements that the dictionary can contain.</param>
    /// <param name="keyComparer">The equality comparer to use when comparing keys.</param>
    /// <param name="valueComparer">The equality comparer to use when comparing values.</param>
    public ConcurrentBidirectionalDictionary(
        int concurrencyLevel,
        int capacity,
        IEqualityComparer<TKey>? keyComparer,
        IEqualityComparer<TValue>? valueComparer)
    {
        ValidateConcurrencyLevel(concurrencyLevel);
        if (capacity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity));
        }

        _forward = new Dictionary<TKey, TValue>(capacity, keyComparer);
        _reverse = new Dictionary<TValue, TKey>(capacity, valueComparer);
        _lock = new ReaderWriterLockSlim();
        Inverse = new ConcurrentBidirectionalDictionary<TValue, TKey>(_reverse, _forward, _lock, this);
    }

    private ConcurrentBidirectionalDictionary(
        Dictionary<TKey, TValue> forward,
        Dictionary<TValue, TKey> reverse,
        ReaderWriterLockSlim sharedLock,
        ConcurrentBidirectionalDictionary<TValue, TKey> inverse)
    {
        _forward = forward;
        _reverse = reverse;
        _lock = sharedLock;
        Inverse = inverse;
    }

    /// <summary>Gets the inverse <see cref="ConcurrentBidirectionalDictionary{TKey,TValue}"/>.</summary>
    public ConcurrentBidirectionalDictionary<TValue, TKey> Inverse { get; }

    /// <summary>Gets the equality comparer that is used to determine equality of keys.</summary>
    public IEqualityComparer<TKey> KeyComparer => _forward.Comparer;

    /// <summary>Gets the equality comparer that is used to determine equality of values.</summary>
    public IEqualityComparer<TValue> ValueComparer => _reverse.Comparer;

    /// <summary>Gets the number of key/value pairs contained in the dictionary.</summary>
    public int Count
    {
        get
        {
            EnterReadLock();
            try
            {
                return _forward.Count;
            }
            finally
            {
                ExitReadLock();
            }
        }
    }

    /// <summary>Gets a value that indicates whether the dictionary is empty.</summary>
    public bool IsEmpty
    {
        get
        {
            EnterReadLock();
            try
            {
                return _forward.Count == 0;
            }
            finally
            {
                ExitReadLock();
            }
        }
    }

    /// <summary>Gets a snapshot containing all keys in the dictionary.</summary>
    public ICollection<TKey> Keys => GetKeys();

    /// <summary>Gets a snapshot containing all values in the dictionary.</summary>
    public ICollection<TValue> Values => GetValues();

    /// <summary>Gets or sets the value associated with the specified key.</summary>
    /// <param name="key">The key of the value to get or set.</param>
    public TValue this[TKey key]
    {
        get
        {
            ThrowIfNull(key, nameof(key));
            EnterReadLock();
            try
            {
                return _forward[key];
            }
            finally
            {
                ExitReadLock();
            }
        }
        set
        {
            ThrowIfNull(key, nameof(key));
            ThrowIfNull(value, nameof(value));
            EnterWriteLock();
            try
            {
                SetItemNoLock(key, value);
            }
            finally
            {
                ExitWriteLock();
            }
        }
    }

    ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;

    ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot => throw new NotSupportedException("The SyncRoot property is not supported.");

    bool IDictionary.IsFixedSize => false;

    bool IDictionary.IsReadOnly => false;

    ICollection IDictionary.Keys => (ICollection)Keys;

    ICollection IDictionary.Values => (ICollection)Values;

    object? IDictionary.this[object key]
    {
        get
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return key is TKey typedKey && TryGetValue(typedKey, out TValue? value) ? value : null;
        }
        set
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key is not TKey typedKey)
            {
                throw new ArgumentException($"The key is not of type {typeof(TKey)}.", nameof(key));
            }

            ThrowIfInvalidObjectValue(value);
            this[typedKey] = (TValue)value!;
        }
    }

    /// <summary>Attempts to add the specified key and value to the dictionary.</summary>
    public bool TryAdd(TKey key, TValue value)
    {
        ThrowIfNull(key, nameof(key));
        ThrowIfNull(value, nameof(value));

        EnterWriteLock();
        try
        {
            return TryAddNoLock(key, value);
        }
        finally
        {
            ExitWriteLock();
        }
    }

    /// <summary>Determines whether the dictionary contains the specified key.</summary>
    public bool ContainsKey(TKey key)
    {
        ThrowIfNull(key, nameof(key));

        EnterReadLock();
        try
        {
            return _forward.ContainsKey(key);
        }
        finally
        {
            ExitReadLock();
        }
    }

    private bool ContainsValueCore(TValue value)
    {
        ThrowIfNull(value, nameof(value));

        EnterReadLock();
        try
        {
            return _reverse.ContainsKey(value);
        }
        finally
        {
            ExitReadLock();
        }
    }

    /// <summary>Attempts to remove and return the value with the specified key from the dictionary.</summary>
    public bool TryRemove(TKey key, out TValue value)
    {
        ThrowIfNull(key, nameof(key));

        EnterWriteLock();
        try
        {
            if (!_forward.TryGetValue(key, out value!))
            {
                value = default!;
                return false;
            }

            _forward.Remove(key);
            _reverse.Remove(value);
            return true;
        }
        finally
        {
            ExitWriteLock();
        }
    }

    /// <summary>Removes a key and value from the dictionary.</summary>
    public bool TryRemove(KeyValuePair<TKey, TValue> item)
    {
        ThrowIfNull(item.Key, nameof(item));
        ThrowIfNull(item.Value, nameof(item));

        EnterWriteLock();
        try
        {
            if (!_forward.TryGetValue(item.Key, out TValue? existing) || !ValueComparer.Equals(existing, item.Value))
            {
                return false;
            }

            _forward.Remove(item.Key);
            _reverse.Remove(item.Value);
            return true;
        }
        finally
        {
            ExitWriteLock();
        }
    }

    /// <summary>Attempts to get the value associated with the specified key.</summary>
    public bool TryGetValue(TKey key, out TValue value)
    {
        ThrowIfNull(key, nameof(key));

        EnterReadLock();
        try
        {
            return _forward.TryGetValue(key, out value!);
        }
        finally
        {
            ExitReadLock();
        }
    }

    /// <summary>Updates the value associated with a key if the existing value is equal to the comparison value.</summary>
    public bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue)
    {
        ThrowIfNull(key, nameof(key));
        ThrowIfNull(newValue, nameof(newValue));
        ThrowIfNull(comparisonValue, nameof(comparisonValue));

        EnterWriteLock();
        try
        {
            return TryUpdateNoLock(key, newValue, comparisonValue, throwOnDuplicateValue: false, out _);
        }
        finally
        {
            ExitWriteLock();
        }
    }

    /// <summary>Removes all keys and values from the dictionary.</summary>
    public void Clear()
    {
        EnterWriteLock();
        try
        {
            _forward.Clear();
            _reverse.Clear();
        }
        finally
        {
            ExitWriteLock();
        }
    }

    /// <summary>Copies the key and value pairs stored in the dictionary to a new array.</summary>
    public KeyValuePair<TKey, TValue>[] ToArray()
    {
        EnterReadLock();
        try
        {
            if (_forward.Count == 0)
            {
                return Array.Empty<KeyValuePair<TKey, TValue>>();
            }

            var array = new KeyValuePair<TKey, TValue>[_forward.Count];
            int index = 0;
            foreach (KeyValuePair<TKey, TValue> pair in _forward)
            {
                array[index] = pair;
                index++;
            }

            return array;
        }
        finally
        {
            ExitReadLock();
        }
    }

    /// <summary>Adds a key/value pair to the dictionary if the key does not already exist.</summary>
    public TValue GetOrAdd(TKey key, TValue value)
    {
        ThrowIfNull(key, nameof(key));
        ThrowIfNull(value, nameof(value));

        EnterWriteLock();
        try
        {
            if (_forward.TryGetValue(key, out TValue? existing))
            {
                return existing;
            }

            if (!TryAddNoLock(key, value))
            {
                throw new ArgumentException("The value already exists.", nameof(value));
            }

            return value;
        }
        finally
        {
            ExitWriteLock();
        }
    }

    /// <summary>Adds a key/value pair to the dictionary by using the specified function if the key does not already exist.</summary>
    public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
    {
        ThrowIfNull(key, nameof(key));
        if (valueFactory is null)
        {
            throw new ArgumentNullException(nameof(valueFactory));
        }

        if (TryGetExisting(key, out TValue? existing))
        {
            return existing;
        }

        TValue value = valueFactory(key);
        ThrowIfNull(value, nameof(valueFactory));
        return GetOrAdd(key, value);
    }

    /// <summary>Adds a key/value pair to the dictionary by using the specified function and argument if the key does not already exist.</summary>
    public TValue GetOrAdd<TArg>(TKey key, Func<TKey, TArg, TValue> valueFactory, TArg factoryArgument)
    {
        ThrowIfNull(key, nameof(key));
        if (valueFactory is null)
        {
            throw new ArgumentNullException(nameof(valueFactory));
        }

        if (TryGetExisting(key, out TValue? existing))
        {
            return existing;
        }

        TValue value = valueFactory(key, factoryArgument);
        ThrowIfNull(value, nameof(valueFactory));
        return GetOrAdd(key, value);
    }

    /// <summary>Adds or updates a key/value pair in the dictionary.</summary>
    public TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
    {
        ThrowIfNull(key, nameof(key));
        ThrowIfNull(addValue, nameof(addValue));
        if (updateValueFactory is null)
        {
            throw new ArgumentNullException(nameof(updateValueFactory));
        }

        return AddOrUpdateCore(key, _ => addValue, (k, oldValue) => updateValueFactory(k, oldValue));
    }

    /// <summary>Adds or updates a key/value pair in the dictionary by using the specified functions.</summary>
    public TValue AddOrUpdate(TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
    {
        ThrowIfNull(key, nameof(key));
        if (addValueFactory is null)
        {
            throw new ArgumentNullException(nameof(addValueFactory));
        }
        if (updateValueFactory is null)
        {
            throw new ArgumentNullException(nameof(updateValueFactory));
        }

        return AddOrUpdateCore(key, addValueFactory, updateValueFactory);
    }

    /// <summary>Adds or updates a key/value pair in the dictionary by using the specified functions and argument.</summary>
    public TValue AddOrUpdate<TArg>(
        TKey key,
        Func<TKey, TArg, TValue> addValueFactory,
        Func<TKey, TValue, TArg, TValue> updateValueFactory,
        TArg factoryArgument)
    {
        ThrowIfNull(key, nameof(key));
        if (addValueFactory is null)
        {
            throw new ArgumentNullException(nameof(addValueFactory));
        }
        if (updateValueFactory is null)
        {
            throw new ArgumentNullException(nameof(updateValueFactory));
        }

        return AddOrUpdateCore(
            key,
            k => addValueFactory(k, factoryArgument),
            (k, oldValue) => updateValueFactory(k, oldValue, factoryArgument));
    }

    private void AddCore(TKey key, TValue value)
    {
        if (!TryAdd(key, value))
        {
            throw new ArgumentException("The same key or value already exists.");
        }
    }

    /// <summary>Returns an enumerator that iterates through the dictionary.</summary>
    public Enumerator GetEnumerator() => new(ToArray());

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => AddCore(key, value);

    bool IDictionary<TKey, TValue>.Remove(TKey key) => TryRemove(key, out _);

    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => AddCore(item.Key, item.Value);

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
    {
        ThrowIfNull(item.Key, nameof(item));
        ThrowIfNull(item.Value, nameof(item));

        EnterReadLock();
        try
        {
            return _forward.TryGetValue(item.Key, out TValue? value) && ValueComparer.Equals(value, item.Value);
        }
        finally
        {
            ExitReadLock();
        }
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => TryRemove(item);

    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        ToArray().CopyTo(array, arrayIndex);
    }

    void IDictionary.Add(object key, object? value)
    {
        if (key is null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (key is not TKey typedKey)
        {
            throw new ArgumentException($"The key is not of type {typeof(TKey)}.", nameof(key));
        }

        ThrowIfInvalidObjectValue(value);
        AddCore(typedKey, (TValue)value!);
    }

    bool IDictionary.Contains(object key)
    {
        if (key is null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        return key is TKey typedKey && ContainsKey(typedKey);
    }

    IDictionaryEnumerator IDictionary.GetEnumerator() => new DictionaryEnumerator(ToArray());

    void IDictionary.Remove(object key)
    {
        if (key is null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (key is TKey typedKey)
        {
            TryRemove(typedKey, out _);
        }
    }

    void ICollection.CopyTo(Array array, int index)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        KeyValuePair<TKey, TValue>[] snapshot = ToArray();
        if (array.Length - snapshot.Length < index)
        {
            throw new ArgumentException("The target array is not large enough.", nameof(array));
        }

        if (array is KeyValuePair<TKey, TValue>[] pairs)
        {
            snapshot.CopyTo(pairs, index);
            return;
        }

        if (array is DictionaryEntry[] entries)
        {
            for (int i = 0; i < snapshot.Length; i++)
            {
                entries[index + i] = new DictionaryEntry(snapshot[i].Key, snapshot[i].Value);
            }

            return;
        }

        if (array is object[] objects)
        {
            for (int i = 0; i < snapshot.Length; i++)
            {
                objects[index + i] = snapshot[i];
            }

            return;
        }

        throw new ArgumentException("The target array type is invalid.", nameof(array));
    }

    private void InitializeFromCollection(IEnumerable<KeyValuePair<TKey, TValue>> collection)
    {
        EnterWriteLock();
        try
        {
            foreach (KeyValuePair<TKey, TValue> pair in collection)
            {
                ThrowIfNull(pair.Key, nameof(collection));
                ThrowIfNull(pair.Value, nameof(collection));
                if (!TryAddNoLock(pair.Key, pair.Value))
                {
                    throw new ArgumentException("The source contains duplicate keys or values.", nameof(collection));
                }
            }
        }
        finally
        {
            ExitWriteLock();
        }
    }

    private TValue AddOrUpdateCore(TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
    {
        while (true)
        {
            if (TryGetExisting(key, out TValue? oldValue))
            {
                TValue newValue = updateValueFactory(key, oldValue);
                ThrowIfNull(newValue, nameof(updateValueFactory));

                EnterWriteLock();
                try
                {
                    if (TryUpdateNoLock(key, newValue, oldValue, throwOnDuplicateValue: true, out bool retry))
                    {
                        return newValue;
                    }

                    if (!retry)
                    {
                        continue;
                    }
                }
                finally
                {
                    ExitWriteLock();
                }

                continue;
            }

            TValue addValue = addValueFactory(key);
            ThrowIfNull(addValue, nameof(addValueFactory));
            return GetOrAdd(key, addValue);
        }
    }

    private bool TryGetExisting(TKey key, out TValue value)
    {
        EnterReadLock();
        try
        {
            return _forward.TryGetValue(key, out value!);
        }
        finally
        {
            ExitReadLock();
        }
    }

    private bool TryAddNoLock(TKey key, TValue value)
    {
        if (_forward.ContainsKey(key) || _reverse.ContainsKey(value))
        {
            return false;
        }

        _forward.Add(key, value);
        _reverse.Add(value, key);
        return true;
    }

    private void SetItemNoLock(TKey key, TValue value)
    {
        if (_forward.TryGetValue(key, out TValue? oldValue))
        {
            if (ValueComparer.Equals(oldValue, value))
            {
                return;
            }

            if (_reverse.TryGetValue(value, out TKey? owner) && !KeyComparer.Equals(owner, key))
            {
                throw new ArgumentException("The value already exists.", nameof(value));
            }

            _forward[key] = value;
            _reverse.Remove(oldValue);
            _reverse.Add(value, key);
            return;
        }

        if (!TryAddNoLock(key, value))
        {
            throw new ArgumentException("The value already exists.", nameof(value));
        }
    }

    private bool TryUpdateNoLock(TKey key, TValue newValue, TValue comparisonValue, bool throwOnDuplicateValue, out bool retry)
    {
        retry = false;
        if (!_forward.TryGetValue(key, out TValue? current))
        {
            return false;
        }

        if (!ValueComparer.Equals(current, comparisonValue))
        {
            retry = true;
            return false;
        }

        if (ValueComparer.Equals(current, newValue))
        {
            return true;
        }

        if (_reverse.TryGetValue(newValue, out TKey? owner) && !KeyComparer.Equals(owner, key))
        {
            if (throwOnDuplicateValue)
            {
                throw new ArgumentException("The value already exists.", nameof(newValue));
            }

            return false;
        }

        _forward[key] = newValue;
        _reverse.Remove(current);
        _reverse.Add(newValue, key);
        return true;
    }

    private ReadOnlyCollection<TKey> GetKeys()
    {
        EnterReadLock();
        try
        {
            return new ReadOnlyCollection<TKey>(_forward.Keys.ToArray());
        }
        finally
        {
            ExitReadLock();
        }
    }

    private ReadOnlyCollection<TValue> GetValues()
    {
        EnterReadLock();
        try
        {
            return new ReadOnlyCollection<TValue>(_forward.Values.ToArray());
        }
        finally
        {
            ExitReadLock();
        }
    }

    private static int GetCapacityFromCollection(IEnumerable<KeyValuePair<TKey, TValue>> collection)
    {
        if (collection is null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        if (collection is ICollection<KeyValuePair<TKey, TValue>> c)
        {
            return Math.Max(DefaultCapacity, c.Count);
        }

        if (collection is IReadOnlyCollection<KeyValuePair<TKey, TValue>> rc)
        {
            return Math.Max(DefaultCapacity, rc.Count);
        }

        return DefaultCapacity;
    }

    private static int DefaultConcurrencyLevel => Environment.ProcessorCount;

    private static void ValidateConcurrencyLevel(int concurrencyLevel)
    {
        if (concurrencyLevel <= 0 && concurrencyLevel != -1)
        {
            throw new ArgumentOutOfRangeException(nameof(concurrencyLevel));
        }
    }

    private static void ThrowIfNull<T>(T value, string paramName)
    {
        if (value is null)
        {
            throw new ArgumentNullException(paramName);
        }
    }

    private static void ThrowIfInvalidObjectValue(object? value)
    {
        if (value is not null)
        {
            if (value is not TValue)
            {
                throw new ArgumentException($"The value is not of type {typeof(TValue)}.", nameof(value));
            }
        }
        else if (default(TValue) is not null)
        {
            throw new ArgumentException($"The value is not of type {typeof(TValue)}.", nameof(value));
        }
    }

    private void EnterReadLock() => _lock.EnterReadLock();

    private void ExitReadLock() => _lock.ExitReadLock();

    private void EnterWriteLock() => _lock.EnterWriteLock();

    private void ExitWriteLock() => _lock.ExitWriteLock();

    /// <summary>Provides an enumerator implementation for the dictionary.</summary>
    public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator
    {
        private readonly KeyValuePair<TKey, TValue>[] _snapshot;
        private int _index;

        internal Enumerator(KeyValuePair<TKey, TValue>[] snapshot)
        {
            _snapshot = snapshot;
            _index = -1;
        }

        /// <summary>Gets the element at the current position of the enumerator.</summary>
        public readonly KeyValuePair<TKey, TValue> Current
        {
            get
            {
                if ((uint)_index >= (uint)_snapshot.Length)
                {
                    throw new InvalidOperationException("Enumeration has either not started or has already finished.");
                }

                return _snapshot[_index];
            }
        }

        readonly object IEnumerator.Current => Current;

        /// <summary>Gets both the key and the value of the current dictionary entry.</summary>
        public readonly DictionaryEntry Entry => new(Current.Key, Current.Value);

        /// <summary>Gets the key of the current dictionary entry.</summary>
        public readonly object Key => Current.Key;

        /// <summary>Gets the value of the current dictionary entry.</summary>
        public readonly object? Value => Current.Value;

        /// <summary>Advances the enumerator to the next element of the dictionary.</summary>
        public bool MoveNext()
        {
            int next = _index + 1;
            if (next >= _snapshot.Length)
            {
                _index = _snapshot.Length;
                return false;
            }

            _index = next;
            return true;
        }

        void IEnumerator.Reset() => _index = -1;

        /// <summary>Releases all resources used by the enumerator.</summary>
        public readonly void Dispose()
        {
        }
    }

    private sealed class DictionaryEnumerator : IDictionaryEnumerator
    {
        private readonly IEnumerator<KeyValuePair<TKey, TValue>> _enumerator;

        internal DictionaryEnumerator(KeyValuePair<TKey, TValue>[] snapshot) =>
            _enumerator = ((IEnumerable<KeyValuePair<TKey, TValue>>)snapshot).GetEnumerator();

        public DictionaryEntry Entry => new(_enumerator.Current.Key, _enumerator.Current.Value);

        public object Key => _enumerator.Current.Key;

        public object? Value => _enumerator.Current.Value;

        public object Current => Entry;

        public bool MoveNext() => _enumerator.MoveNext();

        public void Reset() => _enumerator.Reset();
    }
}
