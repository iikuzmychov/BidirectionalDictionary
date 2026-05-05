namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary;

public partial class BidirectionalDictionaryTests
{
    #region Constructor tests

    [Fact]
    [Trait("Constructor", null)]
    public void Constructor_NoArguments_CreatesEmptyBidirectionalDictionary()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>();
        
        Assert.Empty(bidirectionalDictionary);
        Assert.Empty(bidirectionalDictionary.Keys);
        Assert.Empty(bidirectionalDictionary.Values);
        Assert.Equal(EqualityComparer<char>.Default, bidirectionalDictionary.KeyComparer);
        Assert.Equal(EqualityComparer<int>.Default, bidirectionalDictionary.ValueComparer);
        Assert.Empty(bidirectionalDictionary.Inverse.Keys);
        Assert.Empty(bidirectionalDictionary.Inverse.Values);
        Assert.Equal(EqualityComparer<char>.Default, bidirectionalDictionary.Inverse.ValueComparer);
        Assert.Equal(EqualityComparer<int>.Default, bidirectionalDictionary.Inverse.KeyComparer);
    }

    [Theory]
    [Trait("Constructor", null)]
    [InlineData(0)]
    [InlineData(1)]
    public void Constructor_ValidCapacity_CreatesEmptyBidirectionalDictionary(int capacity)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>(capacity);

        Assert.Empty(bidirectionalDictionary);
        Assert.Empty(bidirectionalDictionary.Keys);
        Assert.Empty(bidirectionalDictionary.Values);
        Assert.Equal(EqualityComparer<char>.Default, bidirectionalDictionary.KeyComparer);
        Assert.Equal(EqualityComparer<int>.Default, bidirectionalDictionary.ValueComparer);
        Assert.Empty(bidirectionalDictionary.Inverse.Keys);
        Assert.Empty(bidirectionalDictionary.Inverse.Values);
        Assert.Equal(EqualityComparer<char>.Default, bidirectionalDictionary.Inverse.ValueComparer);
        Assert.Equal(EqualityComparer<int>.Default, bidirectionalDictionary.Inverse.KeyComparer);
    }

    [Theory]
    [Trait("Constructor", null)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void Constructor_InvalidCapacity_ThrowsArgumentOutOfRangeException(int capacity)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = new BidirectionalDictionary<char, int>(capacity));
    }
    
    [Fact]
    [Trait("Constructor", null)]
    public void Constructor_FilledSourceDictionary_CreatesBidirectionalDictionaryFromSourceDictionary()
    {
        var dictionary = new Dictionary<char, int>
        {
            { 'a', 1 },
            { 'b', 2 },
        };

        var bidirectionalDictionary = new BidirectionalDictionary<char, int>(dictionary);

        Assert.Equal(dictionary, bidirectionalDictionary);
        Assert.Equal(dictionary.Keys, bidirectionalDictionary.Keys);
        Assert.Equal(dictionary.Values, bidirectionalDictionary.Values);
        Assert.Equal(EqualityComparer<char>.Default, bidirectionalDictionary.KeyComparer);
        Assert.Equal(EqualityComparer<int>.Default, bidirectionalDictionary.ValueComparer);
        Assert.Equal(dictionary.Keys, bidirectionalDictionary.Inverse.Values);
        Assert.Equal(dictionary.Values, bidirectionalDictionary.Inverse.Keys);
        Assert.Equal(EqualityComparer<char>.Default, bidirectionalDictionary.Inverse.ValueComparer);
        Assert.Equal(EqualityComparer<int>.Default, bidirectionalDictionary.Inverse.KeyComparer);
    }

    [Fact]
    [Trait("Constructor", null)]
    public void Constructor_NullSourceIDictionary_ThrowsArgumentNullException()
    {
        var dictionary = (IDictionary<char, int>?)null;

        Assert.Throws<ArgumentNullException>(() => _ = new BidirectionalDictionary<char, int>(dictionary!));
    }

    [Fact]
    [Trait("Constructor", null)]
    public void Constructor_FilledSourceDictionaryAndComparers_CreatesBidirectionalDictionaryWithExpectedComparers()
    {
        var dictionary = new Dictionary<string, string>
        {
            { "a", "x" },
            { "b", "y" },
        };

        var keyComparer = StringComparer.OrdinalIgnoreCase;
        var valueComparer = StringComparer.Ordinal;

        var bidirectionalDictionary = new BidirectionalDictionary<string, string>(dictionary, keyComparer, valueComparer);

        Assert.Equal(dictionary, bidirectionalDictionary);
        Assert.Equal(keyComparer, bidirectionalDictionary.KeyComparer);
        Assert.Equal(valueComparer, bidirectionalDictionary.ValueComparer);
        Assert.Equal(keyComparer, bidirectionalDictionary.Inverse.ValueComparer);
        Assert.Equal(valueComparer, bidirectionalDictionary.Inverse.KeyComparer);
    }

    [Fact]
    [Trait("Constructor", null)]
    public void Constructor_FilledKeyValuePairsList_CreatesBidirectionalDictionaryFromSourceDictionary()
    {
        var list = new List<KeyValuePair<char, int>>
        {
            { new ('a', 1) },
            { new ('b', 2) },
        };

        var bidirectionalDictionary = new BidirectionalDictionary<char, int>(list);

        Assert.Equal(list, bidirectionalDictionary);
        Assert.Equal(list.Select(pair => pair.Key), bidirectionalDictionary.Keys);
        Assert.Equal(list.Select(pair => pair.Value), bidirectionalDictionary.Values);
        Assert.Equal(EqualityComparer<char>.Default, bidirectionalDictionary.KeyComparer);
        Assert.Equal(EqualityComparer<int>.Default, bidirectionalDictionary.ValueComparer);
        Assert.Equal(list.Select(pair => pair.Key), bidirectionalDictionary.Inverse.Values);
        Assert.Equal(list.Select(pair => pair.Value), bidirectionalDictionary.Inverse.Keys);
        Assert.Equal(EqualityComparer<char>.Default, bidirectionalDictionary.Inverse.ValueComparer);
        Assert.Equal(EqualityComparer<int>.Default, bidirectionalDictionary.Inverse.KeyComparer);
    }

    [Fact]
    [Trait("Constructor", null)]
    public void Constructor_NullKeyValuePairsEnumerable_ThrowsArgumentNullException()
    {
        var collection = (ICollection<KeyValuePair<char, int>>?)null;

        Assert.Throws<ArgumentNullException>(() => _ = new BidirectionalDictionary<char, int>(collection!));
    }

    /*[Fact]
    public void Constructor_Comaparers_CreatesEmptyBidirectionalDictionaryFromSourceDictionary()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>(keyComparer, valueComparer);

        Assert.Empty(bidirectionalDictionary);
        Assert.Empty(bidirectionalDictionary.Keys);
        Assert.Empty(bidirectionalDictionary.Values);
        Assert.Equal(keyComparer, bidirectionalDictionary.KeyComparer);
        Assert.Equal(valueComparer, bidirectionalDictionary.ValueComparer);
        Assert.Empty(bidirectionalDictionary.Inverse.Keys);
        Assert.Empty(bidirectionalDictionary.Inverse.Values);
        Assert.Equal(keyComparer, bidirectionalDictionary.Inverse.ValueComparer);
        Assert.Equal(valueComparer, bidirectionalDictionary.Inverse.KeyComparer);
    }*/

    #endregion

    #region Method tests

    [Theory]
    [Trait("Method", null)]
    [InlineData('a', 0)]
    [InlineData('b', 1)]
    public void Add_EmptyBidirectionalDictionaryAndNonDuplicateKeyValue_AddsEntrySuccessfully(char key, int value)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>();

        bidirectionalDictionary.Add(key, value);

        Assert.Single(bidirectionalDictionary, new KeyValuePair<char, int>(key, value));
        Assert.Single(bidirectionalDictionary.Keys, key);
        Assert.Single(bidirectionalDictionary.Values, value);
        Assert.Single(bidirectionalDictionary.Inverse, new KeyValuePair<int, char>(value, key));
        Assert.Single(bidirectionalDictionary.Inverse.Keys, value);
        Assert.Single(bidirectionalDictionary.Inverse.Values, key);
    }

    [Theory]
    [Trait("Method", null)]
    [InlineData(null, null)]
    [InlineData(null, 0)]
    [InlineData('a', null)]
    public void Add_EmptyBidirectionalDictionaryAndNullKeyValue_ThrowsArgumentNullException(char? key, int? value)
    {
#pragma warning disable CS8714
        var bidirectionalDictionary = new BidirectionalDictionary<char?, int?>();
#pragma warning restore CS8714

        Assert.Throws<ArgumentNullException>(() => bidirectionalDictionary.Add(key, value));

        // checking that bidirectionalDictionary has not changed
        Assert.Empty(bidirectionalDictionary);
        Assert.Empty(bidirectionalDictionary.Keys);
        Assert.Empty(bidirectionalDictionary.Values);
        Assert.Empty(bidirectionalDictionary.Inverse.Keys);
        Assert.Empty(bidirectionalDictionary.Inverse.Values);
    }

    [Theory]
    [Trait("Method", null)]
    [InlineData('a', 0)]
    [InlineData('a', 1)]
    [InlineData('b', 0)]
    public void Add_FilledBidirectionalDictionaryAndDuplicateKeyValue_ThrowsArgumentException(char key, int value)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        Assert.Throws<ArgumentException>(() => bidirectionalDictionary.Add(key, value));

        // checking that bidirectionalDictionary has not changed
        Assert.Single(bidirectionalDictionary, new KeyValuePair<char, int>('a', 0));
        Assert.Single(bidirectionalDictionary.Keys, 'a');
        Assert.Single(bidirectionalDictionary.Values, 0);
        Assert.Single(bidirectionalDictionary.Inverse, new KeyValuePair<int, char>(0, 'a'));
        Assert.Single(bidirectionalDictionary.Inverse.Keys, 0);
        Assert.Single(bidirectionalDictionary.Inverse.Values, 'a');
    }

    [Fact]
    [Trait("Method", null)]
    public void Remove_FilledBidirectionalDictionaryAndExistingKey_RemovesEntrySuccessfullyAndReturnsTrueAndReturnsOutRemovedValue()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var key = 'a';

        var isRemoved = bidirectionalDictionary.Remove(key, out var removedValue);

        Assert.True(isRemoved);
        Assert.Equal(0, removedValue);
        Assert.Empty(bidirectionalDictionary);
        Assert.Empty(bidirectionalDictionary.Keys);
        Assert.Empty(bidirectionalDictionary.Values);
        Assert.Empty(bidirectionalDictionary.Inverse.Keys);
        Assert.Empty(bidirectionalDictionary.Inverse.Values);
    }

    [Theory]
    [Trait("Method", null)]
    [InlineData('b')]
    [InlineData('c')]
    public void Remove_FilledBidirectionalDictionaryAndMissingKey_ReturnsFalse(char key)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var isRemoved = bidirectionalDictionary.Remove(key);

        Assert.False(isRemoved);

        // checking that bidirectionalDictionary has not changed
        Assert.Single(bidirectionalDictionary, new KeyValuePair<char, int>('a', 0));
        Assert.Single(bidirectionalDictionary.Keys, 'a');
        Assert.Single(bidirectionalDictionary.Values, 0);
        Assert.Single(bidirectionalDictionary.Inverse, new KeyValuePair<int, char>(0, 'a'));
        Assert.Single(bidirectionalDictionary.Inverse.Keys, 0);
        Assert.Single(bidirectionalDictionary.Inverse.Values, 'a');
    }

    [Fact]
    [Trait("Method", null)]
    public void Remove_EmptyBidirectionalDictionaryAndNullKey_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714
        var bidirectionalDictionary = new BidirectionalDictionary<char?, int>();
