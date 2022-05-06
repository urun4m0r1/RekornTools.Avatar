using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace RekornTools.Avatar
{
    public static class DictionaryExtensions
    {
        public static void MatchDictionaryKey<T, V>([NotNull] this Dictionary<T, V> target, [NotNull] IReadOnlyCollection<T> types, [NotNull] Func<T, V> getValue)
        {
            var keys     = target.Keys.ToList();
            var toAdd    = types.Except(keys).ToList();
            var toRemove = keys.Except(types).ToList();

            foreach (var key in toAdd) target.Add(key, getValue.Invoke(key));
            foreach (var key in toRemove) target.Remove(key);
        }
    }
}
