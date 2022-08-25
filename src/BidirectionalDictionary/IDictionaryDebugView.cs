using System.Diagnostics;

namespace System.Collections.Generic
{
    internal sealed class IDictionaryDebugView<TKey, TValue>
        where TKey : notnull
        where TValue : notnull
    {
        private IDictionary<TKey, TValue> _dictionary;

        public IDictionaryDebugView(IDictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public KeyValuePair<TKey, TValue>[] Items
        {
            get
            {
                var items = new KeyValuePair<TKey, TValue>[_dictionary.Count];

                _dictionary.CopyTo(items, 0);

                return items;
            }
        }
    }
}
