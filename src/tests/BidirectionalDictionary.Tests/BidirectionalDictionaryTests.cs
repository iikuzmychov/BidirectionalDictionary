public partial class BidirectionalDictionaryTests
{
    #region Constructor tests

    [Fact]
    [Trait("Constructor", null)]
    public void Constructor_NoArguments_CreatesEmptyBiDictionary()
    {
        var biDictionary = new BidirectionalDictionary<char, int>();
        
        Assert.Empty(biDictionary);
        Assert.Empty(biDictionary.Keys);
        Assert.Empty(biDictionary.Values);
        Assert.Equal(EqualityComparer<char>.Default, biDictionary.KeyComparer);
        Assert.Equal(EqualityComparer<int>.Default, biDictionary.ValueComparer);
        Assert.Empty(biDictionary.Inverse.Keys);
        Assert.Empty(biDictionary.Inverse.Values);
        Assert.Equal(EqualityComparer<char>.Default, biDictionary.Inverse.ValueComparer);
        Assert.Equal(EqualityComparer<int>.Default, biDictionary.Inverse.KeyComparer);
    }

    [Theory]
    [Trait("Constructor", null)]
    [InlineData(0)]
    [InlineData(1)]
    public void Constructor_ValidCapacity_CreatesEmptyBiDictionary(int capacity)
    {
        var biDictionary = new BidirectionalDictionary<char, int>(capacity);

        Assert.Empty(biDictionary);
        Assert.Empty(biDictionary.Keys);
        Assert.Empty(biDictionary.Values);
        Assert.Equal(EqualityComparer<char>.Default, biDictionary.KeyComparer);
        Assert.Equal(EqualityComparer<int>.Default, biDictionary.ValueComparer);
        Assert.Empty(biDictionary.Inverse.Keys);
        Assert.Empty(biDictionary.Inverse.Values);
        Assert.Equal(EqualityComparer<char>.Default, biDictionary.Inverse.ValueComparer);
        Assert.Equal(EqualityComparer<int>.Default, biDictionary.Inverse.KeyComparer);
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
    public void Constructor_FilledSourceDictionary_CreatesBiDictionaryFromSourceDictionary()
    {
        var dictionary = new Dictionary<char, int>
        {
            { 'a', 1 },
            { 'b', 2 },
        };

        var biDictionary = new BidirectionalDictionary<char, int>(dictionary);

        Assert.Equal(dictionary, biDictionary);
        Assert.Equal(dictionary.Keys, biDictionary.Keys);
        Assert.Equal(dictionary.Values, biDictionary.Values);
        Assert.Equal(EqualityComparer<char>.Default, biDictionary.KeyComparer);
        Assert.Equal(EqualityComparer<int>.Default, biDictionary.ValueComparer);
        Assert.Equal(dictionary.Keys, biDictionary.Inverse.Values);
        Assert.Equal(dictionary.Values, biDictionary.Inverse.Keys);
        Assert.Equal(EqualityComparer<char>.Default, biDictionary.Inverse.ValueComparer);
        Assert.Equal(EqualityComparer<int>.Default, biDictionary.Inverse.KeyComparer);
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
    public void Constructor_FilledKeyValuePairsList_CreatesBiDictionaryFromSourceDictionary()
    {
        var list = new List<KeyValuePair<char, int>>
        {
            { new ('a', 1) },
            { new ('b', 2) },
        };

        var biDictionary = new BidirectionalDictionary<char, int>(list);

        Assert.Equal(list, biDictionary);
        Assert.Equal(list.Select(pair => pair.Key), biDictionary.Keys);
        Assert.Equal(list.Select(pair => pair.Value), biDictionary.Values);
        Assert.Equal(EqualityComparer<char>.Default, biDictionary.KeyComparer);
        Assert.Equal(EqualityComparer<int>.Default, biDictionary.ValueComparer);
        Assert.Equal(list.Select(pair => pair.Key), biDictionary.Inverse.Values);
        Assert.Equal(list.Select(pair => pair.Value), biDictionary.Inverse.Keys);
        Assert.Equal(EqualityComparer<char>.Default, biDictionary.Inverse.ValueComparer);
        Assert.Equal(EqualityComparer<int>.Default, biDictionary.Inverse.KeyComparer);
    }

    [Fact]
    [Trait("Constructor", null)]
    public void Constructor_NullKeyValuePairsEnumerable_ThrowsArgumentNullException()
    {
        var collection = (ICollection<KeyValuePair<char, int>>?)null;

        Assert.Throws<ArgumentNullException>(() => _ = new BidirectionalDictionary<char, int>(collection!));
    }

    /*[Fact]
    public void Constructor_Comaparers_CreatesEmptyBiDictionaryFromSourceDictionary()
    {
        var biDictionary = new BidirectionalDictionary<char, int>(keyComparer, valueComparer);

        Assert.Empty(biDictionary);
        Assert.Empty(biDictionary.Keys);
        Assert.Empty(biDictionary.Values);
        Assert.Equal(keyComparer, biDictionary.KeyComparer);
        Assert.Equal(valueComparer, biDictionary.ValueComparer);
        Assert.Empty(biDictionary.Inverse.Keys);
        Assert.Empty(biDictionary.Inverse.Values);
        Assert.Equal(keyComparer, biDictionary.Inverse.ValueComparer);
        Assert.Equal(valueComparer, biDictionary.Inverse.KeyComparer);
    }*/

    #endregion

    #region Method tests

    [Theory]
    [Trait("Method", null)]
    [InlineData('a', 0)]
    [InlineData('b', 1)]
    public void Add_EmptyBiDictionaryAndNonDuplicateKeyValue_AddsEntrySuccessfully(char key, int value)
    {
        var biDictionary = new BidirectionalDictionary<char, int>();

        biDictionary.Add(key, value);

        Assert.Single(biDictionary, new KeyValuePair<char, int>(key, value));
        Assert.Single(biDictionary.Keys, key);
        Assert.Single(biDictionary.Values, value);
        Assert.Single(biDictionary.Inverse, new KeyValuePair<int, char>(value, key));
        Assert.Single(biDictionary.Inverse.Keys, value);
        Assert.Single(biDictionary.Inverse.Values, key);
    }

    [Theory]
    [Trait("Method", null)]
    [InlineData(null, null)]
    [InlineData(null, 0)]
    [InlineData('a', null)]
    public void Add_EmptyBiDictionaryAndNullKeyValue_ThrowsArgumentNullException(char? key, int? value)
    {
#pragma warning disable CS8714 // Тип не может быть использован как параметр типа в универсальном типе или методе. Допустимость значения NULL для аргумента типа не соответствует ограничению "notnull".
        var biDictionary = new BidirectionalDictionary<char?, int?>();
#pragma warning restore CS8714 // Тип не может быть использован как параметр типа в универсальном типе или методе. Допустимость значения NULL для аргумента типа не соответствует ограничению "notnull".

        Assert.Throws<ArgumentNullException>(() => biDictionary.Add(key, value));

        // checking that biDictionary has not changed
        Assert.Empty(biDictionary);
        Assert.Empty(biDictionary.Keys);
        Assert.Empty(biDictionary.Values);
        Assert.Empty(biDictionary.Inverse.Keys);
        Assert.Empty(biDictionary.Inverse.Values);
    }

    [Theory]
    [Trait("Method", null)]
    [InlineData('a', 0)]
    [InlineData('a', 1)]
    [InlineData('b', 0)]
    public void Add_FilledBiDictionaryAndDuplicateKeyValue_ThrowsArgumentException(char key, int value)
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        Assert.Throws<ArgumentException>(() => biDictionary.Add(key, value));

        // checking that biDictionary has not changed
        Assert.Single(biDictionary, new KeyValuePair<char, int>('a', 0));
        Assert.Single(biDictionary.Keys, 'a');
        Assert.Single(biDictionary.Values, 0);
        Assert.Single(biDictionary.Inverse, new KeyValuePair<int, char>(0, 'a'));
        Assert.Single(biDictionary.Inverse.Keys, 0);
        Assert.Single(biDictionary.Inverse.Values, 'a');
    }

    [Fact]
    [Trait("Method", null)]
    public void Remove_FilledBiDictionaryAndExistingKey_RemovesEntrySuccessfullyAndReturnsTrueAndReturnsOutRemovedValue()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var key = 'a';

        var isRemoved = biDictionary.Remove(key, out var removedValue);

        Assert.True(isRemoved);
        Assert.Equal(0, removedValue);
        Assert.Empty(biDictionary);
        Assert.Empty(biDictionary.Keys);
        Assert.Empty(biDictionary.Values);
        Assert.Empty(biDictionary.Inverse.Keys);
        Assert.Empty(biDictionary.Inverse.Values);
    }

    [Theory]
    [Trait("Method", null)]
    [InlineData('b')]
    [InlineData('c')]
    public void Remove_FilledBiDictionaryAndMissingKey_ReturnsFalse(char key)
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var isRemoved = biDictionary.Remove(key);

        Assert.False(isRemoved);

        // checking that biDictionary has not changed
        Assert.Single(biDictionary, new KeyValuePair<char, int>('a', 0));
        Assert.Single(biDictionary.Keys, 'a');
        Assert.Single(biDictionary.Values, 0);
        Assert.Single(biDictionary.Inverse, new KeyValuePair<int, char>(0, 'a'));
        Assert.Single(biDictionary.Inverse.Keys, 0);
        Assert.Single(biDictionary.Inverse.Values, 'a');
    }

    [Fact]
    [Trait("Method", null)]
    public void Remove_EmptyBiDictionaryAndNullKey_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714 // Тип не может быть использован как параметр типа в универсальном типе или методе. Допустимость значения NULL для аргумента типа не соответствует ограничению "notnull".
        var biDictionary = new BidirectionalDictionary<char?, int>();
