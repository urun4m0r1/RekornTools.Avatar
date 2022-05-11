using System;
using JetBrains.Annotations;
using UnityEngine;

namespace RekornTools.Avatar
{
    public static class TransformExtensions
    {
        public static void InvokeRecursive([NotNull] this Transform parent, [NotNull] Action<Transform> action)
        {
            action.Invoke(parent);

            foreach (Transform child in parent)
            {
                if (child == null) continue;
                child.InvokeRecursive(action);
            }
        }

        [CanBeNull]
        public static Transform FindRecursive([NotNull] this Transform parent, [NotNull] string name)
        {
            if (parent.name == name) return parent;

            foreach (Transform child in parent)
            {
                if (child      == null) continue;
                if (child.name == name) return child;
                var result = child.FindRecursive(name);
                if (result != null) return result;
            }

            return null;
        }

        public static Bounds TransformBounds([NotNull] this Transform transform, Bounds localBounds)
        {
            var center = transform.TransformPoint(localBounds.center);

            var extents = localBounds.extents;
            var axisX   = transform.TransformVector(extents.x, 0,         0);
            var axisY   = transform.TransformVector(0,         extents.y, 0);
            var axisZ   = transform.TransformVector(0,         0,         extents.z);

            extents.x = Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x);
            extents.y = Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y);
            extents.z = Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z);

            return new Bounds { center = center, extents = extents };
        }

        public static Bounds InverseTransformBounds([NotNull] this Transform transform, Bounds localBounds)
        {
            var center = transform.InverseTransformPoint(localBounds.center);

            var extents = localBounds.extents;
            var axisX   = transform.InverseTransformVector(extents.x, 0,         0);
            var axisY   = transform.InverseTransformVector(0,         extents.y, 0);
            var axisZ   = transform.InverseTransformVector(0,         0,         extents.z);

            extents.x = Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x);
            extents.y = Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y);
            extents.z = Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z);

            return new Bounds { center = center, extents = extents };
        }
    }
}
