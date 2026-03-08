using System.Collections.Generic;

#if NET6_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace System.Linq;

public static partial class Enumerable
{
    /// <summary>
    /// Creates a <see cref="BidirectionalDictionary{TKey,TValue}"/> from an <see cref="IEnumerable{T}"/> according to the default comparers for the key and value types.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys from elements of <paramref name="source"/></typeparam>
    /// <typeparam name="TValue">The type of the values from elements of <paramref name="source"/></typeparam>
    /// <param name="source">The <see cref="IEnumerable{T}"/> to create a <see cref="BidirectionalDictionary{TKey,TValue}"/> from.</param>
    /// <returns>A <see cref="BidirectionalDictionary{TKey,TValue}"/> that contains keys and values from <paramref name="source"/> and uses default comparers for the key and value types.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is a null reference.</exception>
    /// <exception cref="ArgumentException"><paramref name="source"/> contains one or more duplicate keys or values.</exception>
    public static BidirectionalDictionary<TKey, TValue> ToBidirectionalDictionary<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>> source)
        where TKey : notnull
        where TValue : notnull
    {
        return source.ToBidirectionalDictionary(keyComparer: null, valueComparer: null);
    }

    /// <summary>
    /// Creates a <see cref="BidirectionalDictionary{TKey,TValue}"/> from an <see cref="IEnumerable{T}"/> according to specified key and value comparers.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys from elements of <paramref name="source"/></typeparam>
    /// <typeparam name="TValue">The type of the values from elements of <paramref name="source"/></typeparam>
    /// <param name="source">The <see cref="IEnumerable{T}"/> to create a <see cref="BidirectionalDictionary{TKey,TValue}"/> from.</param>
    /// <param name="keyComparer">An <see cref="IEqualityComparer{TKey}"/> to compare keys.</param>
    /// <param name="valueComparer">An <see cref="IEqualityComparer{TValue}"/> to compare values.</param>
    /// <returns>A <see cref="BidirectionalDictionary{TKey,TValue}"/> that contains keys and values from <paramref name="source"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is a null reference.</exception>
    /// <exception cref="ArgumentException"><paramref name="source"/> contains one or more duplicate keys or values.</exception>
    /// <remarks>
    /// If <paramref name="keyComparer"/> is null, the default equality comparer <see cref="EqualityComparer{TKey}.Default"/> is used to compare keys.
    /// <br/>
    /// If <paramref name="valueComparer"/> is null, the default equality comparer <see cref="EqualityComparer{TValue}.Default"/> is used to compare values.
    /// </remarks>
    public static BidirectionalDictionary<TKey, TValue> ToBidirectionalDictionary<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>> source,
        IEqualityComparer<TKey>? keyComparer,
        IEqualityComparer<TValue>? valueComparer)
        where TKey : notnull
        where TValue : notnull
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return new(source, keyComparer, valueComparer);
    }

    /// <summary>
    /// Creates a <see cref="BidirectionalDictionary{TKey,TValue}"/> from an <see cref="IEnumerable{T}"/> according to the default comparers for the key and value types.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys from elements of <paramref name="source"/></typeparam>
    /// <typeparam name="TValue">The type of the values from elements of <paramref name="source"/></typeparam>
    /// <param name="source">The <see cref="IEnumerable{T}"/> to create a <see cref="BidirectionalDictionary{TKey,TValue}"/> from.</param>
    /// <returns>A <see cref="BidirectionalDictionary{TKey,TValue}"/> that contains keys and values from <paramref name="source"/> and uses default comparers for the key and value types.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is a null reference.</exception>
    /// <exception cref="ArgumentException"><paramref name="source"/> contains one or more duplicate keys or values.</exception>
    public static BidirectionalDictionary<TKey, TValue> ToBidirectionalDictionary<TKey, TValue>(
        this IEnumerable<(TKey Key, TValue Value)> source)
        where TKey : notnull
        where TValue : notnull
    {
        return source.ToBidirectionalDictionary(keyComparer: null, valueComparer: null);
    }

    /// <summary>
    /// Creates a <see cref="BidirectionalDictionary{TKey,TValue}"/> from an <see cref="IEnumerable{T}"/> according to specified key and value comparers.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys from elements of <paramref name="source"/></typeparam>
    /// <typeparam name="TValue">The type of the values from elements of <paramref name="source"/></typeparam>
    /// <param name="source">The <see cref="IEnumerable{T}"/> to create a <see cref="BidirectionalDictionary{TKey,TValue}"/> from.</param>
    /// <param name="keyComparer">An <see cref="IEqualityComparer{TKey}"/> to compare keys.</param>
    /// <param name="valueComparer">An <see cref="IEqualityComparer{TValue}"/> to compare value.</param>
    /// <returns>A <see cref="BidirectionalDictionary{TKey,TValue}"/> that contains keys and values from <paramref name="source"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is a null reference.</exception>
    /// <exception cref="ArgumentException"><paramref name="source"/> contains one or more duplicate keys or values.</exception>
    /// <remarks>
    /// If <paramref name="keyComparer"/> is null, the default equality comparer <see cref="EqualityComparer{TKey}.Default"/> is used to compare keys.
    /// If <paramref name="valueComparer"/> is null, the default equality comparer <see cref="EqualityComparer{TValue}.Default"/> is used to compare values.
    /// </remarks>
    public static BidirectionalDictionary<TKey, TValue> ToBidirectionalDictionary<TKey, TValue>(
        this IEnumerable<(TKey Key, TValue Value)> source,
        IEqualityComparer<TKey>? keyComparer,
        IEqualityComparer<TValue>? valueComparer)
        where TKey : notnull
        where TValue : notnull
    {
        return source.ToBidirectionalDictionary(
            keyValue => keyValue.Key,
            keyValue => keyValue.Value,
            keyComparer,
            valueComparer);
    }

    public static BidirectionalDictionary<TKey, TSource> ToBidirectionalDictionary<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
        where TKey : notnull
        where TSource : notnull
    {
        return ToBidirectionalDictionary(source, keySelector, null, null);
    }

    public static BidirectionalDictionary<TKey, TSource> ToBidirectionalDictionary<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? keyComparer,
        IEqualityComparer<TSource>? valueComparer)
        where TKey : notnull
        where TSource : notnull
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (keySelector is null)
        {
            throw new ArgumentNullException(nameof(keySelector));
        }

        var capacity = 0;

