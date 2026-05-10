using System.Collections;
using System.Collections.Concurrent;

namespace BidirectionalDictionary.Tests.Types.ConcurrentBidirectionalDictionary;

public partial class ConcurrentBidirectionalDictionaryTests
{
    [Fact]
    public void Constructor_NoArguments_CreatesEmptyConcurrentBidirectionalDictionary()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();

        Assert.Empty(dictionary);
        Assert.Empty(dictionary.Keys);
        Assert.Empty(dictionary.Values);
        Assert.True(dictionary.IsEmpty);
        Assert.Empty(dictionary);
        Assert.Equal(EqualityComparer<char>.Default, dictionary.KeyComparer);
        Assert.Equal(EqualityComparer<int>.Default, dictionary.ValueComparer);
        Assert.Same(dictionary, dictionary.Inverse.Inverse);
        Assert.Equal(EqualityComparer<int>.Default, dictionary.Inverse.KeyComparer);
        Assert.Equal(EqualityComparer<char>.Default, dictionary.Inverse.ValueComparer);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(4, 10)]
    [InlineData(-1, 2)]
    public void Constructor_ValidConcurrencyLevelAndCapacity_CreatesEmptyDictionary(int concurrencyLevel, int capacity)
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>(concurrencyLevel, capacity);

        Assert.Empty(dictionary);
        Assert.True(dictionary.IsEmpty);
        Assert.Same(dictionary, dictionary.Inverse.Inverse);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(-2, 0)]
    public void Constructor_InvalidConcurrencyLevel_ThrowsArgumentOutOfRangeException(int concurrencyLevel, int capacity)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ConcurrentBidirectionalDictionary<char, int>(concurrencyLevel, capacity));
    }

    [Theory]
    [InlineData(1, -1)]
    [InlineData(-1, -1)]
    public void Constructor_InvalidCapacity_ThrowsArgumentOutOfRangeException(int concurrencyLevel, int capacity)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ConcurrentBidirectionalDictionary<char, int>(concurrencyLevel, capacity));
    }

    [Fact]
    public void Constructor_Comparers_CreatesDictionaryWithExpectedComparers()
    {
        var keyComparer = StringComparer.OrdinalIgnoreCase;
        var valueComparer = StringComparer.Ordinal;

        var dictionary = new ConcurrentBidirectionalDictionary<string, string>(keyComparer, valueComparer);

        Assert.Same(keyComparer, dictionary.KeyComparer);
        Assert.Same(valueComparer, dictionary.ValueComparer);
        Assert.Same(valueComparer, dictionary.Inverse.KeyComparer);
        Assert.Same(keyComparer, dictionary.Inverse.ValueComparer);
    }

    [Fact]
    public void Constructor_Collection_CreatesForwardAndInverseMappings()
    {
        var source = new[]
        {
            new KeyValuePair<char, int>('a', 1),
            new KeyValuePair<char, int>('b', 2),
        };

        var dictionary = new ConcurrentBidirectionalDictionary<char, int>(source);

        Assert.Equal(2, dictionary.Count);
        Assert.Equal(1, dictionary['a']);
        Assert.Equal(2, dictionary['b']);
        Assert.Equal('a', dictionary.Inverse[1]);
        Assert.Equal('b', dictionary.Inverse[2]);
    }

    [Fact]
    public void Constructor_CollectionWithComparers_CreatesDictionaryWithExpectedComparersAndMappings()
    {
        var source = new[]
        {
            new KeyValuePair<string, string>("a", "x"),
        };

        var keyComparer = StringComparer.OrdinalIgnoreCase;
        var valueComparer = StringComparer.Ordinal;

        var dictionary = new ConcurrentBidirectionalDictionary<string, string>(source, keyComparer, valueComparer);

        Assert.Same(keyComparer, dictionary.KeyComparer);
        Assert.Same(valueComparer, dictionary.ValueComparer);
        Assert.Equal("x", dictionary["A"]);
        Assert.Equal("a", dictionary.Inverse["x"]);
    }

    [Fact]
    public void Constructor_ConcurrencyLevelAndCollectionWithComparers_CreatesDictionaryWithExpectedComparersAndMappings()
    {
        var source = new[]
        {
            new KeyValuePair<string, string>("a", "x"),
        };

        var keyComparer = StringComparer.OrdinalIgnoreCase;
        var valueComparer = StringComparer.Ordinal;

        var dictionary = new ConcurrentBidirectionalDictionary<string, string>(1, source, keyComparer, valueComparer);

        Assert.Same(keyComparer, dictionary.KeyComparer);
        Assert.Same(valueComparer, dictionary.ValueComparer);
        Assert.Equal("x", dictionary["A"]);
        Assert.Equal("a", dictionary.Inverse["x"]);
    }

    [Fact]
    public void Constructor_CollectionWithDuplicateValue_ThrowsArgumentException()
    {
        var source = new[]
        {
            new KeyValuePair<char, int>('a', 1),
            new KeyValuePair<char, int>('b', 1),
        };

        Assert.Throws<ArgumentException>(() => new ConcurrentBidirectionalDictionary<char, int>(source));
    }

    [Fact]
    public void Constructor_ReadOnlyCollection_CreatesDictionaryFromSource()
    {
        var source = new ReadOnlyKeyValuePairCollection<char, int>(
        [
            new KeyValuePair<char, int>('a', 1),
        ]);

        var dictionary = new ConcurrentBidirectionalDictionary<char, int>(source);

        Assert.Single(dictionary, new KeyValuePair<char, int>('a', 1));
    }

    [Fact]
    public void Constructor_Enumerable_CreatesDictionaryFromSource()
    {
        static IEnumerable<KeyValuePair<char, int>> Source()
        {
            yield return new KeyValuePair<char, int>('a', 1);
        }

        var dictionary = new ConcurrentBidirectionalDictionary<char, int>(Source());

        Assert.Single(dictionary, new KeyValuePair<char, int>('a', 1));
    }

    [Fact]
    public void Constructor_NullCollection_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ConcurrentBidirectionalDictionary<char, int>(null!));
    }

    [Fact]
    public void Constructor_CollectionWithNullValue_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714
        var source = new[]
        {
            new KeyValuePair<char, int?>('a', null),
        };

        Assert.Throws<ArgumentNullException>(() => new ConcurrentBidirectionalDictionary<char, int?>(source));