#pragma warning restore CS8714

        var key = (char?)null;

        Assert.Throws<ArgumentNullException>(() => bidirectionalDictionary.Remove(key));

        // checking that bidirectionalDictionary has not changed
        Assert.Empty(bidirectionalDictionary);
        Assert.Empty(bidirectionalDictionary.Keys);
        Assert.Empty(bidirectionalDictionary.Values);
        Assert.Empty(bidirectionalDictionary.Inverse.Keys);
        Assert.Empty(bidirectionalDictionary.Inverse.Values);
    }

    [Fact]
    [Trait("Method", null)]
    public void Clear_FilledBidirectionalDictionary_RemovesAllEntriesSuccessfully()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        bidirectionalDictionary.Clear();

        Assert.Empty(bidirectionalDictionary);
        Assert.Empty(bidirectionalDictionary.Keys);
        Assert.Empty(bidirectionalDictionary.Values);
        Assert.Empty(bidirectionalDictionary.Inverse.Keys);
        Assert.Empty(bidirectionalDictionary.Inverse.Values);
    }

    [Theory]
    [Trait("Method", null)]
    [InlineData('a', true)]
    [InlineData('b', false)]
    [InlineData('c', false)]
    public void ContainsKey_FilledBidirectionalDictionaryAndExistingKey_ReturnsExpectedResult(char key, bool expectedResult)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var isExists = bidirectionalDictionary.ContainsKey(key);

        Assert.Equal(expectedResult, isExists);
    }

    [Fact]
    [Trait("Method", null)]
    public void ContainsKey_FilledBidirectionalDictionaryAndNullKey_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714
        var bidirectionalDictionary = new BidirectionalDictionary<char?, int>()
        {
            { 'a', 0 },
        };
