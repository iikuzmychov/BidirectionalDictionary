namespace System.Collections.Generic;

/// <summary>
/// Represents a dictionary with non-null unique values that provides access to an inverse dictionary.
/// </summary>
/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
public interface IBidirectionalDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    where TKey : notnull
    where TValue : notnull
{
    /// <summary>
    /// Gets the inverse <see cref="IBidirectionalDictionary{TKey,TValue}"/> 
    /// in which the roles of keys and values are swapped.
    /// </summary>
    public IBidirectionalDictionary<TValue, TKey> Inverse { get; }

    /// <summary>
    /// Determines whether the <see cref="IBidirectionalDictionary{TKey, TValue}"/> contains the specified value.
    /// </summary>
    /// <param name="value">The value to locate in the <see cref="IBidirectionalDictionary{TKey, TValue}"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="IBidirectionalDictionary{TKey, TValue}"/> contains
    /// an element with the specified value; otherwise, <see langword="false"/>.
    /// </returns>
    public bool ContainsValue(TValue value);
}