#pragma warning restore CS8714 // Тип не может быть использован как параметр типа в универсальном типе или методе. Допустимость значения NULL для аргумента типа не соответствует ограничению "notnull".

        var key = (char?)null;

        Assert.Throws<ArgumentNullException>(() => biDictionary.Remove(key));

        // checking that biDictionary has not changed
        Assert.Empty(biDictionary);
        Assert.Empty(biDictionary.Keys);
        Assert.Empty(biDictionary.Values);
        Assert.Empty(biDictionary.Inverse.Keys);
        Assert.Empty(biDictionary.Inverse.Values);
    }

    [Fact]
    [Trait("Method", null)]
    public void Clear_FilledBiDictionary_RemovesAllEntriesSuccessfully()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        biDictionary.Clear();

        Assert.Empty(biDictionary);
        Assert.Empty(biDictionary.Keys);
        Assert.Empty(biDictionary.Values);
        Assert.Empty(biDictionary.Inverse.Keys);
        Assert.Empty(biDictionary.Inverse.Values);
    }

    [Theory]
    [Trait("Method", null)]
    [InlineData('a', true)]
    [InlineData('b', false)]
    [InlineData('c', false)]
    public void ContainsKey_FilledBiDictionaryAndExistingKey_ReturnsExpectedResult(char key, bool expectedResult)
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var isExists = biDictionary.ContainsKey(key);

        Assert.Equal(expectedResult, isExists);
    }

    [Fact]
    [Trait("Method", null)]
    public void ContainsKey_FilledBiDictionaryAndNullKey_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714 // Тип не может быть использован как параметр типа в универсальном типе или методе. Допустимость значения NULL для аргумента типа не соответствует ограничению "notnull".
        var biDictionary = new BidirectionalDictionary<char?, int>()
        {
            { 'a', 0 },
        };
