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
public class ConcurrentBidirectionalDictionary<TKey, TValue> : IBidirectionalDictionary<TKey, TValue>, IReadOnlyBidirectionalDictionary<TKey, TValue>, IDictionary
    where TKey : notnull
    where TValue : notnull
{
    private const int DefaultCapacity = 31;

    private readonly ConcurrentBidirectionalDictionaryShard<TKey, TValue>[] _forwardShards;
    private readonly ConcurrentBidirectionalDictionaryShard<TValue, TKey>[] _reverseShards;

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

        int capacityPerShard = GetCapacityPerShard(capacity, concurrencyLevel);
        _forwardShards = CreateShards<TKey, TValue>(concurrencyLevel, capacityPerShard, keyComparer, orderOffset: 0);
        _reverseShards = CreateShards<TValue, TKey>(concurrencyLevel, capacityPerShard, valueComparer, orderOffset: concurrencyLevel);
        Inverse = new ConcurrentBidirectionalDictionary<TValue, TKey>(_reverseShards, _forwardShards, this);
    }

    private ConcurrentBidirectionalDictionary(
        ConcurrentBidirectionalDictionaryShard<TKey, TValue>[] forwardShards,
        ConcurrentBidirectionalDictionaryShard<TValue, TKey>[] reverseShards,
        ConcurrentBidirectionalDictionary<TValue, TKey> inverse)
    {
        _forwardShards = forwardShards;
        _reverseShards = reverseShards;
        Inverse = inverse;
    }

    /// <summary>Gets the inverse <see cref="ConcurrentBidirectionalDictionary{TKey,TValue}"/>.</summary>
    public ConcurrentBidirectionalDictionary<TValue, TKey> Inverse { get; }

    IBidirectionalDictionary<TValue, TKey> IBidirectionalDictionary<TKey, TValue>.Inverse => Inverse;

    IReadOnlyBidirectionalDictionary<TValue, TKey> IReadOnlyBidirectionalDictionary<TKey, TValue>.Inverse => Inverse;

    /// <summary>Gets the equality comparer that is used to determine equality of keys.</summary>
    public IEqualityComparer<TKey> KeyComparer => _forwardShards[0].Dictionary.Comparer;

    /// <summary>Gets the equality comparer that is used to determine equality of values.</summary>
    public IEqualityComparer<TValue> ValueComparer => _reverseShards[0].Dictionary.Comparer;

    /// <summary>Gets the number of key/value pairs contained in the dictionary.</summary>
    public int Count
    {
        get
        {
            EnterAllForwardShardLocks();
            try
            {
                int count = 0;
                foreach (ConcurrentBidirectionalDictionaryShard<TKey, TValue> shard in _forwardShards)
                {
                    checked
                    {
                        count += shard.Dictionary.Count;
                    }
                }

                return count;
            }
            finally
            {
                ExitAllForwardShardLocks();
            }
        }
    }

    /// <summary>Gets a value that indicates whether the dictionary is empty.</summary>
    public bool IsEmpty
    {
        get
        {
            EnterAllForwardShardLocks();
            try
            {
                foreach (ConcurrentBidirectionalDictionaryShard<TKey, TValue> shard in _forwardShards)
                {
                    if (shard.Dictionary.Count != 0)
                    {
                        return false;
                    }
                }

                return true;
            }
            finally
            {
                ExitAllForwardShardLocks();
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
            ConcurrentBidirectionalDictionaryShard<TKey, TValue> keyShard = GetForwardShard(key);
            Monitor.Enter(keyShard.Lock);
            try
            {
                return keyShard.Dictionary[key];
            }
            finally
            {
                Monitor.Exit(keyShard.Lock);
            }
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

        ConcurrentBidirectionalDictionaryShard<TKey, TValue> keyShard = GetForwardShard(key);
        ConcurrentBidirectionalDictionaryShard<TValue, TKey> valueShard = GetReverseShard(value);
        EnterShardLocks(keyShard, valueShard, out ConcurrentBidirectionalDictionaryShardBase first, out ConcurrentBidirectionalDictionaryShardBase? second);
        try
        {
            return TryAddNoLock(keyShard, valueShard, key, value);
        }
        finally
        {
            ExitShardLocks(first, second);
        }
    }

    /// <summary>Determines whether the dictionary contains the specified key.</summary>
    public bool ContainsKey(TKey key)
    {
        ThrowIfNull(key, nameof(key));

        ConcurrentBidirectionalDictionaryShard<TKey, TValue> keyShard = GetForwardShard(key);
        Monitor.Enter(keyShard.Lock);
        try
        {
            return keyShard.Dictionary.ContainsKey(key);
        }
        finally
        {
            Monitor.Exit(keyShard.Lock);
        }
    }

    /// <summary>Determines whether the dictionary contains the specified value.</summary>
    public bool ContainsValue(TValue value) => ContainsValueCore(value);

    /// <summary>Attempts to remove and return the value with the specified key from the dictionary.</summary>
    public bool TryRemove(TKey key, out TValue value)
    {
        ThrowIfNull(key, nameof(key));

        ConcurrentBidirectionalDictionaryShard<TKey, TValue> keyShard = GetForwardShard(key);
        while (true)
        {
            Monitor.Enter(keyShard.Lock);
            try
            {
                if (!keyShard.Dictionary.TryGetValue(key, out value!))
                {
                    value = default!;
                    return false;
                }
            }
            finally
            {
                Monitor.Exit(keyShard.Lock);
            }

            ConcurrentBidirectionalDictionaryShard<TValue, TKey> valueShard = GetReverseShard(value);
            EnterShardLocks(keyShard, valueShard, out ConcurrentBidirectionalDictionaryShardBase first, out ConcurrentBidirectionalDictionaryShardBase? second);
            try
            {
                if (!keyShard.Dictionary.TryGetValue(key, out TValue? current))
                {
                    value = default!;
                    return false;
                }

                if (!ValueComparer.Equals(current, value))
                {
                    continue;
                }

                keyShard.Dictionary.Remove(key);
                valueShard.Dictionary.Remove(value);
                return true;
            }
            finally
            {
                ExitShardLocks(first, second);
            }
        }
    }

    /// <summary>Removes a key and value from the dictionary.</summary>
    public bool TryRemove(KeyValuePair<TKey, TValue> item)
    {
        ThrowIfNull(item.Key, nameof(item));
        ThrowIfNull(item.Value, nameof(item));

        ConcurrentBidirectionalDictionaryShard<TKey, TValue> keyShard = GetForwardShard(item.Key);
        ConcurrentBidirectionalDictionaryShard<TValue, TKey> valueShard = GetReverseShard(item.Value);
        EnterShardLocks(keyShard, valueShard, out ConcurrentBidirectionalDictionaryShardBase first, out ConcurrentBidirectionalDictionaryShardBase? second);
        try
        {
            if (!keyShard.Dictionary.TryGetValue(item.Key, out TValue? existing) || !ValueComparer.Equals(existing, item.Value))
            {
                return false;
            }

            keyShard.Dictionary.Remove(item.Key);
            valueShard.Dictionary.Remove(item.Value);
            return true;
        }
        finally
        {
            ExitShardLocks(first, second);
        }
    }

    /// <summary>Attempts to get the value associated with the specified key.</summary>
    public bool TryGetValue(TKey key, out TValue value)
    {
        ThrowIfNull(key, nameof(key));

        ConcurrentBidirectionalDictionaryShard<TKey, TValue> keyShard = GetForwardShard(key);
        Monitor.Enter(keyShard.Lock);
        try
        {
            return keyShard.Dictionary.TryGetValue(key, out value!);
        }
        finally
        {
            Monitor.Exit(keyShard.Lock);
        }
    }

    /// <summary>Updates the value associated with a key if the existing value is equal to the comparison value.</summary>
    public bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue)
    {
        ThrowIfNull(key, nameof(key));
        ThrowIfNull(newValue, nameof(newValue));
        ThrowIfNull(comparisonValue, nameof(comparisonValue));

        return TryUpdateCore(key, newValue, comparisonValue, throwOnDuplicateValue: false, out _);
    }

    /// <summary>Removes all keys and values from the dictionary.</summary>
    public void Clear()
    {
        EnterAllShardLocks();
        try
        {
            foreach (ConcurrentBidirectionalDictionaryShard<TKey, TValue> shard in _forwardShards)
            {
                shard.Dictionary.Clear();
            }

            foreach (ConcurrentBidirectionalDictionaryShard<TValue, TKey> shard in _reverseShards)
            {
                shard.Dictionary.Clear();
            }
        }
        finally
        {
            ExitAllShardLocks();
        }
    }

    /// <summary>Copies the key and value pairs stored in the dictionary to a new array.</summary>
    public KeyValuePair<TKey, TValue>[] ToArray()
    {
        EnterAllForwardShardLocks();
        try
        {
            int count = 0;
            foreach (ConcurrentBidirectionalDictionaryShard<TKey, TValue> shard in _forwardShards)
            {
                checked
                {
                    count += shard.Dictionary.Count;
                }
            }

            if (count == 0)
            {
                return Array.Empty<KeyValuePair<TKey, TValue>>();
            }

            var array = new KeyValuePair<TKey, TValue>[count];
            int index = 0;
            foreach (ConcurrentBidirectionalDictionaryShard<TKey, TValue> shard in _forwardShards)
            {
                foreach (KeyValuePair<TKey, TValue> pair in shard.Dictionary)
                {
                    array[index] = pair;
                    index++;
                }
            }

            return array;
        }
        finally
        {
            ExitAllForwardShardLocks();
        }
    }

    /// <summary>Adds a key/value pair to the dictionary if the key does not already exist.</summary>
    public TValue GetOrAdd(TKey key, TValue value)
    {
        ThrowIfNull(key, nameof(key));
        ThrowIfNull(value, nameof(value));

        ConcurrentBidirectionalDictionaryShard<TKey, TValue> keyShard = GetForwardShard(key);
        ConcurrentBidirectionalDictionaryShard<TValue, TKey> valueShard = GetReverseShard(value);
        EnterShardLocks(keyShard, valueShard, out ConcurrentBidirectionalDictionaryShardBase first, out ConcurrentBidirectionalDictionaryShardBase? second);
        try
        {
            if (keyShard.Dictionary.TryGetValue(key, out TValue? existing))
            {
                return existing;
            }

            if (!TryAddNoLock(keyShard, valueShard, key, value))
            {
                throw new ArgumentException("The value already exists.", nameof(value));
            }

            return value;
        }
        finally
        {
            ExitShardLocks(first, second);
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

        ConcurrentBidirectionalDictionaryShard<TKey, TValue> keyShard = GetForwardShard(item.Key);
        Monitor.Enter(keyShard.Lock);
        try
        {
            return keyShard.Dictionary.TryGetValue(item.Key, out TValue? value) && ValueComparer.Equals(value, item.Value);
        }
        finally
        {
            Monitor.Exit(keyShard.Lock);
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
            if (TryGetExisting(key, out TValue? oldValue))
            {
                TValue newValue = updateValueFactory(key, oldValue);
                ThrowIfNull(newValue, nameof(updateValueFactory));

                if (TryUpdateCore(key, newValue, oldValue, throwOnDuplicateValue: true, out _))
                {
                    return newValue;
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
        ConcurrentBidirectionalDictionaryShard<TKey, TValue> keyShard = GetForwardShard(key);
        Monitor.Enter(keyShard.Lock);
        try
        {
            return keyShard.Dictionary.TryGetValue(key, out value!);
        }
        finally
        {
            Monitor.Exit(keyShard.Lock);
        }
    }

    private bool ContainsValueCore(TValue value)
    {
        ThrowIfNull(value, nameof(value));

        ConcurrentBidirectionalDictionaryShard<TValue, TKey> valueShard = GetReverseShard(value);
        Monitor.Enter(valueShard.Lock);
        try
        {
            return valueShard.Dictionary.ContainsKey(value);
        }
        finally
        {
            Monitor.Exit(valueShard.Lock);
        }
    }

    private bool TryAddNoLock(
        ConcurrentBidirectionalDictionaryShard<TKey, TValue> keyShard,
        ConcurrentBidirectionalDictionaryShard<TValue, TKey> valueShard,
        TKey key,
        TValue value)
    {
        if (keyShard.Dictionary.ContainsKey(key) || valueShard.Dictionary.ContainsKey(value))
        {
            return false;
        }

        keyShard.Dictionary.Add(key, value);
        valueShard.Dictionary.Add(value, key);
        return true;
    }

    private void SetItem(TKey key, TValue value)
    {
        ConcurrentBidirectionalDictionaryShard<TKey, TValue> keyShard = GetForwardShard(key);

        while (true)
        {
            Monitor.Enter(keyShard.Lock);
            try
            {
                if (!keyShard.Dictionary.TryGetValue(key, out TValue? oldValue))
                {
                    break;
                }

                if (ValueComparer.Equals(oldValue, value))
                {
                    return;
                }

                ConcurrentBidirectionalDictionaryShard<TValue, TKey> oldValueShard = GetReverseShard(oldValue);
                ConcurrentBidirectionalDictionaryShard<TValue, TKey> newValueShard = GetReverseShard(value);

                Monitor.Exit(keyShard.Lock);
                EnterShardLocks(keyShard, oldValueShard, newValueShard, out ConcurrentBidirectionalDictionaryShardBase first, out ConcurrentBidirectionalDictionaryShardBase? second, out ConcurrentBidirectionalDictionaryShardBase? third);
                try
                {
                    if (!keyShard.Dictionary.TryGetValue(key, out TValue? current))
                    {
                        continue;
                    }

                    if (!ValueComparer.Equals(current, oldValue))
                    {
                        continue;
                    }

                    if (newValueShard.Dictionary.TryGetValue(value, out TKey? owner) && !KeyComparer.Equals(owner, key))
                    {
                        throw new ArgumentException("The value already exists.", nameof(value));
                    }

                    keyShard.Dictionary[key] = value;
                    oldValueShard.Dictionary.Remove(oldValue);
                    newValueShard.Dictionary.Add(value, key);
                    return;
                }
                finally
                {
                    ExitShardLocks(first, second, third);
                    Monitor.Enter(keyShard.Lock);
                }
            }
            finally
            {
                Monitor.Exit(keyShard.Lock);
            }
        }

        ConcurrentBidirectionalDictionaryShard<TValue, TKey> valueShard = GetReverseShard(value);
        EnterShardLocks(keyShard, valueShard, out ConcurrentBidirectionalDictionaryShardBase addFirst, out ConcurrentBidirectionalDictionaryShardBase? addSecond);
        try
        {
            if (!TryAddNoLock(keyShard, valueShard, key, value))
            {
                throw new ArgumentException("The value already exists.", nameof(value));
            }
        }
        finally
        {
            ExitShardLocks(addFirst, addSecond);
        }
    }

    private bool TryUpdateCore(TKey key, TValue newValue, TValue comparisonValue, bool throwOnDuplicateValue, out bool retry)
    {
        retry = false;

        ConcurrentBidirectionalDictionaryShard<TKey, TValue> keyShard = GetForwardShard(key);
        ConcurrentBidirectionalDictionaryShard<TValue, TKey> comparisonValueShard = GetReverseShard(comparisonValue);
        ConcurrentBidirectionalDictionaryShard<TValue, TKey> newValueShard = GetReverseShard(newValue);

        EnterShardLocks(keyShard, comparisonValueShard, newValueShard, out ConcurrentBidirectionalDictionaryShardBase first, out ConcurrentBidirectionalDictionaryShardBase? second, out ConcurrentBidirectionalDictionaryShardBase? third);
        try
        {
            if (!keyShard.Dictionary.TryGetValue(key, out TValue? current))
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

            ConcurrentBidirectionalDictionaryShard<TValue, TKey> currentValueShard = GetReverseShard(current);
            if (!ReferenceEquals(currentValueShard, comparisonValueShard))
            {
                retry = true;
                return false;
            }

            if (newValueShard.Dictionary.TryGetValue(newValue, out TKey? owner) && !KeyComparer.Equals(owner, key))
            {
                if (throwOnDuplicateValue)
                {
                    throw new ArgumentException("The value already exists.", nameof(newValue));
                }

                return false;
            }

            keyShard.Dictionary[key] = newValue;
            currentValueShard.Dictionary.Remove(current);
            newValueShard.Dictionary.Add(newValue, key);
            return true;
        }
        finally
        {
            ExitShardLocks(first, second, third);
        }
    }

    private ReadOnlyCollection<TKey> GetKeys()
    {
        EnterAllForwardShardLocks();
        try
        {
            var keys = new TKey[CountNoLocks()];
            int index = 0;
            foreach (ConcurrentBidirectionalDictionaryShard<TKey, TValue> shard in _forwardShards)
            {
                foreach (TKey key in shard.Dictionary.Keys)
                {
                    keys[index] = key;
                    index++;
                }
            }

            return new ReadOnlyCollection<TKey>(keys);
        }
        finally
        {
            ExitAllForwardShardLocks();
        }
    }

    private ReadOnlyCollection<TValue> GetValues()
    {
        EnterAllForwardShardLocks();
        try
        {
            var values = new TValue[CountNoLocks()];
            int index = 0;
            foreach (ConcurrentBidirectionalDictionaryShard<TKey, TValue> shard in _forwardShards)
            {
                foreach (TValue value in shard.Dictionary.Values)
                {
                    values[index] = value;
                    index++;
                }
            }

            return new ReadOnlyCollection<TValue>(values);
        }
        finally
        {
            ExitAllForwardShardLocks();
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

    private int CountNoLocks()
    {
        int count = 0;
        foreach (ConcurrentBidirectionalDictionaryShard<TKey, TValue> shard in _forwardShards)
        {
            checked
            {
                count += shard.Dictionary.Count;
            }
        }

        return count;
    }

    private ConcurrentBidirectionalDictionaryShard<TKey, TValue> GetForwardShard(TKey key) =>
        _forwardShards[GetShardIndex(key, KeyComparer, _forwardShards.Length)];

    private ConcurrentBidirectionalDictionaryShard<TValue, TKey> GetReverseShard(TValue value) =>
        _reverseShards[GetShardIndex(value, ValueComparer, _reverseShards.Length)];

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

    private static int GetCapacityPerShard(int capacity, int shardCount)
    {
        if (capacity == 0)
        {
            return 0;
        }

        return Math.Max(1, (int)(((long)capacity + shardCount - 1) / shardCount));
    }

    private static ConcurrentBidirectionalDictionaryShard<TShardKey, TShardValue>[] CreateShards<TShardKey, TShardValue>(
        int count,
        int capacityPerShard,
        IEqualityComparer<TShardKey>? comparer,
        int orderOffset)
        where TShardKey : notnull
        where TShardValue : notnull
    {
        var shards = new ConcurrentBidirectionalDictionaryShard<TShardKey, TShardValue>[count];
        for (int i = 0; i < shards.Length; i++)
        {
            shards[i] = new ConcurrentBidirectionalDictionaryShard<TShardKey, TShardValue>(orderOffset + i, capacityPerShard, comparer);
        }

        return shards;
    }

    private static int GetShardIndex<TShardKey>(TShardKey key, IEqualityComparer<TShardKey> comparer, int shardCount)
        where TShardKey : notnull
    {
        return (int)((uint)comparer.GetHashCode(key) % (uint)shardCount);
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

    private void EnterAllForwardShardLocks()
    {
        foreach (ConcurrentBidirectionalDictionaryShard<TKey, TValue> shard in _forwardShards)
        {
            Monitor.Enter(shard.Lock);
        }
    }

    private void ExitAllForwardShardLocks()
    {
        for (int i = _forwardShards.Length - 1; i >= 0; i--)
        {
            Monitor.Exit(_forwardShards[i].Lock);
        }
    }

    private void EnterAllShardLocks()
    {
        int forwardIndex = 0;
        int reverseIndex = 0;

        while (forwardIndex < _forwardShards.Length || reverseIndex < _reverseShards.Length)
        {
            if (reverseIndex >= _reverseShards.Length ||
                (forwardIndex < _forwardShards.Length && _forwardShards[forwardIndex].LockOrder < _reverseShards[reverseIndex].LockOrder))
            {
                Monitor.Enter(_forwardShards[forwardIndex].Lock);
                forwardIndex++;
                continue;
            }

            Monitor.Enter(_reverseShards[reverseIndex].Lock);
            reverseIndex++;
        }
    }

    private void ExitAllShardLocks()
    {
        int forwardIndex = _forwardShards.Length - 1;
        int reverseIndex = _reverseShards.Length - 1;

        while (forwardIndex >= 0 || reverseIndex >= 0)
        {
            if (reverseIndex < 0 ||
                (forwardIndex >= 0 && _forwardShards[forwardIndex].LockOrder > _reverseShards[reverseIndex].LockOrder))
            {
                Monitor.Exit(_forwardShards[forwardIndex].Lock);
                forwardIndex--;
                continue;
            }

            Monitor.Exit(_reverseShards[reverseIndex].Lock);
            reverseIndex--;
        }
    }

    private static void EnterShardLocks(ConcurrentBidirectionalDictionaryShardBase a, ConcurrentBidirectionalDictionaryShardBase b, out ConcurrentBidirectionalDictionaryShardBase first, out ConcurrentBidirectionalDictionaryShardBase? second)
    {
        if (ReferenceEquals(a, b))
        {
            first = a;
            second = null;
            Monitor.Enter(first.Lock);
            return;
        }

        if (a.LockOrder < b.LockOrder)
        {
            first = a;
            second = b;
        }
        else
        {
            first = b;
            second = a;
        }

        Monitor.Enter(first.Lock);
        Monitor.Enter(second.Lock);
    }

    private static void ExitShardLocks(ConcurrentBidirectionalDictionaryShardBase first, ConcurrentBidirectionalDictionaryShardBase? second)
    {
        if (second is not null)
        {
            Monitor.Exit(second.Lock);
        }

        Monitor.Exit(first.Lock);
    }

    private static void EnterShardLocks(ConcurrentBidirectionalDictionaryShardBase a, ConcurrentBidirectionalDictionaryShardBase b, ConcurrentBidirectionalDictionaryShardBase c, out ConcurrentBidirectionalDictionaryShardBase first, out ConcurrentBidirectionalDictionaryShardBase? second, out ConcurrentBidirectionalDictionaryShardBase? third)
    {
        if (ReferenceEquals(a, b))
        {
            EnterShardLocks(a, c, out first, out second);
            third = null;
            return;
        }

        if (ReferenceEquals(a, c) || ReferenceEquals(b, c))
        {
            EnterShardLocks(a, b, out first, out second);
            third = null;
            return;
        }

        first = a;
        second = b;
        third = c;

        if (first.LockOrder > second.LockOrder)
        {
            (first, second) = (second, first);
        }

        if (second.LockOrder > third.LockOrder)
        {
            (second, third) = (third, second);
        }

        if (first.LockOrder > second.LockOrder)
        {
            (first, second) = (second, first);
        }

        Monitor.Enter(first.Lock);
        Monitor.Enter(second.Lock);
        Monitor.Enter(third.Lock);
    }

    private static void ExitShardLocks(ConcurrentBidirectionalDictionaryShardBase first, ConcurrentBidirectionalDictionaryShardBase? second, ConcurrentBidirectionalDictionaryShardBase? third)
    {
        if (third is not null)
        {
            Monitor.Exit(third.Lock);
        }

        if (second is not null)
        {
            Monitor.Exit(second.Lock);
        }

        Monitor.Exit(first.Lock);
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
            int next = _index + 1;
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

internal abstract class ConcurrentBidirectionalDictionaryShardBase
{
    protected ConcurrentBidirectionalDictionaryShardBase(int lockOrder)
    {
        LockOrder = lockOrder;
    }

    internal object Lock { get; } = new();

    internal int LockOrder { get; }
}

internal sealed class ConcurrentBidirectionalDictionaryShard<TKey, TValue> : ConcurrentBidirectionalDictionaryShardBase
    where TKey : notnull
    where TValue : notnull
{
    internal ConcurrentBidirectionalDictionaryShard(int lockOrder, int capacity, IEqualityComparer<TKey>? comparer)
        : base(lockOrder)
    {
        Dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
    }

    internal Dictionary<TKey, TValue> Dictionary { get; }
}


