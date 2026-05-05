using System.Collections.Generic;
using System.Diagnostics;

namespace System.Collections.ObjectModel;

/// <summary>
/// Represents a read-only dictionary with non-null unique values that provides access to an inverse read-only dictionary.
/// </summary>
/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
[DebuggerTypeProxy(typeof(DictionaryDebugView<,>))]
[DebuggerDisplay("Count = {Count}")]
public class ReadOnlyBidirectionalDictionary<TKey, TValue> : IBidirectionalDictionary<TKey, TValue>, IReadOnlyBidirectionalDictionary<TKey, TValue>
    where TKey : notnull
    where TValue : notnull
{
    private readonly IBidirectionalDictionary<TKey, TValue> _bidirectionalDictionary;
    private KeyCollection? _keys;
    private ValueCollection? _values;

    #region Properties

    /// <summary>
    /// Gets the inverse <see cref="ReadOnlyBidirectionalDictionary{TKey,TValue}"/>.
    /// </summary>
    public ReadOnlyBidirectionalDictionary<TValue, TKey> Inverse { get; }

    /// <summary>
    /// Gets the number of key/value pairs contained in the <see cref="ReadOnlyBidirectionalDictionary{TKey, TValue}"/>.
    /// </summary>
    public int Count => _bidirectionalDictionary.Count;

    /// <summary>
    /// Gets a collection containing the keys in the <see cref="ReadOnlyBidirectionalDictionary{TKey, TValue}"/>.
    /// </summary>
    public KeyCollection Keys => _keys ??= new KeyCollection(_bidirectionalDictionary.Keys);

    /// <summary>
    /// Gets a collection containing the values in the <see cref="ReadOnlyBidirectionalDictionary{TKey, TValue}"/>.
    /// </summary>
    public ValueCollection Values => _values ??= new ValueCollection(_bidirectionalDictionary.Values);

    /// <summary>
    /// Gets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get.</param>
    /// <returns>The value associated with the specified key. If the specified key is not found, a get operation throws a
    /// <see cref="KeyNotFoundException"/>.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    public TValue this[TKey key] => _bidirectionalDictionary[key];

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    IBidirectionalDictionary<TValue, TKey> IBidirectionalDictionary<TKey, TValue>.Inverse => Inverse;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    IReadOnlyBidirectionalDictionary<TValue, TKey> IReadOnlyBidirectionalDictionary<TKey, TValue>.Inverse => Inverse;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    TValue IDictionary<TKey, TValue>.this[TKey key]
    {
        get => this[key];
        set => throw new NotSupportedException();
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;
    
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;
    
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;
    
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => true;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ReadOnlyBidirectionalDictionary{TKey, TValue}"/> class
    /// that is a wrapper around the specified bidirectional dictionary.
    /// </summary>
    /// <param name="bidirectionalDictionary">The bidirectional dictionary to wrap.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ReadOnlyBidirectionalDictionary(IBidirectionalDictionary<TKey, TValue> bidirectionalDictionary)
    {
        _bidirectionalDictionary = bidirectionalDictionary ?? throw new ArgumentNullException(nameof(bidirectionalDictionary));
        Inverse = new ReadOnlyBidirectionalDictionary<TValue, TKey>(this);
    }

    private ReadOnlyBidirectionalDictionary(ReadOnlyBidirectionalDictionary<TValue, TKey> inverse)
    {
        _bidirectionalDictionary = inverse._bidirectionalDictionary.Inverse;
        Inverse = inverse;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Determines whether the <see cref="ReadOnlyBidirectionalDictionary{TKey, TValue}"/> contains the specified key.
    /// </summary>
    /// <param name="key">The key to locate in the <see cref="ReadOnlyBidirectionalDictionary{TKey, TValue}"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="ReadOnlyBidirectionalDictionary{TKey, TValue}"/> contains
    /// an element with the specified key; otherwise, <see langword="false"/>.</returns>
    public bool ContainsKey(TKey key) => _bidirectionalDictionary.ContainsKey(key);

    /// <summary>
    /// Determines whether the <see cref="ReadOnlyBidirectionalDictionary{TKey, TValue}"/> contains the specified value.
    /// </summary>
    /// <param name="value">The value to locate in the <see cref="ReadOnlyBidirectionalDictionary{TKey, TValue}"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="ReadOnlyBidirectionalDictionary{TKey, TValue}"/> contains
    /// an element with the specified value; otherwise, <see langword="false"/>.</returns>
    public bool ContainsValue(TValue value) => _bidirectionalDictionary.ContainsValue(value);

    /// <summary>
    /// Gets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get.</param>
    /// <param name="value">When this method returns, contains the value associated with the specified key,
    /// if the key is found; otherwise, the default value for the type of the value parameter.
    /// This parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the <see cref="ReadOnlyBidirectionalDictionary{TKey, TValue}"/> contains
    /// an element with the specified key; otherwise, <see langword="false"/>.</returns>
    public bool TryGetValue(TKey key, out TValue value) => _bidirectionalDictionary.TryGetValue(key, out value!);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _bidirectionalDictionary.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => throw new NotSupportedException();

    bool IDictionary<TKey, TValue>.Remove(TKey key) => throw new NotSupportedException();

    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();

    void ICollection<KeyValuePair<TKey, TValue>>.Clear() => throw new NotSupportedException();

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
    {
        return _bidirectionalDictionary.Contains(item);
    }

    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        _bidirectionalDictionary.CopyTo(array, arrayIndex);
    }

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
    {
        return _bidirectionalDictionary.GetEnumerator();
    }

    #endregion

    [DebuggerTypeProxy(typeof(DictionaryKeyCollectionDebugView<,>))]
    [DebuggerDisplay("Count = {Count}")]
    public sealed class KeyCollection : ICollection<TKey>, ICollection, IReadOnlyCollection<TKey>
    {
        private readonly ICollection<TKey> _collection;

        internal KeyCollection(ICollection<TKey> collection)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        public int Count => _collection.Count;

        bool ICollection<TKey>.IsReadOnly => true;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => _collection is ICollection collection ? collection.SyncRoot : this;

        public bool Contains(TKey item) => _collection.Contains(item);

        public void CopyTo(TKey[] array, int arrayIndex) => _collection.CopyTo(array, arrayIndex);

        public IEnumerator<TKey> GetEnumerator() => _collection.GetEnumerator();

        void ICollection<TKey>.Add(TKey item) => throw new NotSupportedException();

        void ICollection<TKey>.Clear() => throw new NotSupportedException();

        bool ICollection<TKey>.Remove(TKey item) => throw new NotSupportedException();

        void ICollection.CopyTo(Array array, int index) => ((ICollection)_collection).CopyTo(array, index);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_collection).GetEnumerator();
    }

    [DebuggerTypeProxy(typeof(DictionaryValueCollectionDebugView<,>))]
    [DebuggerDisplay("Count = {Count}")]
    public sealed class ValueCollection : ICollection<TValue>, ICollection, IReadOnlyCollection<TValue>
    {
        private readonly ICollection<TValue> _collection;

        internal ValueCollection(ICollection<TValue> collection)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        public int Count => _collection.Count;

        bool ICollection<TValue>.IsReadOnly => true;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => _collection is ICollection collection ? collection.SyncRoot : this;

        public bool Contains(TValue item) => _collection.Contains(item);

        public void CopyTo(TValue[] array, int arrayIndex) => _collection.CopyTo(array, arrayIndex);

        public IEnumerator<TValue> GetEnumerator() => _collection.GetEnumerator();

        void ICollection<TValue>.Add(TValue item) => throw new NotSupportedException();

        void ICollection<TValue>.Clear() => throw new NotSupportedException();

        bool ICollection<TValue>.Remove(TValue item) => throw new NotSupportedException();

        void ICollection.CopyTo(Array array, int index) => ((ICollection)_collection).CopyTo(array, index);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_collection).GetEnumerator();
    }
}