#pragma warning restore CS8714 // Тип не может быть использован как параметр типа в универсальном типе или методе. Допустимость значения NULL для аргумента типа не соответствует ограничению "notnull".

        var key = (char?)null;

        Assert.Throws<ArgumentNullException>(() => biDictionary.ContainsKey(key));
    }

    [Theory]
    [Trait("Method", null)]
    [InlineData(0, true)]
    [InlineData(1, false)]
    [InlineData(2, false)]
    public void ContainsValue_FilledBiDictionaryAndExistingKey_ReturnsExpectedResult(int value, bool expectedResult)
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var isExists = biDictionary.ContainsValue(value);

        Assert.Equal(expectedResult, isExists);
    }

    [Fact]
    [Trait("Method", null)]
    public void ContainsValue_FilledBiDictionaryAndNullKey_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714 // Тип не может быть использован как параметр типа в универсальном типе или методе. Допустимость значения NULL для аргумента типа не соответствует ограничению "notnull".
        var biDictionary = new BidirectionalDictionary<char, int?>()
        {
            { 'a', 0 },
        };
#pragma warning restore CS8714 // Тип не может быть использован как параметр типа в универсальном типе или методе. Допустимость значения NULL для аргумента типа не соответствует ограничению "notnull".

        var value = (char?)null;

        Assert.Throws<ArgumentNullException>(() => biDictionary.ContainsValue(value));
    }

    [Theory]
    [Trait("Method", null)]
    [InlineData('a', 0)]
    [InlineData('b', 1)]
    public void TryAdd_EmptyBiDictionaryAndExistingKey_AddsEntrySuccessfullyAndReturnsTrue(char key, int value)
    {
        var biDictionary = new BidirectionalDictionary<char, int>();

        var isAdded = biDictionary.TryAdd(key, value);

        Assert.True(isAdded);
        Assert.Single(biDictionary, new KeyValuePair<char, int>(key, value));
        Assert.Single(biDictionary.Keys, key);
        Assert.Single(biDictionary.Values, value);
        Assert.Single(biDictionary.Inverse, new KeyValuePair<int, char>(value, key));
        Assert.Single(biDictionary.Inverse.Keys, value);
        Assert.Single(biDictionary.Inverse.Values, key);

    }

    [Theory]
    [Trait("Method", null)]
    [InlineData('a', 0)]
    [InlineData('a', 1)]
    [InlineData('b', 0)]
    public void TryAdd_FilledBiDictionaryAndMissingKey_ReturnsFalse(char key, int value)
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var isAdded = biDictionary.TryAdd(key, value);

        Assert.False(isAdded);

        // checking that biDictionary has not changed
        Assert.Single(biDictionary, new KeyValuePair<char, int>('a', 0));
        Assert.Single(biDictionary.Keys, 'a');
        Assert.Single(biDictionary.Values, 0);
        Assert.Single(biDictionary.Inverse, new KeyValuePair<int, char>(0, 'a'));
        Assert.Single(biDictionary.Inverse.Keys, 0);
        Assert.Single(biDictionary.Inverse.Values, 'a');
    }

    [Theory]
    [Trait("Method", null)]
    [InlineData(null, null)]
    [InlineData(null, 0)]
    [InlineData('a', null)]
    public void TryAdd_EmptyBiDictionaryAndNullKeyValue_ThrowsArgumentNullException(char? key, int? value)
    {
#pragma warning disable CS8714 // Тип не может быть использован как параметр типа в универсальном типе или методе. Допустимость значения NULL для аргумента типа не соответствует ограничению "notnull".
        var biDictionary = new BidirectionalDictionary<char?, int?>();
#pragma warning restore CS8714 // Тип не может быть использован как параметр типа в универсальном типе или методе. Допустимость значения NULL для аргумента типа не соответствует ограничению "notnull".

        Assert.Throws<ArgumentNullException>(() => _ = biDictionary.TryAdd(key, value));

        // checking that biDictionary has not changed
        Assert.Empty(biDictionary);
        Assert.Empty(biDictionary.Keys);
        Assert.Empty(biDictionary.Values);
        Assert.Empty(biDictionary.Inverse.Keys);
        Assert.Empty(biDictionary.Inverse.Values);
    }

    [Fact]
    [Trait("Method", null)]
    public void TryGetValue_FilledBiDictionaryAndExistingKey_ReturnsTrueAndReturnsOutExpectedValue()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 }
        };

        var key           = 'a';
        var expectedValue = 0;

        var isExists = biDictionary.TryGetValue(key, out var value);

        Assert.True(isExists);
        Assert.Equal(expectedValue, value);

    }

    [Theory]
    [Trait("Method", null)]
    [InlineData('a')]
    [InlineData('b')]
    public void TryGetValue_EmptyBiDictionaryAndMissingKey_ReturnsFalse(char key)
    {
        var biDictionary = new BidirectionalDictionary<char, int>();

        var isExists = biDictionary.TryGetValue(key, out _);

        Assert.False(isExists);
    }

    [Fact]
    [Trait("Method", null)]
    public void TryGetValue_EmptyBiDictionaryAndNullKey_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714 // Тип не может быть использован как параметр типа в универсальном типе или методе. Допустимость значения NULL для аргумента типа не соответствует ограничению "notnull".
        var biDictionary = new BidirectionalDictionary<char?, int?>();
