using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace RekornTools.Math
{
    public static class ListExtensions
    {
        public static int FindNearestBiggerIndex<T1, T2>(
            [NotNull] this IReadOnlyList<T1> list, [NotNull] T2 target, [NotNull] Func<T1, T2> selector)
        {
            var i = list.BinarySearch(target, selector);

            return ClampArrayIndex(i >= 0 ? i : ~i, 0, list.Count);
        }

        public static int FindNearestSmallerIndex<T1, T2>(
            [NotNull] this IReadOnlyList<T1> list, [NotNull] T2 target, [NotNull] Func<T1, T2> selector)
        {
            var i = list.BinarySearch(target, selector);

            return ClampArrayIndex(i >= 0 ? i : ~i - 1, 0, list.Count);
        }

        private static int ClampArrayIndex(int i, int from, int to)
        {
            if (i < from) i = from;
            if (i >= to) i = to - 1;
            return i;
        }

        private static int BinarySearch<T1, T2>([NotNull] this IReadOnlyList<T1> list, [NotNull] T2 target, [NotNull] Func<T1, T2> selector)
        {
            var min = 0;
            var max = list.Count - 1;
            var comparer = Comparer<T2>.Default;

            while (min <= max)
            {
                var mid = min + ((max - min) >> 1);
                var num = comparer.Compare(selector(list[mid]), target);

                if (num == 0) return mid;

                if (num < 0) min = mid + 1;
                else max = mid - 1;
            }

            return ~min;
        }
    }
}
