using System;
using JetBrains.Annotations;
using UnityEngine;

namespace RekornTools.Debug
{
    public static class DrawerHelper
    {
        public static bool WillDrawOnSelected([NotNull] this IGizmos target) =>
            target.DrawMode is DrawMode.Always || target.DrawMode is DrawMode.OnSelected;
        public static bool WillDrawOnNonSelected([NotNull] this IGizmos target) =>
            target.DrawMode is DrawMode.Always || target.DrawMode is DrawMode.OnNonSelected;

        public static void DrawOnSelected<T>([NotNull] this T target, [NotNull] Action<T> action)
            where T : MonoBehaviour, IGizmos
        {
            if (target.WillDrawOnSelected()) action(target);
        }

        public static void DrawOnNonSelected<T>([NotNull] this T target, [NotNull] Action<T> action)
            where T : MonoBehaviour, IGizmos
        {
            if (target.WillDrawOnNonSelected()) action(target);
        }
    }
}