#pragma warning restore CS8714 // Тип не может быть использован как параметр типа в универсальном типе или методе. Допустимость значения NULL для аргумента типа не соответствует ограничению "notnull".

        var key = (char?)null;

        Assert.Throws<ArgumentNullException>(() => _ = biDictionary.TryGetValue(key, out _));
    }

    #endregion

    #region Indexer tests

    [Fact]
    [Trait("Indexer", null)]
    public void Indexer_Get_FilledBiDictionaryAndExistingKey_ReturnsExpectedValue()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var key           = 'a';
        var expectedValue = 0;

        var value = biDictionary[key];

        Assert.Equal(expectedValue, value);
    }

    [Fact]
    [Trait("Indexer", null)]
    public void Indexer_Get_EmptyBiDictionaryAndNullKey_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714 // Тип не может быть использован как параметр типа в универсальном типе или методе. Допустимость значения NULL для аргумента типа не соответствует ограничению "notnull".
        var biDictionary = new BidirectionalDictionary<char?, int>();
#pragma warning restore CS8714 // Тип не может быть использован как параметр типа в универсальном типе или методе. Допустимость значения NULL для аргумента типа не соответствует ограничению "notnull".

        var key = (char?)null;

        Assert.Throws<ArgumentNullException>(() => _ = biDictionary[key]);
    }

    [Theory]
    [Trait("Indexer", null)]
    [InlineData('a')]
    [InlineData('b')]
    public void Indexer_Get_EmptyBiDictionaryAndMissingKey_ThrowsKeyNotFoundException(char key)
    {
        var biDictionary = new BidirectionalDictionary<char, int>();

        Assert.Throws<KeyNotFoundException>(() => _ = biDictionary[key]);
    }

    [Theory]
    [Trait("Indexer", null)]
    [InlineData('a', 0)]
    [InlineData('a', 1)]
    public void Indexer_Set_FilledBiDictionaryAndExistingKeyAndNonDuplicateValue_UpdateValueSuccessfully(char key, int value)
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        biDictionary[key] = value;

        Assert.Single(biDictionary, new KeyValuePair<char, int>(key, value));
        Assert.Single(biDictionary.Keys, key);
        Assert.Single(biDictionary.Values, value);
        Assert.Single(biDictionary.Inverse, new KeyValuePair<int, char>(value, key));
        Assert.Single(biDictionary.Inverse.Keys, value);
        Assert.Single(biDictionary.Inverse.Values, key);
    }

    [Fact]
    [Trait("Indexer", null)]
    public void Indexer_Set_EmptyBiDictionaryAndNullKeyAndNonDuplicateValue_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714 // Тип не может быть использован как параметр типа в универсальном типе или методе. Допустимость значения NULL для аргумента типа не соответствует ограничению "notnull".
        var biDictionary = new BidirectionalDictionary<char?, int>();
