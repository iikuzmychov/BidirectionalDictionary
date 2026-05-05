using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

[ExcludeFromCodeCoverage]
internal sealed class DictionaryDebugView<TKey, TValue>
    where TKey : notnull
{
    private readonly IDictionary<TKey, TValue> _dictionary;

    public DictionaryDebugView(IDictionary<TKey, TValue> dictionary)
    {
        _dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
    }

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public DebugViewDictionaryItem<TKey, TValue>[] Items
    {
        get
        {
            var keyValuePairs = new KeyValuePair<TKey, TValue>[_dictionary.Count];
            _dictionary.CopyTo(keyValuePairs, 0);

            var items = new DebugViewDictionaryItem<TKey, TValue>[keyValuePairs.Length];

            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new DebugViewDictionaryItem<TKey, TValue>(keyValuePairs[i]);
            }

            return items;
        }
    }
}
