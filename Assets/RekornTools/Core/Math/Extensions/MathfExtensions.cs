using UnityEngine;

namespace RekornTools.Math
{
    public static class MathfExtensions
    {
        public static int LerpInt(int prevValue, int nextValue, float lerpSpeed, int lerpStep)
        {
            var lerpValue = Mathf.Lerp(prevValue, nextValue, lerpSpeed);
            var answer = AbsMax(lerpValue, lerpStep);
            return (int)answer;
        }

        public static float AbsMax(float a, float b) => Mathf.Max(Mathf.Abs(a), Mathf.Abs(b));
        public static float AbsMin(float a, float b) => Mathf.Min(Mathf.Abs(a), Mathf.Abs(b));
    }
}
