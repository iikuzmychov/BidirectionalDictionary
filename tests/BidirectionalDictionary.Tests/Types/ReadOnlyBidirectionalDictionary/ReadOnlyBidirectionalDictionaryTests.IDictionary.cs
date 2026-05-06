using System.Collections;
using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests.Types.ReadOnlyBidirectionalDictionary;

public partial class ReadOnlyBidirectionalDictionaryTests
{
    [Fact]
    public void IDictionary_Properties_FilledReadOnlyBidirectionalDictionary_ReturnsExpectedValues()
    {
        var readOnlyBidirectionalDictionary = CreateReadOnlyBidirectionalDictionaryForNonGenericDictionary();
        var dictionary = (IDictionary)readOnlyBidirectionalDictionary;

        Assert.True(dictionary.IsFixedSize);
        Assert.True(dictionary.IsReadOnly);
        Assert.False(((ICollection)dictionary).IsSynchronized);
        Assert.NotNull(((ICollection)dictionary).SyncRoot);
        Assert.Same(((ICollection)dictionary).SyncRoot, ((ICollection)dictionary).SyncRoot);
        Assert.Equal(['a'], dictionary.Keys.Cast<char>());
        Assert.Equal([0], dictionary.Values.Cast<int>());
    }

    [Fact]
    public void IDictionary_IndexerGet_FilledReadOnlyBidirectionalDictionary_ReturnsExpectedValue()
    {
        var dictionary = (IDictionary)CreateReadOnlyBidirectionalDictionaryForNonGenericDictionary();

        Assert.Equal(0, dictionary['a']);
        Assert.Null(dictionary['b']);
        Assert.Null(dictionary["a"]);
        Assert.Throws<ArgumentNullException>(() => dictionary[null!]);
    }

    [Fact]
    public void IDictionary_Contains_FilledReadOnlyBidirectionalDictionary_ReturnsExpectedResult()
    {
        var dictionary = (IDictionary)CreateReadOnlyBidirectionalDictionaryForNonGenericDictionary();

        Assert.True(dictionary.Contains('a'));
        Assert.False(dictionary.Contains('b'));
        Assert.False(dictionary.Contains("a"));
        Assert.Throws<ArgumentNullException>(() => dictionary.Contains(null!));
    }

    [Fact]
    public void IDictionary_Mutations_FilledReadOnlyBidirectionalDictionary_ThrowNotSupportedException()
    {
        var dictionary = (IDictionary)CreateReadOnlyBidirectionalDictionaryForNonGenericDictionary();

        Assert.Throws<NotSupportedException>(() => dictionary.Add('b', 1));
        Assert.Throws<NotSupportedException>(() => dictionary['a'] = 1);
        Assert.Throws<NotSupportedException>(() => dictionary.Remove('a'));
        Assert.Throws<NotSupportedException>(() => dictionary.Clear());
    }

