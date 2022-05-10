using System;
using UnityEngine;

namespace RekornTools.Avatar
{
    public sealed class RigPair : RigPair<object> { }

    [Serializable]
    public class RigPair<T>
    {
        [field: SerializeField] public T Avatar { get; private set; }
        [field: SerializeField] public T Cloth  { get; private set; }
    }
}
