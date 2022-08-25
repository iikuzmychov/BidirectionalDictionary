﻿using System.Diagnostics;

namespace System.Collections.Generic
{
    [DebuggerDisplay("Count = {Count}")]
    public class ReadOnlyBidirectionalDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
        where TKey : notnull
        where TValue : notnull
    {
        private readonly BidirectionalDictionary<TKey, TValue> _baseDictionary;

        #region Properties

        /// <summary>
        /// Gets the inverse <see cref="ReadOnlyBidirectionalDictionary{TKey,TValue}"/>.
        /// </summary>
        public ReadOnlyBidirectionalDictionary<TValue, TKey> Inverse { get; }

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to determine equality of keys for the dictionary.
        /// </summary>
        public IEqualityComparer<TKey> KeyComparer => _baseDictionary.KeyComparer;

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to determine equality of values for the dictionary.
        /// </summary>
        public IEqualityComparer<TValue> ValueComparer => _baseDictionary.ValueComparer;

        /// <summary>
        /// Gets the number of key/value pairs contained in the <see cref="ReadOnlyBidirectionalDictionary{TKey, TValue}"/>.
        /// </summary>
        public int Count => _baseDictionary.Count;

        /// <summary>
        /// Gets a collection containing the keys in the <see cref="ReadOnlyBidirectionalDictionary{TKey, TValue}"/>.
        /// </summary>
        public Dictionary<TKey, TValue>.KeyCollection Keys => _baseDictionary.Keys;

        /// <summary>
        /// Gets a collection containing the values in the <see cref="ReadOnlyBidirectionalDictionary{TKey, TValue}"/>.
        /// </summary>
        public Dictionary<TKey, TValue>.ValueCollection Values => _baseDictionary.Values;

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key. If the specified key is not found, a get operation throws a
        /// <see cref="KeyNotFoundException"/>. A set operation is always throws <see cref="NotSupportedException"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public TValue this[TKey key]
        {
            get => _baseDictionary[key];
            set => throw new NotSupportedException();
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;
        ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        #endregion

        #region Constructors

        public ReadOnlyBidirectionalDictionary(BidirectionalDictionary<TKey, TValue> dictionary)
        {
            _baseDictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
            Inverse         = new ReadOnlyBidirectionalDictionary<TValue, TKey>(dictionary.Inverse);
        }

        #endregion

        #region Methods

        public void Add(TKey key, TValue value) => throw new NotSupportedException();

        public bool Remove(TKey key) => throw new NotSupportedException();

        public void Clear() => throw new NotSupportedException();

        /// <summary>
        /// Determines whether the <see cref="ReadOnlyBidirectionalDictionary{TKey, TValue}"/> contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="ReadOnlyBidirectionalDictionary{TKey, TValue}"/>.</param>
        /// <returns><see langword="true"/> if the <see cref="ReadOnlyBidirectionalDictionary{TKey, TValue}"/> contains
        /// an element with the specified key; otherwise, <see langword="false"/>.</returns>
        public bool ContainsKey(TKey key) => _baseDictionary.ContainsKey(key);

        /// <summary>
        /// Determines whether the <see cref="ReadOnlyBidirectionalDictionary{TKey, TValue}"/> contains the specified value.
        /// </summary>
        /// <param name="value">The value to locate in the <see cref="ReadOnlyBidirectionalDictionary{TKey, TValue}"/>.</param>
        /// <returns><see langword="true"/> if the <see cref="ReadOnlyBidirectionalDictionary{TKey, TValue}"/> contains
        /// an element with the specified value; otherwise, <see langword="false"/>.</returns>
        public bool ContainsValue(TValue value) => _baseDictionary.ContainsValue(value);

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key,
        /// if the key is found; otherwise, the default value for the type of the value parameter.
        /// This parameter is passed uninitialized.</param>
        /// <returns><see langword="true"/> if the <see cref="ReadOnlyBidirectionalDictionary{TKey, TValue}"/> contains
        /// an element with the specified key; otherwise, <see langword="false"/>.</returns>
        public bool TryGetValue(TKey key, out TValue value) => _baseDictionary.TryGetValue(key, out value);

        public IEnumerator GetEnumerator() => _baseDictionary.GetEnumerator();

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) =>
            ((ICollection<KeyValuePair<TKey, TValue>>)_baseDictionary).Contains(item);

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) =>
            ((ICollection<KeyValuePair<TKey, TValue>>)_baseDictionary).CopyTo(array, arrayIndex);

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
            => ((IEnumerable<KeyValuePair<TKey, TValue>>)_baseDictionary).GetEnumerator();

        #endregion
    }
}