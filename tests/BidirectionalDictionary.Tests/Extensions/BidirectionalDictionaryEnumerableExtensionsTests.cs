namespace BidirectionalDictionary.Tests.Extensions;

public class BidirectionalDictionaryEnumerableExtensionsTests
{
    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_KeyValuePairsEnumerable_CreatesBiDictionaryFromSource()
    {
        var source = new List<KeyValuePair<char, int>>
        {
            new('a', 1),
            new('b', 2),
        };

        var biDictionary = source.ToBidirectionalDictionary();

        Assert.Equal(source, biDictionary);
        Assert.Equal(source.Select(pair => pair.Key), biDictionary.Keys);
        Assert.Equal(source.Select(pair => pair.Value), biDictionary.Values);
        Assert.Equal(source.Select(pair => pair.Key), biDictionary.Inverse.Values);
        Assert.Equal(source.Select(pair => pair.Value), biDictionary.Inverse.Keys);
        Assert.Equal(EqualityComparer<char>.Default, biDictionary.KeyComparer);
        Assert.Equal(EqualityComparer<int>.Default, biDictionary.ValueComparer);
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_KeyValuePairsEnumerableAndCustomComparers_CreatesBiDictionaryWithExpectedComparers()
    {
        var source = new List<KeyValuePair<string, string>>
        {
            new("a", "x"),
            new("b", "y"),
        };

        var keyComparer   = StringComparer.OrdinalIgnoreCase;
        var valueComparer = StringComparer.Ordinal;

        var biDictionary = source.ToBidirectionalDictionary(keyComparer, valueComparer);

        Assert.Equal(source, biDictionary);
        Assert.Equal(keyComparer, biDictionary.KeyComparer);
        Assert.Equal(valueComparer, biDictionary.ValueComparer);
        Assert.Equal(keyComparer, biDictionary.Inverse.ValueComparer);
        Assert.Equal(valueComparer, biDictionary.Inverse.KeyComparer);
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_KeyValuePairsEnumerableAndNullSource_ThrowsArgumentNullException()
    {
        var source = (IEnumerable<KeyValuePair<char, int>>?)null;

        Assert.Throws<ArgumentNullException>(() => source!.ToBidirectionalDictionary());
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_TupleEnumerable_CreatesBiDictionaryFromSource()
    {
        var source = new List<(char Key, int Value)>
        {
            ('a', 1),
            ('b', 2),
        };

        var biDictionary = source.ToBidirectionalDictionary();

        Assert.Equal(source.Select(pair => pair.Key), biDictionary.Keys);
        Assert.Equal(source.Select(pair => pair.Value), biDictionary.Values);
        Assert.Equal(source.Select(pair => pair.Key), biDictionary.Inverse.Values);
        Assert.Equal(source.Select(pair => pair.Value), biDictionary.Inverse.Keys);
        Assert.Equal(EqualityComparer<char>.Default, biDictionary.KeyComparer);
        Assert.Equal(EqualityComparer<int>.Default, biDictionary.ValueComparer);
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_TupleEnumerableAndNullSource_ThrowsArgumentNullException()
    {
        var source = (IEnumerable<(char Key, int Value)>?)null;

        Assert.Throws<ArgumentNullException>(() => source!.ToBidirectionalDictionary());
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceAndKeySelector_CreatesBiDictionaryFromSource()
    {
        var source = new[] { "a", "bb" };

        var biDictionary = source.ToBidirectionalDictionary(value => value.Length);

        Assert.Equal(2, biDictionary.Count);
        Assert.Contains(new KeyValuePair<int, string>(1, "a"), biDictionary);
        Assert.Contains(new KeyValuePair<int, string>(2, "bb"), biDictionary);
        Assert.Contains(new KeyValuePair<string, int>("a", 1), biDictionary.Inverse);
        Assert.Contains(new KeyValuePair<string, int>("bb", 2), biDictionary.Inverse);
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceAndKeySelectorAndNullSource_ThrowsArgumentNullException()
    {
        var source = (IEnumerable<string>?)null;

        Assert.Throws<ArgumentNullException>(() => source!.ToBidirectionalDictionary(value => value.Length));
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceAndKeySelectorAndNullKeySelector_ThrowsArgumentNullException()
    {
        var source      = new[] { "a" };
        var keySelector = (Func<string, int>?)null;

        Assert.Throws<ArgumentNullException>(() => source.ToBidirectionalDictionary(keySelector!));
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceAndKeySelectorAndCustomComparers_CreatesBiDictionaryWithExpectedComparers()
    {
        var source = new[] { "a", "bb" };

        var keyComparer   = EqualityComparer<int>.Default;
        var valueComparer = StringComparer.OrdinalIgnoreCase;

        var biDictionary = source.ToBidirectionalDictionary(value => value.Length, keyComparer, valueComparer);

        Assert.Equal(keyComparer, biDictionary.KeyComparer);
        Assert.Equal(valueComparer, biDictionary.ValueComparer);
        Assert.Equal(keyComparer, biDictionary.Inverse.ValueComparer);
        Assert.Equal(valueComparer, biDictionary.Inverse.KeyComparer);
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceListAndKeySelectorAndCustomComparers_CreatesBiDictionaryFromSource()
    {
        var source = new List<string> { "a", "bb" };

        var biDictionary = source.ToBidirectionalDictionary(
            value => value.Length,
            EqualityComparer<int>.Default,
            StringComparer.Ordinal);

        Assert.Contains(new KeyValuePair<int, string>(1, "a"), biDictionary);
        Assert.Contains(new KeyValuePair<int, string>(2, "bb"), biDictionary);
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceQueueAndKeySelectorAndCustomComparers_CreatesBiDictionaryFromSource()
    {
        var source = new Queue<string>(new[] { "a", "bb" });

        var biDictionary = source.ToBidirectionalDictionary(
            value => value.Length,
            EqualityComparer<int>.Default,
            StringComparer.Ordinal);

        Assert.Contains(new KeyValuePair<int, string>(1, "a"), biDictionary);
        Assert.Contains(new KeyValuePair<int, string>(2, "bb"), biDictionary);
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceAndSelectors_CreatesBiDictionaryFromSource()
    {
        var source = new[] { "1:a", "2:bb" };

        var biDictionary = source.ToBidirectionalDictionary(
            value => int.Parse(value[..1]),
            value => value[2..]);

        Assert.Equal(2, biDictionary.Count);
        Assert.Contains(new KeyValuePair<int, string>(1, "a"), biDictionary);
        Assert.Contains(new KeyValuePair<int, string>(2, "bb"), biDictionary);
        Assert.Contains(new KeyValuePair<string, int>("a", 1), biDictionary.Inverse);
        Assert.Contains(new KeyValuePair<string, int>("bb", 2), biDictionary.Inverse);
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceAndSelectorsAndNullSource_ThrowsArgumentNullException()
    {
        var source = (IEnumerable<string>?)null;

        Assert.Throws<ArgumentNullException>(
            () => source!.ToBidirectionalDictionary(
                value => int.Parse(value[..1]),
                value => value[2..]));
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceAndSelectorsAndNullKeySelector_ThrowsArgumentNullException()
    {
        var source          = new[] { "1:a" };
        var keySelector     = (Func<string, int>?)null;
        var elementSelector = (Func<string, string>)(value => value[2..]);

        Assert.Throws<ArgumentNullException>(() => source.ToBidirectionalDictionary(keySelector!, elementSelector));
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceAndSelectorsAndNullElementSelector_ThrowsArgumentNullException()
    {
        var source          = new[] { "1:a" };
        var keySelector     = (Func<string, int>)(value => int.Parse(value[..1]));
        var elementSelector = (Func<string, string>?)null;

        Assert.Throws<ArgumentNullException>(() => source.ToBidirectionalDictionary(keySelector, elementSelector!));
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceAndSelectorsAndCustomComparers_CreatesBiDictionaryWithExpectedComparers()
    {
        var source = new[] { "1:a", "2:bb" };

        var keyComparer   = EqualityComparer<int>.Default;
        var valueComparer = StringComparer.OrdinalIgnoreCase;

        var biDictionary = source.ToBidirectionalDictionary(
            value => int.Parse(value[..1]),
            value => value[2..],
            keyComparer,
            valueComparer);

        Assert.Equal(keyComparer, biDictionary.KeyComparer);
        Assert.Equal(valueComparer, biDictionary.ValueComparer);
        Assert.Equal(keyComparer, biDictionary.Inverse.ValueComparer);
        Assert.Equal(valueComparer, biDictionary.Inverse.KeyComparer);
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceQueueAndSelectorsAndCustomComparers_CreatesBiDictionaryFromSource()
    {
        var source = new Queue<string>(new[] { "1:a", "2:bb" });

        var biDictionary = source.ToBidirectionalDictionary(
            value => int.Parse(value[..1]),
            value => value[2..],
            EqualityComparer<int>.Default,
            StringComparer.Ordinal);

        Assert.Contains(new KeyValuePair<int, string>(1, "a"), biDictionary);
        Assert.Contains(new KeyValuePair<int, string>(2, "bb"), biDictionary);
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_KeyValuePairsEnumerableAndEmptySource_ReturnsEmptyBiDictionary()
    {
        var source = Array.Empty<KeyValuePair<char, int>>();

        var biDictionary = source.ToBidirectionalDictionary();

        Assert.Empty(biDictionary);
        Assert.Empty(biDictionary.Inverse);
    }

    [Fact]
    [Trait("Extension", null)]
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
    [Trait("Extension", null)]
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
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_KeyValuePairsEnumerableAndCustomComparersAndNullSource_ThrowsArgumentNullException()
    {
        var source = (IEnumerable<KeyValuePair<char, int>>?)null;

        Assert.Throws<ArgumentNullException>(
            () => source!.ToBidirectionalDictionary(EqualityComparer<char>.Default, EqualityComparer<int>.Default));
    }

    [Fact]
    [Trait("Extension", null)]
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
    [Trait("Extension", null)]
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
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_TupleEnumerableAndEmptySource_ReturnsEmptyBiDictionary()
    {
        var source = Array.Empty<(char Key, int Value)>();

        var biDictionary = source.ToBidirectionalDictionary();

        Assert.Empty(biDictionary);
        Assert.Empty(biDictionary.Inverse);
    }

    [Fact]
    [Trait("Extension", null)]
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
    [Trait("Extension", null)]
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
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_TupleEnumerableAndCustomComparersAndNullSource_ThrowsArgumentNullException()
    {
        var source = (IEnumerable<(char Key, int Value)>?)null;

        Assert.Throws<ArgumentNullException>(
            () => source!.ToBidirectionalDictionary(EqualityComparer<char>.Default, EqualityComparer<int>.Default));
    }

    [Fact]
    [Trait("Extension", null)]
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
    [Trait("Extension", null)]
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
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceAndKeySelectorAndEmptySource_ReturnsEmptyBiDictionary()
    {
        var source = Array.Empty<string>();

        var biDictionary = source.ToBidirectionalDictionary(value => value.Length);

        Assert.Empty(biDictionary);
        Assert.Empty(biDictionary.Inverse);
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceAndKeySelectorAndDuplicateKey_ThrowsArgumentException()
    {
        var source = new[] { "aa", "ab" };

        Assert.Throws<ArgumentException>(() => source.ToBidirectionalDictionary(value => value[0]));
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceAndKeySelectorAndCustomComparersAndNullSource_ThrowsArgumentNullException()
    {
        var source = (IEnumerable<string>?)null;

        Assert.Throws<ArgumentNullException>(
            () => source!.ToBidirectionalDictionary(value => value.Length, EqualityComparer<int>.Default, StringComparer.Ordinal));
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceAndKeySelectorAndCustomComparersAndNullKeySelector_ThrowsArgumentNullException()
    {
        var source      = new[] { "a" };
        var keySelector = (Func<string, int>?)null;

        Assert.Throws<ArgumentNullException>(
            () => source.ToBidirectionalDictionary(keySelector!, EqualityComparer<int>.Default, StringComparer.Ordinal));
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceAndKeySelectorAndCustomComparersAndDuplicateKeyByComparer_ThrowsArgumentException()
    {
        var source = new[] { "a", "A" };

        Assert.Throws<ArgumentException>(
            () => source.ToBidirectionalDictionary(value => value, StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal));
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceAndKeySelectorAndCustomComparersAndDuplicateValueByComparer_ThrowsArgumentException()
    {
        var source = new[] { "a", "A" };
        var key    = 0;

        Assert.Throws<ArgumentException>(
            () => source.ToBidirectionalDictionary(_ => key++, EqualityComparer<int>.Default, StringComparer.OrdinalIgnoreCase));
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceAndSelectorsAndEmptySource_ReturnsEmptyBiDictionary()
    {
        var source = Array.Empty<string>();

        var biDictionary = source.ToBidirectionalDictionary(value => value, value => value);

        Assert.Empty(biDictionary);
        Assert.Empty(biDictionary.Inverse);
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceAndSelectorsAndDuplicateKey_ThrowsArgumentException()
    {
        var source = new[] { "a", "A" };

        Assert.Throws<ArgumentException>(
            () => source.ToBidirectionalDictionary(
                value => value.ToLowerInvariant(),
                value => value));
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceAndSelectorsAndDuplicateValue_ThrowsArgumentException()
    {
        var source = new[] { "a1", "b1" };

        Assert.Throws<ArgumentException>(
            () => source.ToBidirectionalDictionary(value => value[0], value => value[1]));
    }

    [Fact]
    [Trait("Extension", null)]
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
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceAndSelectorsAndCustomComparersAndNullKeySelector_ThrowsArgumentNullException()
    {
        var source          = new[] { "a" };
        var keySelector     = (Func<string, string>?)null;
        var elementSelector = (Func<string, string>)(value => value);

        Assert.Throws<ArgumentNullException>(
            () => source.ToBidirectionalDictionary(
                keySelector!,
                elementSelector,
                StringComparer.Ordinal,
                StringComparer.Ordinal));
    }

    [Fact]
    [Trait("Extension", null)]
    public void ToBidirectionalDictionary_SourceAndSelectorsAndCustomComparersAndNullElementSelector_ThrowsArgumentNullException()
    {
        var source          = new[] { "a" };
        var keySelector     = (Func<string, string>)(value => value);
        var elementSelector = (Func<string, string>?)null;

        Assert.Throws<ArgumentNullException>(() => source.ToBidirectionalDictionary(
            keySelector,
            elementSelector!,
            StringComparer.Ordinal,
            StringComparer.Ordinal));
    }

    [Fact]
    [Trait("Extension", null)]
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
    [Trait("Extension", null)]
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
