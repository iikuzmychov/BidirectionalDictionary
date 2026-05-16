namespace System.Collections.Generic;

/// <summary>
/// Represents a read-only dictionary with non-null unique values that provides access to an inverse read-only dictionary.
/// </summary>
/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
public interface IReadOnlyBidirectionalDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
    where TValue : notnull
{
    /// <summary>
    /// Gets the inverse <see cref="IReadOnlyBidirectionalDictionary{TKey,TValue}"/> 
    /// in which the roles of keys and values are swapped.
    /// </summary>
    public IReadOnlyBidirectionalDictionary<TValue, TKey> Inverse { get; }

    /// <summary>
    /// Determines whether the <see cref="IReadOnlyBidirectionalDictionary{TKey, TValue}"/> contains the specified value.
    /// </summary>
    /// <param name="value">The value to locate in the <see cref="IReadOnlyBidirectionalDictionary{TKey, TValue}"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="IReadOnlyBidirectionalDictionary{TKey, TValue}"/> contains
    /// an element with the specified value; otherwise, <see langword="false"/>.
    /// </returns>
    public bool ContainsValue(TValue value);
}