#pragma warning restore CS8714

        var key = (char?)null;

        Assert.Throws<ArgumentNullException>(() => bidirectionalDictionary.ContainsKey(key));
    }

    [Theory]
    [Trait("Method", null)]
    [InlineData(0, true)]
    [InlineData(1, false)]
    [InlineData(2, false)]
    public void ContainsValue_FilledBidirectionalDictionaryAndExistingKey_ReturnsExpectedResult(int value, bool expectedResult)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var isExists = bidirectionalDictionary.ContainsValue(value);

        Assert.Equal(expectedResult, isExists);
    }

    [Fact]
    [Trait("Method", null)]
    public void ContainsValue_FilledBidirectionalDictionaryAndNullKey_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714
        var bidirectionalDictionary = new BidirectionalDictionary<char, int?>()
        {
            { 'a', 0 },
        };
#pragma warning restore CS8714

        var value = (char?)null;

        Assert.Throws<ArgumentNullException>(() => bidirectionalDictionary.ContainsValue(value));
    }

    [Theory]
    [Trait("Method", null)]
    [InlineData('a', 0)]
    [InlineData('b', 1)]
    public void TryAdd_EmptyBidirectionalDictionaryAndExistingKey_AddsEntrySuccessfullyAndReturnsTrue(char key, int value)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>();

        var isAdded = bidirectionalDictionary.TryAdd(key, value);

        Assert.True(isAdded);
        Assert.Single(bidirectionalDictionary, new KeyValuePair<char, int>(key, value));
        Assert.Single(bidirectionalDictionary.Keys, key);
        Assert.Single(bidirectionalDictionary.Values, value);
        Assert.Single(bidirectionalDictionary.Inverse, new KeyValuePair<int, char>(value, key));
        Assert.Single(bidirectionalDictionary.Inverse.Keys, value);
        Assert.Single(bidirectionalDictionary.Inverse.Values, key);

    }

    [Theory]
    [Trait("Method", null)]
    [InlineData('a', 0)]
    [InlineData('a', 1)]
    [InlineData('b', 0)]
    public void TryAdd_FilledBidirectionalDictionaryAndMissingKey_ReturnsFalse(char key, int value)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var isAdded = bidirectionalDictionary.TryAdd(key, value);

        Assert.False(isAdded);

        // checking that bidirectionalDictionary has not changed
        Assert.Single(bidirectionalDictionary, new KeyValuePair<char, int>('a', 0));
        Assert.Single(bidirectionalDictionary.Keys, 'a');
        Assert.Single(bidirectionalDictionary.Values, 0);
        Assert.Single(bidirectionalDictionary.Inverse, new KeyValuePair<int, char>(0, 'a'));
        Assert.Single(bidirectionalDictionary.Inverse.Keys, 0);
        Assert.Single(bidirectionalDictionary.Inverse.Values, 'a');
    }

    [Theory]
    [Trait("Method", null)]
    [InlineData(null, null)]
    [InlineData(null, 0)]
    [InlineData('a', null)]
    public void TryAdd_EmptyBidirectionalDictionaryAndNullKeyValue_ThrowsArgumentNullException(char? key, int? value)
    {
#pragma warning disable CS8714
        var bidirectionalDictionary = new BidirectionalDictionary<char?, int?>();
#pragma warning restore CS8714

        Assert.Throws<ArgumentNullException>(() => _ = bidirectionalDictionary.TryAdd(key, value));

        // checking that bidirectionalDictionary has not changed
        Assert.Empty(bidirectionalDictionary);
        Assert.Empty(bidirectionalDictionary.Keys);
        Assert.Empty(bidirectionalDictionary.Values);
        Assert.Empty(bidirectionalDictionary.Inverse.Keys);
        Assert.Empty(bidirectionalDictionary.Inverse.Values);
    }

    [Fact]
    [Trait("Method", null)]
    public void TryGetValue_FilledBidirectionalDictionaryAndExistingKey_ReturnsTrueAndReturnsOutExpectedValue()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 }
        };

        var key = 'a';
        var expectedValue = 0;

        var isExists = bidirectionalDictionary.TryGetValue(key, out var value);

        Assert.True(isExists);
        Assert.Equal(expectedValue, value);

    }

    [Theory]
    [Trait("Method", null)]
    [InlineData('a')]
    [InlineData('b')]
    public void TryGetValue_EmptyBidirectionalDictionaryAndMissingKey_ReturnsFalse(char key)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>();

        var isExists = bidirectionalDictionary.TryGetValue(key, out _);

        Assert.False(isExists);
    }

    [Fact]
    [Trait("Method", null)]
    public void TryGetValue_EmptyBidirectionalDictionaryAndNullKey_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714
        var bidirectionalDictionary = new BidirectionalDictionary<char?, int?>();