#if NET6_0_OR_GREATER
        if (source.TryGetNonEnumeratedCount(out capacity))
        {
            if (capacity == 0)
            {
                return new BidirectionalDictionary<TKey, TSource>(keyComparer, valueComparer);
            }

            if (source is TSource[] array)
            {
                return SpanToBidirectionalDictionary(array, keySelector, keyComparer, valueComparer);
            }

            if (source is List<TSource> list)
            {
                ReadOnlySpan<TSource> span = CollectionsMarshal.AsSpan(list);
                return SpanToBidirectionalDictionary(span, keySelector, keyComparer, valueComparer);
            }
        }
#endif

        var bidirectionalDictionary = new BidirectionalDictionary<TKey, TSource>(capacity, keyComparer, valueComparer);
        
        foreach (TSource element in source)
        {
            bidirectionalDictionary.Add(keySelector(element), element);
        }

        return bidirectionalDictionary;
    }

#if NET6_0_OR_GREATER
    private static BidirectionalDictionary<TKey, TSource> SpanToBidirectionalDictionary<TSource, TKey>(
        ReadOnlySpan<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? keyComparer,
        IEqualityComparer<TSource>? valueComparer)
        where TKey : notnull
        where TSource : notnull
    {
        var bidirectionalDictionary = new BidirectionalDictionary<TKey, TSource>(source.Length, keyComparer, valueComparer);
        
        foreach (TSource element in source)
        {
            bidirectionalDictionary.Add(keySelector(element), element);
        }

        return bidirectionalDictionary;
    }
#endif

    public static BidirectionalDictionary<TKey, TElement> ToBidirectionalDictionary<TSource, TKey, TElement>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource, TElement> elementSelector)
        where TKey : notnull
        where TElement : notnull
    {
        return ToBidirectionalDictionary(source, keySelector, elementSelector, keyComparer: null, valueComparer: null);
    }

    public static BidirectionalDictionary<TKey, TElement> ToBidirectionalDictionary<TSource, TKey, TElement>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource, TElement> elementSelector,
        IEqualityComparer<TKey>? keyComparer,
        IEqualityComparer<TElement>? valueComparer)
        where TKey : notnull
        where TElement : notnull
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (keySelector is null)
        {
            throw new ArgumentNullException(nameof(keySelector));
        }

        if (elementSelector is null)
        {
            throw new ArgumentNullException(nameof(elementSelector));
        }

        var capacity = 0;

#if NET6_0_OR_GREATER
        if (source.TryGetNonEnumeratedCount(out capacity))
        {
            if (capacity == 0)
            {
                return new BidirectionalDictionary<TKey, TElement>(keyComparer, valueComparer);
            }

            if (source is TSource[] array)
            {
                return SpanToBidirectionalDictionary(array, keySelector, elementSelector, keyComparer, valueComparer);
            }

            if (source is List<TSource> list)
            {
                ReadOnlySpan<TSource> span = CollectionsMarshal.AsSpan(list);
                return SpanToBidirectionalDictionary(span, keySelector, elementSelector, keyComparer, valueComparer);
            }
        }
#endif

        var bidirectionalDictionary = new BidirectionalDictionary<TKey, TElement>(capacity, keyComparer, valueComparer);

        foreach (TSource element in source)
        {
            bidirectionalDictionary.Add(keySelector(element), elementSelector(element));
        }

        return bidirectionalDictionary;
    }

#if NET6_0_OR_GREATER
    private static BidirectionalDictionary<TKey, TElement> SpanToBidirectionalDictionary<TSource, TKey, TElement>(
        ReadOnlySpan<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource, TElement> elementSelector,
        IEqualityComparer<TKey>? keyComparer,
        IEqualityComparer<TElement>? valueComparer)
        where TKey : notnull
        where TElement : notnull
    {
        var bidirectionalDictionary = new BidirectionalDictionary<TKey, TElement>(source.Length, keyComparer, valueComparer);
        
        foreach (TSource element in source)
        {
            bidirectionalDictionary.Add(keySelector(element), elementSelector(element));
        }

        return bidirectionalDictionary;
    }
#endif
}
