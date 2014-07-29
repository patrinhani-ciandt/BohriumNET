namespace Bohrium.Core.Extensions
{
    using System.Collections.Generic;

    public static class IDictionaryExtensionMethods
    {
        /// <summary>
        /// returns true if the value is in the dictionary
        /// </summary>      
        public static bool ContainsValue<TK, TV>(this IDictionary<TK, TV> instance, TV value)
        {
            return instance.Exists(kvp => kvp.Value.Equals(value));
        }

        /// <summary>
        /// Gets the first key with the matching value. Returns true if the value is found, false if not.
        /// </summary>
        public static bool TryGetKey<TK, TV>(this IDictionary<TK, TV> instance, TV value, out TK key)
        {
            foreach (var entry in instance)
            {
                if (entry.Value.Equals(value))
                {
                    key = entry.Key;
                    return true;
                }
            }
            key = default(TK);
            return false;
        }

        /// <summary>
        /// Gets all of th ekeys with the matching value. Returns true if the value is found, false if not.
        /// </summary>
        public static bool TryGetKeys<TK, TV>(this IDictionary<TK, TV> instance, TV value, out TK[] keys)
        {
            var found = new List<TK>();
            foreach (var entry in instance)
            {
                if (entry.Value.Equals(value))
                {
                    found.Add(entry.Key);
                }
            }
            keys = found.ToArray();
            return found.Count > 0;
        }
    }
}