#pragma warning restore CS8714

        var key = (char?)null;

        Assert.Throws<ArgumentNullException>(() => _ = bidirectionalDictionary.TryGetValue(key, out _));
    }

    [Theory]
    [Trait("Method", null)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    public void EnsureCapacity_EmptyBidirectionalDictionaryAndValidCapacity_DoesNotChangeState(int capacity)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>();

        bidirectionalDictionary.EnsureCapacity(capacity);

        Assert.Empty(bidirectionalDictionary);
        Assert.Empty(bidirectionalDictionary.Keys);
        Assert.Empty(bidirectionalDictionary.Values);
        Assert.Empty(bidirectionalDictionary.Inverse.Keys);
        Assert.Empty(bidirectionalDictionary.Inverse.Values);
    }

    [Theory]
    [Trait("Method", null)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void EnsureCapacity_EmptyBidirectionalDictionaryAndInvalidCapacity_ThrowsArgumentOutOfRangeException(int capacity)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentOutOfRangeException>(() => bidirectionalDictionary.EnsureCapacity(capacity));

        // checking that bidirectionalDictionary has not changed
        Assert.Empty(bidirectionalDictionary);
        Assert.Empty(bidirectionalDictionary.Keys);
        Assert.Empty(bidirectionalDictionary.Values);
        Assert.Empty(bidirectionalDictionary.Inverse.Keys);
        Assert.Empty(bidirectionalDictionary.Inverse.Values);
    }

    [Fact]
    [Trait("Method", null)]
    public void TrimExcess_FilledBidirectionalDictionary_TrimmedSuccessfullyAndDoesNotChangeState()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        bidirectionalDictionary.TrimExcess();

        Assert.Equal(2, bidirectionalDictionary.Count);
        Assert.Equal(2, bidirectionalDictionary.Inverse.Count);
        Assert.Contains(new KeyValuePair<char, int>('a', 0), bidirectionalDictionary);
        Assert.Contains(new KeyValuePair<char, int>('b', 1), bidirectionalDictionary);
        Assert.Contains(new KeyValuePair<int, char>(0, 'a'), bidirectionalDictionary.Inverse);
        Assert.Contains(new KeyValuePair<int, char>(1, 'b'), bidirectionalDictionary.Inverse);
    }

    [Theory]
    [Trait("Method", null)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(10)]
    public void TrimExcess_FilledBidirectionalDictionaryAndValidCapacity_TrimmedSuccessfullyAndDoesNotChangeState(int capacity)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        bidirectionalDictionary.TrimExcess(capacity);

        Assert.Equal(2, bidirectionalDictionary.Count);
        Assert.Equal(2, bidirectionalDictionary.Inverse.Count);
        Assert.Contains(new KeyValuePair<char, int>('a', 0), bidirectionalDictionary);
        Assert.Contains(new KeyValuePair<char, int>('b', 1), bidirectionalDictionary);
        Assert.Contains(new KeyValuePair<int, char>(0, 'a'), bidirectionalDictionary.Inverse);
        Assert.Contains(new KeyValuePair<int, char>(1, 'b'), bidirectionalDictionary.Inverse);
    }

    [Theory]
    [Trait("Method", null)]
    [InlineData(-1)]
    [InlineData(1)]
    public void TrimExcess_FilledBidirectionalDictionaryAndInvalidCapacity_ThrowsArgumentOutOfRangeException(int capacity)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        Assert.Throws<ArgumentOutOfRangeException>(() => bidirectionalDictionary.TrimExcess(capacity));

        // checking that bidirectionalDictionary has not changed
        Assert.Equal(2, bidirectionalDictionary.Count);
        Assert.Equal(2, bidirectionalDictionary.Inverse.Count);
        Assert.Contains(new KeyValuePair<char, int>('a', 0), bidirectionalDictionary);
        Assert.Contains(new KeyValuePair<char, int>('b', 1), bidirectionalDictionary);
        Assert.Contains(new KeyValuePair<int, char>(0, 'a'), bidirectionalDictionary.Inverse);
        Assert.Contains(new KeyValuePair<int, char>(1, 'b'), bidirectionalDictionary.Inverse);
    }

    [Fact]
    [Trait("Method", null)]
    public void AsReadOnly_FilledBidirectionalDictionary_ReturnsReadOnlyWrapper()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = bidirectionalDictionary.AsReadOnly();

        Assert.Equal(bidirectionalDictionary, readOnlyBidirectionalDictionary);
        Assert.Same(readOnlyBidirectionalDictionary, readOnlyBidirectionalDictionary.Inverse.Inverse);
    }

    #endregion

    #region Indexer tests

    [Fact]
    [Trait("Indexer", null)]
    public void Indexer_Get_FilledBidirectionalDictionaryAndExistingKey_ReturnsExpectedValue()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var key = 'a';
        var expectedValue = 0;

        var value = bidirectionalDictionary[key];

        Assert.Equal(expectedValue, value);
    }

    [Fact]
    [Trait("Indexer", null)]
    public void Indexer_Get_EmptyBidirectionalDictionaryAndNullKey_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714
        var bidirectionalDictionary = new BidirectionalDictionary<char?, int>();
