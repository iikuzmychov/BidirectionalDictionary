using System.Diagnostics;
using System.Linq;

namespace System.Collections.Generic
{
    [DebuggerDisplay("Count = {Count}")]
    public class BidirectionalDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
        where TKey : notnull
        where TValue : notnull
    {
        protected readonly Dictionary<TKey, TValue> _baseDictionary;

        #region Properties

        public BidirectionalDictionary<TValue, TKey> Inverse { get; }
        public IEqualityComparer<TKey> KeyComparer => _baseDictionary.Comparer;
        public IEqualityComparer<TValue> ValueComparer { get; }
        public int Count => _baseDictionary.Count;
        public ICollection<TKey> Keys => _baseDictionary.Keys;
        public ICollection<TValue> Values => _baseDictionary.Values;
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => _baseDictionary.Keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => _baseDictionary.Values;
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;
        public TValue this[TKey key]
        {
            get => _baseDictionary[key];
            set
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));

                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                if (TryGetValue(key, out var oldValue))
                {
                    if (ValueComparer.Equals(oldValue, value))
                        return;

                    if (ContainsValue(value))
                    {
                        throw new ArgumentException("The value already exists.", nameof(value));
                    }
                    else
                    {
                        _baseDictionary[key] = value;

                        Inverse._baseDictionary.Remove(oldValue);
                        Inverse._baseDictionary.Add(value, key);
                    }
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        #endregion

        #region Constructors

        public BidirectionalDictionary() : this(new Dictionary<TKey, TValue>()) { }

        public BidirectionalDictionary(int capacity) : this(capacity, null, null) { }

        public BidirectionalDictionary(IDictionary<TKey, TValue> dictionary)
            : this(new Dictionary<TKey, TValue>(dictionary)) { }

        public BidirectionalDictionary(IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer)
            : this(new Dictionary<TKey, TValue>(keyComparer), valueComparer) { }

        public BidirectionalDictionary(int capacity, IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer)
            : this(new Dictionary<TKey, TValue>(capacity, keyComparer), valueComparer) { }

        public BidirectionalDictionary(IDictionary<TKey, TValue> dictionary,
            IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer)
            : this(new Dictionary<TKey, TValue>(dictionary, keyComparer), valueComparer) { }

        private BidirectionalDictionary(BidirectionalDictionary<TValue, TKey> inverse)
        {
            _baseDictionary = inverse._baseDictionary.ToDictionary(pair => pair.Value, pair => pair.Key, inverse.ValueComparer);
            ValueComparer   = inverse.KeyComparer;
            Inverse         = inverse;
        }

        private BidirectionalDictionary(Dictionary<TKey, TValue> dictionary, IEqualityComparer<TValue>? valueComparer = null)
        {
            _baseDictionary = dictionary;
            ValueComparer   = valueComparer ?? EqualityComparer<TValue>.Default;
            Inverse         = new BidirectionalDictionary<TValue, TKey>(this);
        }

        #endregion

        #region Methods

        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (ContainsKey(key))
                throw new ArgumentException("The same key already exists.", nameof(key));

            if (ContainsValue(value))
                throw new ArgumentException("The same value already exists.", nameof(value));

            _baseDictionary.Add(key, value);
            Inverse._baseDictionary.Add(value, key);
        }

        public bool Remove(TKey key) => Remove(key, out _);

        public bool Remove(TKey key, out TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

#if NETSTANDARD2_1_OR_GREATER
            return _baseDictionary.Remove(key, out value) &&
                Inverse._baseDictionary.Remove(value);
#elif NETSTANDARD2_0
            return _baseDictionary.TryGetValue(key, out value) &&
                _baseDictionary.Remove(key) &&
                Inverse._baseDictionary.Remove(value);
#endif
        }

        public void Clear()
        {
            _baseDictionary.Clear();
            Inverse._baseDictionary.Clear();
        }

        public bool ContainsKey(TKey key) => _baseDictionary.ContainsKey(key);

        public bool ContainsValue(TValue value) => Inverse.ContainsKey(value);

        public bool TryAdd(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (ContainsKey(key) || ContainsValue(value))
                return false;

            _baseDictionary.Add(key, value);
            Inverse._baseDictionary.Add(value, key);

            return true;
        }

        public bool TryGetValue(TKey key, out TValue value) => _baseDictionary.TryGetValue(key, out value);

        /*public void EnsureCapacity(int capacity)
        {
            _baseDictionary.EnsureCapacity(capacity);
            Inverse._baseDictionary.EnsureCapacity(capacity);
        }

        public void TrimExcess()
        {
            _baseDictionary.TrimExcess();
            Inverse._baseDictionary.TrimExcess();
        }

        public void TrimExcess(int capacity)
        {
            _baseDictionary.TrimExcess(capacity);
            Inverse._baseDictionary.TrimExcess(capacity);
        }*/
        
        public IEnumerator GetEnumerator() => _baseDictionary.GetEnumerator();

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            if (item.Key == null)
                throw new ArgumentNullException("The item key == null.", nameof(item));

            if (item.Value == null)
                throw new ArgumentNullException("The item value == null.", nameof(item));

            return ((ICollection<KeyValuePair<TKey, TValue>>)_baseDictionary).Remove(item) &&
                Inverse._baseDictionary.Remove(item.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            if (item.Key == null)
                throw new ArgumentNullException("The item key == null.", nameof(item));

            if (item.Value == null)
                throw new ArgumentNullException("The item value == null.", nameof(item));

            return ((ICollection<KeyValuePair<TKey, TValue>>)_baseDictionary).Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) =>
            ((ICollection<KeyValuePair<TKey, TValue>>)_baseDictionary).CopyTo(array, arrayIndex);

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
            => _baseDictionary.GetEnumerator();

        #endregion
    }
}
