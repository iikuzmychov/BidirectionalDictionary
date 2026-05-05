namespace BidirectionalDictionary.Tests.Extensions;

public class BidirectionalDictionaryEnumerableExtensionsTests
{
    [Fact]
    public void ToBidirectionalDictionary_KeyValuePairsEnumerable_CreatesBidirectionalDictionaryFromSource()
    {
        var source = new List<KeyValuePair<char, int>>
        {
            new('a', 1),
            new('b', 2),
        };

        var bidirectionalDictionary = source.ToBidirectionalDictionary();

        Assert.Equal(source, bidirectionalDictionary);
        Assert.Equal(source.Select(pair => pair.Key), bidirectionalDictionary.Keys);
        Assert.Equal(source.Select(pair => pair.Value), bidirectionalDictionary.Values);
        Assert.Equal(source.Select(pair => pair.Key), bidirectionalDictionary.Inverse.Values);
        Assert.Equal(source.Select(pair => pair.Value), bidirectionalDictionary.Inverse.Keys);
        Assert.Equal(EqualityComparer<char>.Default, bidirectionalDictionary.KeyComparer);
        Assert.Equal(EqualityComparer<int>.Default, bidirectionalDictionary.ValueComparer);
    }

    [Fact]
    public void ToBidirectionalDictionary_KeyValuePairsEnumerableAndCustomComparers_CreatesBidirectionalDictionaryWithExpectedComparers()
    {
        var source = new List<KeyValuePair<string, string>>
        {
            new("a", "x"),
            new("b", "y"),
        };

        var keyComparer = StringComparer.OrdinalIgnoreCase;
        var valueComparer = StringComparer.Ordinal;

        var bidirectionalDictionary = source.ToBidirectionalDictionary(keyComparer, valueComparer);

        Assert.Equal(source, bidirectionalDictionary);
        Assert.Equal(keyComparer, bidirectionalDictionary.KeyComparer);
        Assert.Equal(valueComparer, bidirectionalDictionary.ValueComparer);
        Assert.Equal(keyComparer, bidirectionalDictionary.Inverse.ValueComparer);
        Assert.Equal(valueComparer, bidirectionalDictionary.Inverse.KeyComparer);
    }

    [Fact]
    public void ToBidirectionalDictionary_KeyValuePairsEnumerableAndNullSource_ThrowsArgumentNullException()
    {
        var source = (IEnumerable<KeyValuePair<char, int>>?)null;

        Assert.Throws<ArgumentNullException>(() => source!.ToBidirectionalDictionary());
    }

    [Fact]
    public void ToBidirectionalDictionary_TupleEnumerable_CreatesBidirectionalDictionaryFromSource()
    {
        var source = new List<(char Key, int Value)>
        {
            ('a', 1),
            ('b', 2),
        };

        var bidirectionalDictionary = source.ToBidirectionalDictionary();

        Assert.Equal(source.Select(pair => pair.Key), bidirectionalDictionary.Keys);
        Assert.Equal(source.Select(pair => pair.Value), bidirectionalDictionary.Values);
        Assert.Equal(source.Select(pair => pair.Key), bidirectionalDictionary.Inverse.Values);
        Assert.Equal(source.Select(pair => pair.Value), bidirectionalDictionary.Inverse.Keys);
        Assert.Equal(EqualityComparer<char>.Default, bidirectionalDictionary.KeyComparer);
        Assert.Equal(EqualityComparer<int>.Default, bidirectionalDictionary.ValueComparer);
    }