#pragma warning restore CS8714
    }

    [Fact]
    public void TryAdd_NewKeyAndValue_AddsForwardAndInverseEntry()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();

        var added = dictionary.TryAdd('a', 1);

        Assert.True(added);
        Assert.Equal(1, dictionary['a']);
        Assert.Equal('a', dictionary.Inverse[1]);
        Assert.False(dictionary.IsEmpty);
    }

    [Theory]
    [InlineData('a', 2)]
    [InlineData('b', 1)]
    public void TryAdd_DuplicateKeyOrValue_ReturnsFalseAndDoesNotMutate(char key, int value)
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        Assert.True(dictionary.TryAdd('a', 1));

        var added = dictionary.TryAdd(key, value);

        Assert.False(added);
        Assert.Single(dictionary.ToArray(), new KeyValuePair<char, int>('a', 1));
        Assert.Single(dictionary.Inverse.ToArray(), new KeyValuePair<int, char>(1, 'a'));
    }

    [Fact]
    public void TryRemove_ExistingKey_RemovesForwardAndInverseEntry()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);

        var removed = dictionary.TryRemove('a', out var value);

        Assert.True(removed);
        Assert.Equal(1, value);
        Assert.Empty(dictionary);
        Assert.Empty(dictionary.Inverse);
    }

    [Fact]
    public void TryRemove_MissingKey_ReturnsFalseAndDefaultValue()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();

        var removed = dictionary.TryRemove('a', out var value);

        Assert.False(removed);
        Assert.Equal(default, value);
    }

    [Fact]
    public void TryRemove_KeyValuePairWithMismatchedValue_ReturnsFalse()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);

        var removed = dictionary.TryRemove(new KeyValuePair<char, int>('a', 2));

        Assert.False(removed);
        Assert.Single(dictionary.ToArray(), new KeyValuePair<char, int>('a', 1));
    }

    [Fact]
    public void TryRemove_KeyValuePairWithMissingKey_ReturnsFalse()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();

        var removed = dictionary.TryRemove(new KeyValuePair<char, int>('a', 1));

        Assert.False(removed);
        Assert.Empty(dictionary);
    }

    [Fact]
    public void TryUpdate_ExistingKeyAndExpectedValue_UpdatesForwardAndInverseEntry()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);

        var updated = dictionary.TryUpdate('a', 2, 1);

        Assert.True(updated);
        Assert.Equal(2, dictionary['a']);
        Assert.False(dictionary.Inverse.ContainsKey(1));
        Assert.Equal('a', dictionary.Inverse[2]);
    }

    [Fact]
    public void TryUpdate_NewValueOwnedByDifferentKey_ReturnsFalse()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);
        dictionary.TryAdd('b', 2);

        var updated = dictionary.TryUpdate('a', 2, 1);

        Assert.False(updated);
        Assert.Equal(1, dictionary['a']);
        Assert.Equal('b', dictionary.Inverse[2]);
    }

    [Fact]
    public void TryUpdate_MissingKey_ReturnsFalse()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();

        var updated = dictionary.TryUpdate('a', 2, 1);

        Assert.False(updated);
    }

    [Fact]
    public void TryUpdate_ExistingKeyAndDifferentComparisonValue_ReturnsFalse()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);

        var updated = dictionary.TryUpdate('a', 2, 0);

        Assert.False(updated);
        Assert.Equal(1, dictionary['a']);
    }

    [Fact]
    public void TryUpdate_ExistingKeyAndSameValue_ReturnsTrueWithoutChangingState()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);

        var updated = dictionary.TryUpdate('a', 1, 1);

        Assert.True(updated);
        Assert.Single(dictionary, new KeyValuePair<char, int>('a', 1));
        Assert.Single(dictionary.Inverse, new KeyValuePair<int, char>(1, 'a'));
    }

    [Fact]
    public void Clear_FilledDictionary_RemovesBothDirections()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);
        dictionary.TryAdd('b', 2);

        dictionary.Clear();

        Assert.Empty(dictionary);
        Assert.Empty(dictionary.Inverse);
        Assert.True(dictionary.IsEmpty);
    }

    [Fact]
    public void ToArray_FilledDictionary_ReturnsSnapshot()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);
        dictionary.TryAdd('b', 2);

        var snapshot = dictionary.ToArray();
        dictionary.Clear();

        Assert.Equal(2, snapshot.Length);
        Assert.Contains(new KeyValuePair<char, int>('a', 1), snapshot);
        Assert.Contains(new KeyValuePair<char, int>('b', 2), snapshot);
    }

    [Fact]
    public void InverseContainsKey_FilledConcurrentBidirectionalDictionary_ReturnsExpectedResult()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);

        Assert.True(dictionary.Inverse.ContainsKey(1));
        Assert.False(dictionary.Inverse.ContainsKey(2));
    }

    [Fact]
    public void ContainsValue_FilledConcurrentBidirectionalDictionary_ReturnsExpectedResult()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);

        Assert.True(dictionary.ContainsValue(1));
        Assert.False(dictionary.ContainsValue(2));
    }

    [Fact]
    public void Indexer_SetMissingKey_AddsForwardAndInverseEntry()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();

        dictionary['a'] = 1;

        Assert.Single(dictionary, new KeyValuePair<char, int>('a', 1));
        Assert.Single(dictionary.Inverse, new KeyValuePair<int, char>(1, 'a'));
    }

    [Fact]
    public void Indexer_SetMissingKeyAndDuplicateValue_ThrowsArgumentException()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);

        Assert.Throws<ArgumentException>(() => dictionary['b'] = 1);
        Assert.Single(dictionary, new KeyValuePair<char, int>('a', 1));
        Assert.Single(dictionary.Inverse, new KeyValuePair<int, char>(1, 'a'));
    }

    [Fact]
    public void Indexer_SetExistingKeyToSameValue_DoesNotChangeState()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);

        dictionary['a'] = 1;

        Assert.Single(dictionary, new KeyValuePair<char, int>('a', 1));
        Assert.Single(dictionary.Inverse, new KeyValuePair<int, char>(1, 'a'));
    }

    [Fact]
    public void GetOrAdd_ValueFactory_MissingKey_AddsProducedValue()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();

        var value = dictionary.GetOrAdd('a', key => key - 'a' + 1);

        Assert.Equal(1, value);
        Assert.Equal(1, dictionary['a']);
        Assert.Equal('a', dictionary.Inverse[1]);
    }

    [Fact]
    public void GetOrAdd_ValueFactory_ExistingKey_DoesNotInvokeFactory()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);

        var value = dictionary.GetOrAdd('a', _ => throw new InvalidOperationException());

        Assert.Equal(1, value);
    }

    [Fact]
    public void GetOrAdd_ProducedDuplicateValue_ThrowsArgumentException()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);

        Assert.Throws<ArgumentException>(() => dictionary.GetOrAdd('b', _ => 1));
        Assert.False(dictionary.ContainsKey('b'));
    }

    [Fact]
    public void GetOrAdd_Value_MissingKey_AddsValue()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();

        var value = dictionary.GetOrAdd('a', 1);

        Assert.Equal(1, value);
        Assert.Single(dictionary, new KeyValuePair<char, int>('a', 1));
        Assert.Single(dictionary.Inverse, new KeyValuePair<int, char>(1, 'a'));
    }

    [Fact]
    public void GetOrAdd_Value_ExistingKey_ReturnsExistingValue()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);

        var value = dictionary.GetOrAdd('a', 2);

        Assert.Equal(1, value);
        Assert.Single(dictionary, new KeyValuePair<char, int>('a', 1));
    }

    [Fact]
    public void GetOrAdd_ValueFactoryWithArgument_MissingKey_AddsProducedValue()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();

        var value = dictionary.GetOrAdd('a', (key, argument) => key - 'a' + argument, 1);

        Assert.Equal(1, value);
        Assert.Equal(1, dictionary['a']);
    }

    [Fact]
    public void GetOrAdd_ValueFactoryWithArgument_ExistingKey_DoesNotInvokeFactory()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);

        var value = dictionary.GetOrAdd('a', (_, _) => throw new InvalidOperationException(), 1);

        Assert.Equal(1, value);
    }

    [Fact]
    public void GetOrAdd_NullValueFactory_ThrowsArgumentNullException()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentNullException>(() => dictionary.GetOrAdd('a', (Func<char, int>)null!));
    }

    [Fact]
    public void GetOrAdd_NullValueFactoryWithArgument_ThrowsArgumentNullException()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentNullException>(() => dictionary.GetOrAdd('a', (Func<char, int, int>)null!, 1));
    }

    [Fact]
    public void AddOrUpdate_MissingKey_AddsValue()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();

        var value = dictionary.AddOrUpdate('a', 1, (_, oldValue) => oldValue + 1);

        Assert.Equal(1, value);
        Assert.Equal(1, dictionary['a']);
    }

    [Fact]
    public void AddOrUpdate_ExistingKey_UpdatesValue()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);

        var value = dictionary.AddOrUpdate('a', 10, (_, oldValue) => oldValue + 1);

        Assert.Equal(2, value);
        Assert.Equal(2, dictionary['a']);
        Assert.Equal('a', dictionary.Inverse[2]);
    }

    [Fact]
    public void AddOrUpdate_UpdateProducesDuplicateValue_ThrowsArgumentException()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);
        dictionary.TryAdd('b', 2);

        Assert.Throws<ArgumentException>(() => dictionary.AddOrUpdate('a', 10, (_, _) => 2));
        Assert.Equal(1, dictionary['a']);
        Assert.Equal('b', dictionary.Inverse[2]);
    }

    [Fact]
    public void AddOrUpdate_AddValueFactory_MissingKey_AddsProducedValue()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();

        var value = dictionary.AddOrUpdate('a', _ => 1, (_, oldValue) => oldValue + 1);

        Assert.Equal(1, value);
        Assert.Single(dictionary, new KeyValuePair<char, int>('a', 1));
    }

    [Fact]
    public void AddOrUpdate_AddValueFactory_ExistingKey_UpdatesValue()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);

        var value = dictionary.AddOrUpdate('a', _ => throw new InvalidOperationException(), (_, oldValue) => oldValue + 1);

        Assert.Equal(2, value);
        Assert.Single(dictionary, new KeyValuePair<char, int>('a', 2));
    }

    [Fact]
    public void AddOrUpdate_FactoriesWithArgument_MissingKey_AddsProducedValue()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();

        var value = dictionary.AddOrUpdate('a', (_, argument) => argument, (_, oldValue, argument) => oldValue + argument, 1);

        Assert.Equal(1, value);
        Assert.Single(dictionary, new KeyValuePair<char, int>('a', 1));
    }

    [Fact]
    public void AddOrUpdate_FactoriesWithArgument_ExistingKey_UpdatesValue()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);

        var value = dictionary.AddOrUpdate('a', (_, argument) => argument, (_, oldValue, argument) => oldValue + argument, 1);

        Assert.Equal(2, value);
        Assert.Single(dictionary, new KeyValuePair<char, int>('a', 2));
    }

    [Fact]
    public void AddOrUpdate_UpdateFactoryChangesKeyDuringFactory_RetriesAndUpdatesLatestValue()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);
        var calls = 0;

        var value = dictionary.AddOrUpdate('a', 0, (_, oldValue) =>
        {
            calls++;
            if (calls == 1)
            {
                dictionary['a'] = 2;
            }

            return oldValue + 2;
        });

        Assert.Equal(4, value);
        Assert.Equal(4, dictionary['a']);
        Assert.Equal(2, calls);
    }

    [Fact]
    public void AddOrUpdate_NullAddValueFactory_ThrowsArgumentNullException()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentNullException>(() => dictionary.AddOrUpdate('a', null!, (_, oldValue) => oldValue));
    }

    [Fact]
    public void AddOrUpdate_NullUpdateValueFactory_ThrowsArgumentNullException()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentNullException>(() => dictionary.AddOrUpdate('a', 1, null!));
    }

    [Fact]
    public void AddOrUpdate_NullUpdateValueFactoryForFactoryOverload_ThrowsArgumentNullException()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentNullException>(() => dictionary.AddOrUpdate('a', _ => 1, null!));
    }

    [Fact]
    public void AddOrUpdate_UpdateFactoryRemovesKeyDuringFactory_RetriesAndAddsProducedValue()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);
        var calls = 0;

        var value = dictionary.AddOrUpdate('a', _ => 4, (_, oldValue) =>
        {
            calls++;
            dictionary.TryRemove('a', out int _);
            return oldValue + 1;
        });

        Assert.Equal(4, value);
        Assert.Single(dictionary, new KeyValuePair<char, int>('a', 4));
        Assert.Equal(1, calls);
    }

    [Fact]
    public void AddOrUpdate_NullAddValueFactoryWithArgument_ThrowsArgumentNullException()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentNullException>(() => dictionary.AddOrUpdate('a', null!, (_, oldValue, _) => oldValue, 1));
    }

    [Fact]
    public void AddOrUpdate_NullUpdateValueFactoryWithArgument_ThrowsArgumentNullException()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentNullException>(() => dictionary.AddOrUpdate('a', (_, argument) => argument, null!, 1));
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(null, 1)]
    [InlineData('a', null)]
    public void TryAdd_NullKeyOrValue_ThrowsArgumentNullException(char? key, int? value)
    {
#pragma warning disable CS8714
        var dictionary = new ConcurrentBidirectionalDictionary<char?, int?>();
#pragma warning restore CS8714

        Assert.Throws<ArgumentNullException>(() => dictionary.TryAdd(key, value));
        Assert.Empty(dictionary);
    }

    [Fact]
    public void Indexer_SetDuplicateValueForDifferentKey_ThrowsArgumentException()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);
        dictionary.TryAdd('b', 2);

        Assert.Throws<ArgumentException>(() => dictionary['a'] = 2);
        Assert.Equal(1, dictionary['a']);
        Assert.Equal('b', dictionary.Inverse[2]);
    }

    [Fact]
    public void Inverse_TryAddDuplicateOriginalKey_ReturnsFalse()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);

        var added = dictionary.Inverse.TryAdd(2, 'a');

        Assert.False(added);
        Assert.Single(dictionary.ToArray(), new KeyValuePair<char, int>('a', 1));
    }

    private sealed class ReadOnlyKeyValuePairCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[] pairs)
        : IReadOnlyCollection<KeyValuePair<TKey, TValue>>
    {
        public int Count => pairs.Length;

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => ((IEnumerable<KeyValuePair<TKey, TValue>>)pairs).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