#pragma warning restore CS8714 // Тип не может быть использован как параметр типа в универсальном типе или методе. Допустимость значения NULL для аргумента типа не соответствует ограничению "notnull".

        var key   = (char?)null;
        var value = 0;

        Assert.Throws<ArgumentNullException>(() => biDictionary[key] = value);
    }

    [Fact]
    [Trait("Indexer", null)]
    public void Indexer_Set_FilledBiDictionaryAndExistingKeyAndNullValue_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714 // Тип не может быть использован как параметр типа в универсальном типе или методе. Допустимость значения NULL для аргумента типа не соответствует ограничению "notnull".
        var biDictionary = new BidirectionalDictionary<char, int?>()
        {
            { 'a', 0 }
        };
#pragma warning restore CS8714 // Тип не может быть использован как параметр типа в универсальном типе или методе. Допустимость значения NULL для аргумента типа не соответствует ограничению "notnull".

        var key   = 'a';
        var value = (int?)null;

        Assert.Throws<ArgumentNullException>(() => biDictionary[key] = value);
    }

    [Theory]
    [Trait("Indexer", null)]
    [InlineData('a', 1)]
    [InlineData('b', 0)]
    [InlineData('c', 0)]
    [InlineData('c', 1)]
    public void Indexer_Set_FilledBiDictionaryAndNotNullKeyAndDuplicateValue_ThrowsArgumentException(char key, int value)
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        Assert.Throws<ArgumentException>(() => biDictionary[key] = value);

        // checking that biDictionary has not changed
        Assert.Single(biDictionary, new KeyValuePair<char, int>('a', 0));
        Assert.Single(biDictionary.Keys, 'a');
        Assert.Single(biDictionary.Values, 0);
        Assert.Single(biDictionary.Inverse, new KeyValuePair<int, char>(0, 'a'));
        Assert.Single(biDictionary.Inverse.Keys, 0);
        Assert.Single(biDictionary.Inverse.Values, 'a');
    }

    [Fact]
    [Trait("Indexer", null)]
    public void Indexer_Set_EmptyBiDictionaryAndMissingKeyAndNonDuplicateValue_CreatesNewEntry()
    {
        var biDictionary = new BidirectionalDictionary<char, int>();
        var key          = 'a';
        var value        = 0;

        biDictionary[key] = value;

        Assert.Single(biDictionary, new KeyValuePair<char, int>('a', 0));
        Assert.Single(biDictionary.Keys, 'a');
        Assert.Single(biDictionary.Values, 0);
        Assert.Single(biDictionary.Inverse, new KeyValuePair<int, char>(0, 'a'));
        Assert.Single(biDictionary.Inverse.Keys, 0);
        Assert.Single(biDictionary.Inverse.Values, 'a');
    }

    #endregion
}