    [Fact]
    public void ToBidirectionalDictionary_TupleEnumerableAndNullSource_ThrowsArgumentNullException()
    {
        var source = (IEnumerable<(char Key, int Value)>?)null;

        Assert.Throws<ArgumentNullException>(() => source!.ToBidirectionalDictionary());
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndKeySelector_CreatesBidirectionalDictionaryFromSource()
    {
        var source = new[] { "a", "bb" };

        var bidirectionalDictionary = source.ToBidirectionalDictionary(value => value.Length);

        Assert.Equal(2, bidirectionalDictionary.Count);
        Assert.Contains(new KeyValuePair<int, string>(1, "a"), bidirectionalDictionary);
        Assert.Contains(new KeyValuePair<int, string>(2, "bb"), bidirectionalDictionary);
        Assert.Contains(new KeyValuePair<string, int>("a", 1), bidirectionalDictionary.Inverse);
        Assert.Contains(new KeyValuePair<string, int>("bb", 2), bidirectionalDictionary.Inverse);
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndKeySelectorAndNullSource_ThrowsArgumentNullException()
    {
        var source = (IEnumerable<string>?)null;

        Assert.Throws<ArgumentNullException>(() => source!.ToBidirectionalDictionary(value => value.Length));
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndKeySelectorAndNullKeySelector_ThrowsArgumentNullException()
    {
        var source = new[] { "a" };
        var keySelector = (Func<string, int>?)null;

        Assert.Throws<ArgumentNullException>(() => source.ToBidirectionalDictionary(keySelector!));
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndKeySelectorAndCustomComparers_CreatesBidirectionalDictionaryWithExpectedComparers()
    {
        var source = new[] { "a", "bb" };

        var keyComparer = EqualityComparer<int>.Default;
        var valueComparer = StringComparer.OrdinalIgnoreCase;

        var bidirectionalDictionary = source.ToBidirectionalDictionary(value => value.Length, keyComparer, valueComparer);

        Assert.Equal(keyComparer, bidirectionalDictionary.KeyComparer);
        Assert.Equal(valueComparer, bidirectionalDictionary.ValueComparer);
        Assert.Equal(keyComparer, bidirectionalDictionary.Inverse.ValueComparer);
        Assert.Equal(valueComparer, bidirectionalDictionary.Inverse.KeyComparer);
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceListAndKeySelectorAndCustomComparers_CreatesBidirectionalDictionaryFromSource()
    {
        var source = new List<string> { "a", "bb" };

        var bidirectionalDictionary = source.ToBidirectionalDictionary(
            value => value.Length,
            EqualityComparer<int>.Default,
            StringComparer.Ordinal);

        Assert.Contains(new KeyValuePair<int, string>(1, "a"), bidirectionalDictionary);
        Assert.Contains(new KeyValuePair<int, string>(2, "bb"), bidirectionalDictionary);
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceQueueAndKeySelectorAndCustomComparers_CreatesBidirectionalDictionaryFromSource()
    {
        var source = new Queue<string>(new[] { "a", "bb" });

        var bidirectionalDictionary = source.ToBidirectionalDictionary(
            value => value.Length,
            EqualityComparer<int>.Default,
            StringComparer.Ordinal);

        Assert.Contains(new KeyValuePair<int, string>(1, "a"), bidirectionalDictionary);
        Assert.Contains(new KeyValuePair<int, string>(2, "bb"), bidirectionalDictionary);
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndSelectors_CreatesBidirectionalDictionaryFromSource()
    {
        var source = new[] { "1:a", "2:bb" };

        var bidirectionalDictionary = source.ToBidirectionalDictionary(
            value => int.Parse(value[..1]),
            value => value[2..]);

        Assert.Equal(2, bidirectionalDictionary.Count);
        Assert.Contains(new KeyValuePair<int, string>(1, "a"), bidirectionalDictionary);
        Assert.Contains(new KeyValuePair<int, string>(2, "bb"), bidirectionalDictionary);
        Assert.Contains(new KeyValuePair<string, int>("a", 1), bidirectionalDictionary.Inverse);
        Assert.Contains(new KeyValuePair<string, int>("bb", 2), bidirectionalDictionary.Inverse);
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndSelectorsAndNullSource_ThrowsArgumentNullException()
    {
        var source = (IEnumerable<string>?)null;

        Assert.Throws<ArgumentNullException>(
            () => source!.ToBidirectionalDictionary(
                value => int.Parse(value[..1]),
                value => value[2..]));
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndSelectorsAndNullKeySelector_ThrowsArgumentNullException()
    {
        var source = new[] { "1:a" };
        var keySelector = (Func<string, int>?)null;
        var elementSelector = (Func<string, string>)(value => value[2..]);

        Assert.Throws<ArgumentNullException>(() => source.ToBidirectionalDictionary(keySelector!, elementSelector));
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndSelectorsAndNullElementSelector_ThrowsArgumentNullException()
    {
        var source = new[] { "1:a" };
        var keySelector = (Func<string, int>)(value => int.Parse(value[..1]));
        var elementSelector = (Func<string, string>?)null;

        Assert.Throws<ArgumentNullException>(() => source.ToBidirectionalDictionary(keySelector, elementSelector!));
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndSelectorsAndCustomComparers_CreatesBidirectionalDictionaryWithExpectedComparers()
    {
        var source = new[] { "1:a", "2:bb" };

        var keyComparer = EqualityComparer<int>.Default;
        var valueComparer = StringComparer.OrdinalIgnoreCase;

        var bidirectionalDictionary = source.ToBidirectionalDictionary(
            value => int.Parse(value[..1]),
            value => value[2..],
            keyComparer,
            valueComparer);

        Assert.Equal(keyComparer, bidirectionalDictionary.KeyComparer);
        Assert.Equal(valueComparer, bidirectionalDictionary.ValueComparer);
        Assert.Equal(keyComparer, bidirectionalDictionary.Inverse.ValueComparer);
        Assert.Equal(valueComparer, bidirectionalDictionary.Inverse.KeyComparer);
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceQueueAndSelectorsAndCustomComparers_CreatesBidirectionalDictionaryFromSource()
    {
        var source = new Queue<string>(new[] { "1:a", "2:bb" });

        var bidirectionalDictionary = source.ToBidirectionalDictionary(
            value => int.Parse(value[..1]),
            value => value[2..],
            EqualityComparer<int>.Default,
            StringComparer.Ordinal);

        Assert.Contains(new KeyValuePair<int, string>(1, "a"), bidirectionalDictionary);
        Assert.Contains(new KeyValuePair<int, string>(2, "bb"), bidirectionalDictionary);
    }

    [Fact]
    public void ToBidirectionalDictionary_KeyValuePairsEnumerableAndEmptySource_ReturnsEmptyBidirectionalDictionary()
    {
        var source = Array.Empty<KeyValuePair<char, int>>();

        var bidirectionalDictionary = source.ToBidirectionalDictionary();

        Assert.Empty(bidirectionalDictionary);
        Assert.Empty(bidirectionalDictionary.Inverse);
    }

    [Fact]
    public void ToBidirectionalDictionary_KeyValuePairsEnumerableAndDuplicateKey_ThrowsArgumentException()
    {
        var source = new List<KeyValuePair<char, int>>
        {
            new('a', 1),
            new('a', 2),
        };

        Assert.Throws<ArgumentException>(() => source.ToBidirectionalDictionary());
    }

    [Fact]
    public void ToBidirectionalDictionary_KeyValuePairsEnumerableAndDuplicateValue_ThrowsArgumentException()
    {
        var source = new List<KeyValuePair<char, int>>
        {
            new('a', 1),
            new('b', 1),
        };

        Assert.Throws<ArgumentException>(() => source.ToBidirectionalDictionary());
    }

    [Fact]
    public void ToBidirectionalDictionary_KeyValuePairsEnumerableAndCustomComparersAndNullSource_ThrowsArgumentNullException()
    {
        var source = (IEnumerable<KeyValuePair<char, int>>?)null;

        Assert.Throws<ArgumentNullException>(
            () => source!.ToBidirectionalDictionary(EqualityComparer<char>.Default, EqualityComparer<int>.Default));
    }

    [Fact]
    public void ToBidirectionalDictionary_KeyValuePairsEnumerableAndCustomComparersAndDuplicateKeyByComparer_ThrowsArgumentException()
    {
        var source = new List<KeyValuePair<string, int>>
        {
            new("a", 1),
            new("A", 2),
        };

        Assert.Throws<ArgumentException>(
            () => source.ToBidirectionalDictionary(StringComparer.OrdinalIgnoreCase, EqualityComparer<int>.Default));
    }

    [Fact]
    public void ToBidirectionalDictionary_KeyValuePairsEnumerableAndCustomComparersAndDuplicateValueByComparer_ThrowsArgumentException()
    {
        var source = new List<KeyValuePair<int, string>>
        {
            new(1, "x"),
            new(2, "X"),
        };

        Assert.Throws<ArgumentException>(
            () => source.ToBidirectionalDictionary(EqualityComparer<int>.Default, StringComparer.OrdinalIgnoreCase));
    }

    [Fact]
    public void ToBidirectionalDictionary_TupleEnumerableAndEmptySource_ReturnsEmptyBidirectionalDictionary()
    {
        var source = Array.Empty<(char Key, int Value)>();

        var bidirectionalDictionary = source.ToBidirectionalDictionary();

        Assert.Empty(bidirectionalDictionary);
        Assert.Empty(bidirectionalDictionary.Inverse);
    }

    [Fact]
    public void ToBidirectionalDictionary_TupleEnumerableAndDuplicateKey_ThrowsArgumentException()
    {
        var source = new List<(char Key, int Value)>
        {
            ('a', 1),
            ('a', 2),
        };

        Assert.Throws<ArgumentException>(() => source.ToBidirectionalDictionary());
    }

    [Fact]
    public void ToBidirectionalDictionary_TupleEnumerableAndDuplicateValue_ThrowsArgumentException()
    {
        var source = new List<(char Key, int Value)>
        {
            ('a', 1),
            ('b', 1),
        };

        Assert.Throws<ArgumentException>(() => source.ToBidirectionalDictionary());
    }

    [Fact]
    public void ToBidirectionalDictionary_TupleEnumerableAndCustomComparersAndNullSource_ThrowsArgumentNullException()
    {
        var source = (IEnumerable<(char Key, int Value)>?)null;

        Assert.Throws<ArgumentNullException>(
            () => source!.ToBidirectionalDictionary(EqualityComparer<char>.Default, EqualityComparer<int>.Default));
    }

    [Fact]
    public void ToBidirectionalDictionary_TupleEnumerableAndCustomComparersAndDuplicateKeyByComparer_ThrowsArgumentException()
    {
        var source = new List<(string Key, int Value)>
        {
            ("a", 1),
            ("A", 2),
        };

        Assert.Throws<ArgumentException>(
            () => source.ToBidirectionalDictionary(StringComparer.OrdinalIgnoreCase, EqualityComparer<int>.Default));
    }

    [Fact]
    public void ToBidirectionalDictionary_TupleEnumerableAndCustomComparersAndDuplicateValueByComparer_ThrowsArgumentException()
    {
        var source = new List<(int Key, string Value)>
        {
            (1, "x"),
            (2, "X"),
        };

        Assert.Throws<ArgumentException>(
            () => source.ToBidirectionalDictionary(EqualityComparer<int>.Default, StringComparer.OrdinalIgnoreCase));
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndKeySelectorAndEmptySource_ReturnsEmptyBidirectionalDictionary()
    {
        var source = Array.Empty<string>();

        var bidirectionalDictionary = source.ToBidirectionalDictionary(value => value.Length);

        Assert.Empty(bidirectionalDictionary);
        Assert.Empty(bidirectionalDictionary.Inverse);
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndKeySelectorAndDuplicateKey_ThrowsArgumentException()
    {
        var source = new[] { "aa", "ab" };

        Assert.Throws<ArgumentException>(() => source.ToBidirectionalDictionary(value => value[0]));
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndKeySelectorAndCustomComparersAndNullSource_ThrowsArgumentNullException()
    {
        var source = (IEnumerable<string>?)null;

        Assert.Throws<ArgumentNullException>(
            () => source!.ToBidirectionalDictionary(value => value.Length, EqualityComparer<int>.Default, StringComparer.Ordinal));
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndKeySelectorAndCustomComparersAndNullKeySelector_ThrowsArgumentNullException()
    {
        var source = new[] { "a" };
        var keySelector = (Func<string, int>?)null;

        Assert.Throws<ArgumentNullException>(
            () => source.ToBidirectionalDictionary(keySelector!, EqualityComparer<int>.Default, StringComparer.Ordinal));
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndKeySelectorAndCustomComparersAndDuplicateKeyByComparer_ThrowsArgumentException()
    {
        var source = new[] { "a", "A" };

        Assert.Throws<ArgumentException>(
            () => source.ToBidirectionalDictionary(value => value, StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal));
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndKeySelectorAndCustomComparersAndDuplicateValueByComparer_ThrowsArgumentException()
    {
        var source = new[] { "a", "A" };
        var key = 0;

        Assert.Throws<ArgumentException>(
            () => source.ToBidirectionalDictionary(_ => key++, EqualityComparer<int>.Default, StringComparer.OrdinalIgnoreCase));
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndSelectorsAndEmptySource_ReturnsEmptyBidirectionalDictionary()
    {
        var source = Array.Empty<string>();

        var bidirectionalDictionary = source.ToBidirectionalDictionary(value => value, value => value);

        Assert.Empty(bidirectionalDictionary);
        Assert.Empty(bidirectionalDictionary.Inverse);
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndSelectorsAndDuplicateKey_ThrowsArgumentException()
    {
        var source = new[] { "a", "A" };

        Assert.Throws<ArgumentException>(
            () => source.ToBidirectionalDictionary(
                value => value.ToLowerInvariant(),
                value => value));
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndSelectorsAndDuplicateValue_ThrowsArgumentException()
    {
        var source = new[] { "a1", "b1" };

        Assert.Throws<ArgumentException>(
            () => source.ToBidirectionalDictionary(value => value[0], value => value[1]));
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndSelectorsAndCustomComparersAndNullSource_ThrowsArgumentNullException()
    {
        var source = (IEnumerable<string>?)null;

        Assert.Throws<ArgumentNullException>(
            () => source!.ToBidirectionalDictionary(
                value => value,
                value => value,
                StringComparer.Ordinal,
                StringComparer.Ordinal));
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndSelectorsAndCustomComparersAndNullKeySelector_ThrowsArgumentNullException()
    {
        var source = new[] { "a" };
        var keySelector = (Func<string, string>?)null;
        var elementSelector = (Func<string, string>)(value => value);

        Assert.Throws<ArgumentNullException>(
            () => source.ToBidirectionalDictionary(
                keySelector!,
                elementSelector,
                StringComparer.Ordinal,
                StringComparer.Ordinal));
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndSelectorsAndCustomComparersAndNullElementSelector_ThrowsArgumentNullException()
    {
        var source = new[] { "a" };
        var keySelector = (Func<string, string>)(value => value);
        var elementSelector = (Func<string, string>?)null;

        Assert.Throws<ArgumentNullException>(() => source.ToBidirectionalDictionary(
            keySelector,
            elementSelector!,
            StringComparer.Ordinal,
            StringComparer.Ordinal));
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndSelectorsAndCustomComparersAndDuplicateKeyByComparer_ThrowsArgumentException()
    {
        var source = new[] { "a", "A" };

        Assert.Throws<ArgumentException>(
            () => source.ToBidirectionalDictionary(
                value => value,
                value => value + "_suffix",
                StringComparer.OrdinalIgnoreCase,
                StringComparer.Ordinal));
    }

    [Fact]
    public void ToBidirectionalDictionary_SourceAndSelectorsAndCustomComparersAndDuplicateValueByComparer_ThrowsArgumentException()
    {
        var source = new[] { "1:a", "2:A" };

        Assert.Throws<ArgumentException>(
            () => source.ToBidirectionalDictionary(
                value => int.Parse(value[..1]),
                value => value[2..],
                EqualityComparer<int>.Default,
                StringComparer.OrdinalIgnoreCase));
    }
}
