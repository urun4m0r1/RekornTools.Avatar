using System;

// ReSharper disable InconsistentNaming

namespace RekornTools.Math
{
    public static class BetweenExtensions
    {
        public static bool IsBetweenII<T>(this T value, T min, T max) where T : struct, IComparable<T> =>
            min.CompareTo(value) <= 0 && value.CompareTo(max) <= 0;

        public static bool IsBetweenEI<T>(this T value, T min, T max) where T : struct, IComparable<T> =>
            min.CompareTo(value) < 0 && value.CompareTo(max) <= 0;

        public static bool IsBetweenIE<T>(this T value, T min, T max) where T : struct, IComparable<T> =>
            min.CompareTo(value) <= 0 && value.CompareTo(max) < 0;

        public static bool IsBetweenEE<T>(this T value, T min, T max) where T : struct, IComparable<T> =>
            min.CompareTo(value) < 0 && value.CompareTo(max) < 0;
    }
}