#pragma warning restore CS8714

        var key = (char?)null;

        Assert.Throws<ArgumentNullException>(() => _ = bidirectionalDictionary[key]);
    }

    [Theory]
    [Trait("Indexer", null)]
    [InlineData('a')]
    [InlineData('b')]
    public void Indexer_Get_EmptyBidirectionalDictionaryAndMissingKey_ThrowsKeyNotFoundException(char key)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>();

        Assert.Throws<KeyNotFoundException>(() => _ = bidirectionalDictionary[key]);
    }

    [Theory]
    [Trait("Indexer", null)]
    [InlineData('a', 0)]
    [InlineData('a', 1)]
    public void Indexer_Set_FilledBidirectionalDictionaryAndExistingKeyAndNonDuplicateValue_UpdateValueSuccessfully(char key, int value)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        bidirectionalDictionary[key] = value;

        Assert.Single(bidirectionalDictionary, new KeyValuePair<char, int>(key, value));
        Assert.Single(bidirectionalDictionary.Keys, key);
        Assert.Single(bidirectionalDictionary.Values, value);
        Assert.Single(bidirectionalDictionary.Inverse, new KeyValuePair<int, char>(value, key));
        Assert.Single(bidirectionalDictionary.Inverse.Keys, value);
        Assert.Single(bidirectionalDictionary.Inverse.Values, key);
    }

    [Fact]
    [Trait("Indexer", null)]
    public void Indexer_Set_EmptyBidirectionalDictionaryAndNullKeyAndNonDuplicateValue_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714
        var bidirectionalDictionary = new BidirectionalDictionary<char?, int>();