    [Fact]
    public void IDictionary_GetEnumerator_FilledReadOnlyBidirectionalDictionary_EnumeratesDictionaryEntries()
    {
        var dictionary = (IDictionary)CreateReadOnlyBidirectionalDictionaryForNonGenericDictionary();

        var enumerator = dictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new DictionaryEntry('a', 0), enumerator.Entry);
        Assert.Equal(new DictionaryEntry('a', 0), enumerator.Current);
    }

    [Fact]
    public void IDictionary_GetEnumerator_GenericOnlyBidirectionalDictionary_EnumeratesDictionaryEntries()
    {
        var dictionary = (IDictionary)new ReadOnlyBidirectionalDictionary<char, int>(
            new GenericOnlyBidirectionalDictionary<char, int>([new KeyValuePair<char, int>('a', 0)]));

        var enumerator = dictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new DictionaryEntry('a', 0), enumerator.Entry);
        Assert.Equal('a', enumerator.Key);
        Assert.Equal(0, enumerator.Value);
        Assert.Equal(new DictionaryEntry('a', 0), enumerator.Current);

        enumerator.Reset();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new DictionaryEntry('a', 0), enumerator.Entry);
    }

    [Fact]
    public void ICollection_SyncRoot_GenericOnlyBidirectionalDictionary_ReturnsReadOnlyDictionary()
    {
        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(
            new GenericOnlyBidirectionalDictionary<char, int>([new KeyValuePair<char, int>('a', 0)]));

        var syncRoot = ((ICollection)readOnlyBidirectionalDictionary).SyncRoot;

        Assert.Same(readOnlyBidirectionalDictionary, syncRoot);
    }

    [Fact]
    public void IEnumerableT_GetEnumerator_FilledReadOnlyBidirectionalDictionary_EnumeratesPairs()
    {
        var readOnlyBidirectionalDictionary = CreateReadOnlyBidirectionalDictionaryForNonGenericDictionary();

        var entries = ((IEnumerable<KeyValuePair<char, int>>)readOnlyBidirectionalDictionary).ToArray();

        Assert.Equal([new KeyValuePair<char, int>('a', 0)], entries);
    }

    [Fact]
    public void ICollection_CopyTo_FilledReadOnlyBidirectionalDictionaryAndDictionaryEntryArray_CopiesEntries()
    {
        var dictionary = (ICollection)CreateReadOnlyBidirectionalDictionaryForNonGenericDictionary();
        var entries = new DictionaryEntry[2];

        dictionary.CopyTo(entries, 1);

        Assert.Equal(default, entries[0]);
        Assert.Equal(new DictionaryEntry('a', 0), entries[1]);
    }

    [Fact]
    public void ICollection_CopyTo_GenericOnlyReadOnlyBidirectionalDictionaryAndDictionaryEntryArray_CopiesEntries()
    {
        var dictionary = (ICollection)CreateReadOnlyBidirectionalDictionaryForGenericOnlyDictionary();
        var entries = new DictionaryEntry[2];

        dictionary.CopyTo(entries, 1);

        Assert.Equal(default, entries[0]);
        Assert.Equal(new DictionaryEntry('a', 0), entries[1]);
    }

    [Fact]
    public void ICollection_CopyTo_FilledReadOnlyBidirectionalDictionaryAndObjectArray_CopiesPairs()
    {
        var dictionary = (ICollection)CreateReadOnlyBidirectionalDictionaryForGenericOnlyDictionary();
        var entries = new object[2];

        dictionary.CopyTo(entries, 1);

        Assert.Null(entries[0]);
        Assert.Equal(new KeyValuePair<char, int>('a', 0), entries[1]);
    }

    [Fact]
    public void ICollection_CopyTo_FilledReadOnlyBidirectionalDictionaryAndKeyValuePairArray_CopiesPairs()
    {
        var dictionary = (ICollection)CreateReadOnlyBidirectionalDictionaryForGenericOnlyDictionary();
        var entries = new KeyValuePair<char, int>[2];

        dictionary.CopyTo(entries, 1);

        Assert.Equal(default, entries[0]);
        Assert.Equal(new KeyValuePair<char, int>('a', 0), entries[1]);
    }

    [Fact]
    public void ICollection_CopyTo_FilledReadOnlyBidirectionalDictionaryAndNullArray_ThrowsArgumentNullException()
    {
        var dictionary = (ICollection)CreateReadOnlyBidirectionalDictionaryForGenericOnlyDictionary();

        Assert.Throws<ArgumentNullException>(() => dictionary.CopyTo(null!, 0));
    }

    [Fact]
    public void ICollection_CopyTo_FilledReadOnlyBidirectionalDictionaryAndMultidimensionalArray_ThrowsArgumentException()
    {
        var dictionary = (ICollection)CreateReadOnlyBidirectionalDictionaryForGenericOnlyDictionary();

        Assert.Throws<ArgumentException>(() => dictionary.CopyTo(new DictionaryEntry[1, 1], 0));
    }

    [Fact]
    public void ICollection_CopyTo_FilledReadOnlyBidirectionalDictionaryAndNonZeroLowerBoundArray_ThrowsArgumentException()
    {
        var dictionary = (ICollection)CreateReadOnlyBidirectionalDictionaryForGenericOnlyDictionary();
        var entries = Array.CreateInstance(typeof(DictionaryEntry), [1], [1]);

        Assert.Throws<ArgumentException>(() => dictionary.CopyTo(entries, 0));
    }

    [Fact]
    public void ICollection_CopyTo_FilledReadOnlyBidirectionalDictionaryAndOutOfRangeIndex_ThrowsArgumentOutOfRangeException()
    {
        var dictionary = (ICollection)CreateReadOnlyBidirectionalDictionaryForGenericOnlyDictionary();
        var entries = new DictionaryEntry[1];

        Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.CopyTo(entries, 2));
    }

    [Fact]
    public void ICollection_CopyTo_FilledReadOnlyBidirectionalDictionaryAndSmallArray_ThrowsArgumentException()
    {
        var dictionary = (ICollection)CreateReadOnlyBidirectionalDictionaryForGenericOnlyDictionary();
        var entries = Array.Empty<DictionaryEntry>();

        Assert.Throws<ArgumentException>(() => dictionary.CopyTo(entries, 0));
    }

    [Fact]
    public void ICollection_CopyTo_FilledReadOnlyBidirectionalDictionaryAndIncompatibleArray_ThrowsArgumentException()
    {
        var dictionary = (ICollection)CreateReadOnlyBidirectionalDictionaryForGenericOnlyDictionary();
        var entries = new int[2];

        Assert.Throws<ArgumentException>(() => dictionary.CopyTo(entries, 1));
    }

    [Fact]
    public void ICollection_CopyTo_FilledReadOnlyBidirectionalDictionaryAndMismatchedObjectArray_ThrowsArgumentException()
    {
        var dictionary = (ICollection)CreateReadOnlyBidirectionalDictionaryForGenericOnlyDictionary();
        var entries = new string[2];

        Assert.Throws<ArgumentException>(() => dictionary.CopyTo(entries, 1));
    }

    private static ReadOnlyBidirectionalDictionary<char, int> CreateReadOnlyBidirectionalDictionaryForNonGenericDictionary()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        return new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);
    }

    private static ReadOnlyBidirectionalDictionary<char, int> CreateReadOnlyBidirectionalDictionaryForGenericOnlyDictionary()
    {
        return new ReadOnlyBidirectionalDictionary<char, int>(
            new GenericOnlyBidirectionalDictionary<char, int>([new KeyValuePair<char, int>('a', 0)]));
    }

    private sealed class GenericOnlyBidirectionalDictionary<TKey, TValue> : IBidirectionalDictionary<TKey, TValue>
        where TKey : notnull
        where TValue : notnull
    {
        private readonly Dictionary<TKey, TValue> _dictionary;

        public GenericOnlyBidirectionalDictionary(IEnumerable<KeyValuePair<TKey, TValue>> entries)
        {
            _dictionary = entries.ToDictionary(entry => entry.Key, entry => entry.Value);
            Inverse = new GenericOnlyBidirectionalDictionary<TValue, TKey>(this);
        }

        private GenericOnlyBidirectionalDictionary(GenericOnlyBidirectionalDictionary<TValue, TKey> inverse)
        {
            _dictionary = inverse._dictionary.ToDictionary(entry => entry.Value, entry => entry.Key);
            Inverse = inverse;
        }

        public IBidirectionalDictionary<TValue, TKey> Inverse { get; }

        public ICollection<TKey> Keys => _dictionary.Keys;

        public ICollection<TValue> Values => _dictionary.Values;

        public int Count => _dictionary.Count;

        public bool IsReadOnly => true;

        public TValue this[TKey key]
        {
            get => _dictionary[key];
            set => throw new NotSupportedException();
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            return _dictionary.ContainsValue(value);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value!);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TKey key, TValue value)
        {
            throw new NotSupportedException();
        }

        public bool Remove(TKey key)
        {
            throw new NotSupportedException();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException();
        }
    }
}
