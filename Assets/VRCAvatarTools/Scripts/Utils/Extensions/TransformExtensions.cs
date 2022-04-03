using UnityEngine;

namespace VRCAvatarTools
{
    public static class TransformExtensions
    {
        public static Bounds TransformBounds(this Transform transform, Bounds localBounds)
        {
            Vector3 center = transform.TransformPoint(localBounds.center);

            Vector3 extents = localBounds.extents;
            Vector3 axisX   = transform.TransformVector(extents.x, 0,         0);
            Vector3 axisY   = transform.TransformVector(0,         extents.y, 0);
            Vector3 axisZ   = transform.TransformVector(0,         0,         extents.z);

            extents.x = Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x);
            extents.y = Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y);
            extents.z = Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z);

            return new Bounds { center = center, extents = extents };
        }

        public static Bounds InverseTransformBounds(this Transform transform, Bounds localBounds)
        {
            Vector3 center = transform.InverseTransformPoint(localBounds.center);

            Vector3 extents = localBounds.extents;
            Vector3 axisX   = transform.InverseTransformVector(extents.x, 0,         0);
            Vector3 axisY   = transform.InverseTransformVector(0,         extents.y, 0);
            Vector3 axisZ   = transform.InverseTransformVector(0,         0,         extents.z);

            extents.x = Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x);
            extents.y = Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y);
            extents.z = Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z);

            return new Bounds { center = center, extents = extents };
        }
    }
}