#pragma warning restore CS8714

        var key = (char?)null;
        var value = 0;

        Assert.Throws<ArgumentNullException>(() => bidirectionalDictionary[key] = value);
    }

    [Fact]
    [Trait("Indexer", null)]
    public void Indexer_Set_FilledBidirectionalDictionaryAndExistingKeyAndNullValue_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714
        var bidirectionalDictionary = new BidirectionalDictionary<char, int?>()
        {
            { 'a', 0 }
        };
#pragma warning restore CS8714

        var key = 'a';
        var value = (int?)null;

        Assert.Throws<ArgumentNullException>(() => bidirectionalDictionary[key] = value);
    }

    [Theory]
    [Trait("Indexer", null)]
    [InlineData('a', 1)]
    [InlineData('b', 0)]
    [InlineData('c', 0)]
    [InlineData('c', 1)]
    public void Indexer_Set_FilledBidirectionalDictionaryAndNotNullKeyAndDuplicateValue_ThrowsArgumentException(char key, int value)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        Assert.Throws<ArgumentException>(() => bidirectionalDictionary[key] = value);

        // checking that bidirectionalDictionary has not changed
        Assert.Single(bidirectionalDictionary, new KeyValuePair<char, int>('a', 0));
        Assert.Single(bidirectionalDictionary.Keys, 'a');
        Assert.Single(bidirectionalDictionary.Values, 0);
        Assert.Single(bidirectionalDictionary.Inverse, new KeyValuePair<int, char>(0, 'a'));
        Assert.Single(bidirectionalDictionary.Inverse.Keys, 0);
        Assert.Single(bidirectionalDictionary.Inverse.Values, 'a');
    }

    [Fact]
    [Trait("Indexer", null)]
    public void Indexer_Set_EmptyBidirectionalDictionaryAndMissingKeyAndNonDuplicateValue_CreatesNewEntry()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>();
        var key = 'a';
        var value = 0;

        bidirectionalDictionary[key] = value;

        Assert.Single(bidirectionalDictionary, new KeyValuePair<char, int>('a', 0));
        Assert.Single(bidirectionalDictionary.Keys, 'a');
        Assert.Single(bidirectionalDictionary.Values, 0);
        Assert.Single(bidirectionalDictionary.Inverse, new KeyValuePair<int, char>(0, 'a'));
        Assert.Single(bidirectionalDictionary.Inverse.Keys, 0);
        Assert.Single(bidirectionalDictionary.Inverse.Values, 'a');
    }

    #endregion
}
