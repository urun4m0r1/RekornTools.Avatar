using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using UnityEngine;

namespace RekornTools.Math
{
    public static class VectorExtensions
    {
        public enum NormalAxis
        {
            Up,
            Down,
            Left,
            Right,
            Forward,
            Back,
        }

        public static Vector3 GetNormal(NormalAxis axis)
        {
            switch (axis)
            {
                case NormalAxis.Up:      return Vector3.up;
                case NormalAxis.Down:    return Vector3.down;
                case NormalAxis.Left:    return Vector3.left;
                case NormalAxis.Right:   return Vector3.right;
                case NormalAxis.Forward: return Vector3.forward;
                case NormalAxis.Back:    return Vector3.back;
                default:                 return Vector3.zero;
            }
        }

        [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
        public static Vector3 GetNormal([System.Diagnostics.CodeAnalysis.NotNull] this Transform vector, NormalAxis axis)
        {
            switch (axis)
            {
                case NormalAxis.Up:      return vector.up;
                case NormalAxis.Down:    return -vector.up;
                case NormalAxis.Left:    return -vector.right;
                case NormalAxis.Right:   return vector.right;
                case NormalAxis.Forward: return vector.forward;
                case NormalAxis.Back:    return -vector.forward;
                default:                 return Vector3.zero;
            }
        }

        public static Vector2 ConvertXZ(this Vector3 input) => new Vector2(input.x, input.z);
        public static Vector3 ConvertXZ(this Vector2 input) => new Vector3(input.x, 0f, input.y);

        public static Vector2 ClampMagnitude(this Vector2 v, float min, float max)
        {
            double sm = v.sqrMagnitude;
            if (sm > (double)max * max) return v.normalized * max;
            if (sm < (double)min * min) return v.normalized * min;

            return v;
        }

        public static Vector3 ClampMagnitudePlain(this Vector3 v, float min, float max)
        {
            var y = v.y;
            v   = v.ConvertXZ().ClampMagnitude(min, max).ConvertXZ();
            v.y = y;
            return v;
        }

        public static Vector3 ClampMagnitude(this Vector3 v, float min, float max)
        {
            double sm = v.sqrMagnitude;
            if (sm > (double)max * max) return v.normalized * max;
            if (sm < (double)min * min) return v.normalized * min;

            return v;
        }
    }
}
