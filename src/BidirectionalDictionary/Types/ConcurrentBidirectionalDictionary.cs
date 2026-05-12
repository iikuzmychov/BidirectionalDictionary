using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

#if NET9_0_OR_GREATER
using LockType = System.Threading.Lock;
#else
using LockType = System.Object;
#endif

namespace System.Collections.Concurrent;

/// <summary>Represents a thread-safe collection of unique keys and unique values that provides access to an inverse dictionary.</summary>
/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
/// <remarks>
/// All public and protected members of <see cref="ConcurrentBidirectionalDictionary{TKey,TValue}"/> are thread-safe and may be used
/// concurrently from multiple threads.
/// </remarks>
[DebuggerDisplay("Count = {Count}")]
public class ConcurrentBidirectionalDictionary<TKey, TValue> : IBidirectionalDictionary<TKey, TValue>, IReadOnlyBidirectionalDictionary<TKey, TValue>, IDictionary
    where TKey : notnull
    where TValue : notnull
{
    private const int DefaultCapacity = 31;

    private readonly ConcurrentDictionary<TKey, TValue> _forward;
    private readonly ConcurrentDictionary<TValue, TKey> _reverse;
    private readonly LockType[] _locks;
    private readonly int _keyLockBase;
    private readonly int _valueLockBase;

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
        concurrencyLevel = ResolveConcurrencyLevel(concurrencyLevel);

        if (capacity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity));
        }

        KeyComparer = keyComparer ?? EqualityComparer<TKey>.Default;
        ValueComparer = valueComparer ?? EqualityComparer<TValue>.Default;
        _forward = new ConcurrentDictionary<TKey, TValue>(concurrencyLevel, capacity, KeyComparer);
        _reverse = new ConcurrentDictionary<TValue, TKey>(concurrencyLevel, capacity, ValueComparer);
        _locks = CreateLocks(concurrencyLevel * 2);
        _keyLockBase = 0;
        _valueLockBase = concurrencyLevel;
        Inverse = new ConcurrentBidirectionalDictionary<TValue, TKey>(_reverse, _forward, ValueComparer, KeyComparer, _locks, _valueLockBase, _keyLockBase, this);
    }

    private ConcurrentBidirectionalDictionary(
        ConcurrentDictionary<TKey, TValue> forward,
        ConcurrentDictionary<TValue, TKey> reverse,
        IEqualityComparer<TKey> keyComparer,
        IEqualityComparer<TValue> valueComparer,
        LockType[] locks,
        int keyLockBase,
        int valueLockBase,
        ConcurrentBidirectionalDictionary<TValue, TKey> inverse)
    {
        _forward = forward;
        _reverse = reverse;
        KeyComparer = keyComparer;
        ValueComparer = valueComparer;
        _locks = locks;
        _keyLockBase = keyLockBase;
        _valueLockBase = valueLockBase;
        Inverse = inverse;
    }

    /// <summary>Gets the inverse <see cref="ConcurrentBidirectionalDictionary{TKey,TValue}"/>.</summary>
    public ConcurrentBidirectionalDictionary<TValue, TKey> Inverse { get; }

    IBidirectionalDictionary<TValue, TKey> IBidirectionalDictionary<TKey, TValue>.Inverse => Inverse;

    IReadOnlyBidirectionalDictionary<TValue, TKey> IReadOnlyBidirectionalDictionary<TKey, TValue>.Inverse => Inverse;

    /// <summary>Gets the equality comparer that is used to determine equality of keys.</summary>
    public IEqualityComparer<TKey> KeyComparer { get; }

    /// <summary>Gets the equality comparer that is used to determine equality of values.</summary>
    public IEqualityComparer<TValue> ValueComparer { get; }

    /// <summary>Gets the number of key/value pairs contained in the dictionary.</summary>
    public int Count => _forward.Count;

    /// <summary>Gets a value that indicates whether the dictionary is empty.</summary>
    public bool IsEmpty => _forward.IsEmpty;

    /// <summary>Gets a snapshot containing all keys in the dictionary.</summary>
    public ICollection<TKey> Keys => _forward.Keys;

    /// <summary>Gets a snapshot containing all values in the dictionary.</summary>
    public ICollection<TValue> Values => _forward.Values;

    /// <summary>Gets or sets the value associated with the specified key.</summary>
    /// <param name="key">The key of the value to get or set.</param>
    public TValue this[TKey key]
    {
        get
        {
            ThrowIfNull(key, nameof(key));
            return _forward[key];
        }
        set
        {
            ThrowIfNull(key, nameof(key));
            ThrowIfNull(value, nameof(value));
            SetItem(key, value);
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

        int keyIndex = GetKeyLockIndex(key);
        int valueIndex = GetValueLockIndex(value);

        EnterPairLock(keyIndex, valueIndex, out int first, out int second);

        try
        {
            return TryAddNoLock(key, value);
        }
        finally
        {
            ExitPairLock(first, second);
        }
    }

    /// <summary>Determines whether the dictionary contains the specified key.</summary>
    public bool ContainsKey(TKey key)
    {
        ThrowIfNull(key, nameof(key));
        return _forward.ContainsKey(key);
    }

    /// <summary>Determines whether the dictionary contains the specified value.</summary>
    public bool ContainsValue(TValue value)
    {
        ThrowIfNull(value, nameof(value));
        return _reverse.ContainsKey(value);
    }

    /// <summary>Attempts to remove and return the value with the specified key from the dictionary.</summary>
    public bool TryRemove(TKey key, out TValue value)
    {
        ThrowIfNull(key, nameof(key));

        int keyIndex = GetKeyLockIndex(key);

        while (true)
        {
            if (!_forward.TryGetValue(key, out value!))
            {
                return false;
            }

            var candidate = value;
            var valueIndex = GetValueLockIndex(candidate);

            EnterPairLock(keyIndex, valueIndex, out int first, out int second);

            try
            {
                if (!_forward.TryGetValue(key, out TValue? current))
                {
                    value = default!;
                    return false;
                }

                if (!ValueComparer.Equals(current, candidate))
                {
                    continue;
                }

                _forward.TryRemove(key, out _);
                _reverse.TryRemove(candidate, out _);

                return true;
            }
            finally
            {
                ExitPairLock(first, second);
            }
        }
    }

    /// <summary>Removes a key and value from the dictionary.</summary>
    public bool TryRemove(KeyValuePair<TKey, TValue> item)
    {
        ThrowIfNull(item.Key, nameof(item));
        ThrowIfNull(item.Value, nameof(item));

        var keyIndex = GetKeyLockIndex(item.Key);
        var valueIndex = GetValueLockIndex(item.Value);

        EnterPairLock(keyIndex, valueIndex, out int first, out int second);

        try
        {
            if (!_forward.TryGetValue(item.Key, out TValue? existing) || !ValueComparer.Equals(existing, item.Value))
            {
                return false;
            }

            _forward.TryRemove(item.Key, out _);
            _reverse.TryRemove(item.Value, out _);

            return true;
        }
        finally
        {
            ExitPairLock(first, second);
        }
    }

    /// <summary>Attempts to get the value associated with the specified key.</summary>
    public bool TryGetValue(TKey key, out TValue value)
    {
        ThrowIfNull(key, nameof(key));
        return _forward.TryGetValue(key, out value!);
    }

    /// <summary>Updates the value associated with a key if the existing value is equal to the comparison value.</summary>
    public bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue)
    {
        ThrowIfNull(key, nameof(key));
        ThrowIfNull(newValue, nameof(newValue));
        ThrowIfNull(comparisonValue, nameof(comparisonValue));

        return TryUpdateCore(key, newValue, comparisonValue, throwOnDuplicateValue: false);
    }

    /// <summary>Removes all keys and values from the dictionary.</summary>
    public void Clear()
    {
        EnterAllLocks();

        try
        {
            _forward.Clear();
            _reverse.Clear();
        }
        finally
        {
            ExitAllLocks();
        }
    }

    /// <summary>Copies the key and value pairs stored in the dictionary to a new array.</summary>
    public KeyValuePair<TKey, TValue>[] ToArray() => _forward.ToArray();

    /// <summary>Adds a key/value pair to the dictionary if the key does not already exist.</summary>
    public TValue GetOrAdd(TKey key, TValue value)
    {
        ThrowIfNull(key, nameof(key));
        ThrowIfNull(value, nameof(value));

        var keyIndex = GetKeyLockIndex(key);
        var valueIndex = GetValueLockIndex(value);

        EnterPairLock(keyIndex, valueIndex, out int first, out int second);

        try
        {
            if (_forward.TryGetValue(key, out TValue? existing))
            {
                return existing;
            }

            if (!_reverse.TryAdd(value, key))
            {
                throw new ArgumentException("The value already exists.", nameof(value));
            }

            _forward.TryAdd(key, value);
            return value;
        }
        finally
        {
            ExitPairLock(first, second);
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

        if (_forward.TryGetValue(key, out TValue? existing))
        {
            return existing;
        }

        var value = valueFactory(key);
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

        if (_forward.TryGetValue(key, out TValue? existing))
        {
            return existing;
        }

        var value = valueFactory(key, factoryArgument);
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

        return AddOrUpdateCore(key, addValue, updateValueFactory);
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

        return AddOrUpdateCore(key, addValueFactory, updateValueFactory, factoryArgument);
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

        return _forward.TryGetValue(item.Key, out TValue? value) && ValueComparer.Equals(value, item.Value);
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
        foreach (KeyValuePair<TKey, TValue> pair in collection)
        {
            ThrowIfNull(pair.Key, nameof(collection));
            ThrowIfNull(pair.Value, nameof(collection));

            if (!TryAdd(pair.Key, pair.Value))
            {
                throw new ArgumentException("The source contains duplicate keys or values.", nameof(collection));
            }
        }
    }

    private TValue AddOrUpdateCore(TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
    {
        while (true)
        {
            if (_forward.TryGetValue(key, out TValue? oldValue))
            {
                var newValue = updateValueFactory(key, oldValue);
                ThrowIfNull(newValue, nameof(updateValueFactory));

                if (TryUpdateCore(key, newValue, oldValue, throwOnDuplicateValue: true))
                {
                    return newValue;
                }

                continue;
            }

            var addValue = addValueFactory(key);
            ThrowIfNull(addValue, nameof(addValueFactory));

            return GetOrAdd(key, addValue);
        }
    }

    private TValue AddOrUpdateCore(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
    {
        while (true)
        {
            if (_forward.TryGetValue(key, out TValue? oldValue))
            {
                TValue newValue = updateValueFactory(key, oldValue);
                ThrowIfNull(newValue, nameof(updateValueFactory));

                if (TryUpdateCore(key, newValue, oldValue, throwOnDuplicateValue: true))
                {
                    return newValue;
                }

                continue;
            }

            return GetOrAdd(key, addValue);
        }
    }

    private TValue AddOrUpdateCore<TArg>(
        TKey key,
        Func<TKey, TArg, TValue> addValueFactory,
        Func<TKey, TValue, TArg, TValue> updateValueFactory,
        TArg factoryArgument)
    {
        while (true)
        {
            if (_forward.TryGetValue(key, out TValue? oldValue))
            {
                var newValue = updateValueFactory(key, oldValue, factoryArgument);
                ThrowIfNull(newValue, nameof(updateValueFactory));

                if (TryUpdateCore(key, newValue, oldValue, throwOnDuplicateValue: true))
                {
                    return newValue;
                }

                continue;
            }

            var addValue = addValueFactory(key, factoryArgument);
            ThrowIfNull(addValue, nameof(addValueFactory));

            return GetOrAdd(key, addValue);
        }
    }

    private bool TryAddNoLock(TKey key, TValue value)
    {
        if (!_forward.TryAdd(key, value))
        {
            return false;
        }

        if (!_reverse.TryAdd(value, key))
        {
            _forward.TryRemove(key, out _);
            return false;
        }

        return true;
    }

    private void SetItem(TKey key, TValue value)
    {
        int keyIndex = GetKeyLockIndex(key);

        while (true)
        {
            if (_forward.TryGetValue(key, out TValue? oldValue))
            {
                if (ValueComparer.Equals(oldValue, value))
                {
                    return;
                }

                var oldValueIndex = GetValueLockIndex(oldValue);
                var newValueIndex = GetValueLockIndex(value);

                EnterTripleLock(keyIndex, oldValueIndex, newValueIndex, out int first, out int second, out int third);

                try
                {
                    if (!_forward.TryGetValue(key, out TValue? current))
                    {
                        continue;
                    }

                    if (!ValueComparer.Equals(current, oldValue))
                    {
                        continue;
                    }

                    if (_reverse.ContainsKey(value))
                    {
                        throw new ArgumentException("The value already exists.", nameof(value));
                    }

                    _forward[key] = value;
                    _reverse.TryRemove(oldValue, out _);
                    _reverse[value] = key;
                    return;
                }
                finally
                {
                    ExitTripleLock(first, second, third);
                }
            }

            var addValueIndex = GetValueLockIndex(value);
            EnterPairLock(keyIndex, addValueIndex, out int addFirst, out int addSecond);

            try
            {
                if (!_forward.TryAdd(key, value))
                {
                    continue;
                }

                if (!_reverse.TryAdd(value, key))
                {
                    _forward.TryRemove(key, out _);
                    throw new ArgumentException("The value already exists.", nameof(value));
                }

                return;
            }
            finally
            {
                ExitPairLock(addFirst, addSecond);
            }
        }
    }

    private bool TryUpdateCore(TKey key, TValue newValue, TValue comparisonValue, bool throwOnDuplicateValue)
    {
        var keyIndex = GetKeyLockIndex(key);
        var comparisonValueIndex = GetValueLockIndex(comparisonValue);
        var newValueIndex = GetValueLockIndex(newValue);

        EnterTripleLock(keyIndex, comparisonValueIndex, newValueIndex, out int first, out int second, out int third);

        try
        {
            if (!_forward.TryGetValue(key, out TValue? current))
            {
                return false;
            }

            if (!ValueComparer.Equals(current, comparisonValue))
            {
                return false;
            }

            if (ValueComparer.Equals(current, newValue))
            {
                return true;
            }

            if (_reverse.ContainsKey(newValue))
            {
                if (throwOnDuplicateValue)
                {
                    throw new ArgumentException("The value already exists.", nameof(newValue));
                }

                return false;
            }

            _forward[key] = newValue;
            _reverse.TryRemove(current, out _);
            _reverse[newValue] = key;

            return true;
        }
        finally
        {
            ExitTripleLock(first, second, third);
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

    private int GetKeyLockIndex(TKey key)
    {
        return _keyLockBase + (int)((uint)KeyComparer.GetHashCode(key) % (uint)(_locks.Length / 2));
    }

    private int GetValueLockIndex(TValue value)
    {
        return _valueLockBase + (int)((uint)ValueComparer.GetHashCode(value) % (uint)(_locks.Length / 2));
    }

    private static void EnterLock(LockType lockObject)
    {
#if NET9_0_OR_GREATER
        lockObject.Enter();
#else
        Monitor.Enter(lockObject);
#endif
    }

    private static void ExitLock(LockType lockObject)
    {
#if NET9_0_OR_GREATER
        lockObject.Exit();
#else
        Monitor.Exit(lockObject);
#endif
    }

    private void EnterPairLock(int indexA, int indexB, out int first, out int second)
    {
        // indexA and indexB live in disjoint key/value stripe ranges and are always distinct.
        if (indexA < indexB)
        {
            first = indexA;
            second = indexB;
        }
        else
        {
            first = indexB;
            second = indexA;
        }

        EnterLock(_locks[first]);
        EnterLock(_locks[second]);
    }

    private void ExitPairLock(int first, int second)
    {
        ExitLock(_locks[second]);
        ExitLock(_locks[first]);
    }

    private void EnterTripleLock(int keyIndex, int valueIndexA, int valueIndexB, out int first, out int second, out int third)
    {
        // valueIndexA and valueIndexB share the value range and may collide; keyIndex is in the
        // disjoint key range, so it is always distinct from both.
        int v1, v2;
        if (valueIndexA == valueIndexB)
        {
            v1 = valueIndexA;
            v2 = -1;
        }
        else if (valueIndexA < valueIndexB)
        {
            v1 = valueIndexA;
            v2 = valueIndexB;
        }
        else
        {
            v1 = valueIndexB;
            v2 = valueIndexA;
        }

        if (v2 == -1)
        {
            if (keyIndex < v1)
            {
                first = keyIndex;
                second = v1;
            }
            else
            {
                first = v1;
                second = keyIndex;
            }

            third = -1;
        }
        else if (keyIndex < v1)
        {
            first = keyIndex;
            second = v1;
            third = v2;
        }
        else if (keyIndex < v2)
        {
            first = v1;
            second = keyIndex;
            third = v2;
        }
        else
        {
            first = v1;
            second = v2;
            third = keyIndex;
        }

        EnterLock(_locks[first]);
        EnterLock(_locks[second]);
        if (third >= 0)
        {
            EnterLock(_locks[third]);
        }
    }

    private void ExitTripleLock(int first, int second, int third)
    {
        if (third >= 0)
        {
            ExitLock(_locks[third]);
        }

        ExitLock(_locks[second]);
        ExitLock(_locks[first]);
    }

    private void EnterAllLocks()
    {
        for (int i = 0; i < _locks.Length; i++)
        {
            EnterLock(_locks[i]);
        }
    }

    private void ExitAllLocks()
    {
        for (int i = _locks.Length - 1; i >= 0; i--)
        {
            ExitLock(_locks[i]);
        }
    }

    private static int DefaultConcurrencyLevel => Environment.ProcessorCount;

    private static int ResolveConcurrencyLevel(int concurrencyLevel)
    {
        if (concurrencyLevel <= 0)
        {
            if (concurrencyLevel != -1)
            {
                throw new ArgumentOutOfRangeException(nameof(concurrencyLevel));
            }

            return DefaultConcurrencyLevel;
        }

        return concurrencyLevel;
    }

    private static LockType[] CreateLocks(int count)
    {
        var locks = new LockType[count];

        for (int i = 0; i < count; i++)
        {
            locks[i] = new LockType();
        }

        return locks;
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

    /// <summary>Provides an enumerator implementation for the dictionary.</summary>
    public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
    {
        private readonly KeyValuePair<TKey, TValue>[] _snapshot;
        private int _index;

        internal Enumerator(KeyValuePair<TKey, TValue>[] snapshot)
        {
            _snapshot = snapshot;
            _index = -1;
            Current = default;
        }

        /// <summary>Gets the element at the current position of the enumerator.</summary>
        public KeyValuePair<TKey, TValue> Current { get; private set; }

        readonly object IEnumerator.Current => Current;

        /// <summary>Advances the enumerator to the next element of the dictionary.</summary>
        public bool MoveNext()
        {
            var next = _index + 1;

            if ((uint)next >= (uint)_snapshot.Length)
            {
                _index = _snapshot.Length;
                return false;
            }

            _index = next;
            Current = _snapshot[next];

            return true;
        }

        void IEnumerator.Reset()
        {
            _index = -1;
            Current = default;
        }

        /// <summary>Releases all resources used by the enumerator.</summary>
        public readonly void Dispose()
        {
        }
    }

    private sealed class DictionaryEnumerator : IDictionaryEnumerator
    {
        private readonly IEnumerator<KeyValuePair<TKey, TValue>> _enumerator;

        internal DictionaryEnumerator(KeyValuePair<TKey, TValue>[] snapshot)
        {
            _enumerator = new Enumerator(snapshot);
        }

        public DictionaryEntry Entry => new(_enumerator.Current.Key, _enumerator.Current.Value);

        public object Key => _enumerator.Current.Key;

        public object? Value => _enumerator.Current.Value;

        public object Current => Entry;

        public bool MoveNext() => _enumerator.MoveNext();

        public void Reset() => _enumerator.Reset();
    }
}
