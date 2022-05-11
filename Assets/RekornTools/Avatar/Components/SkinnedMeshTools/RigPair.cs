using System;
using UnityEngine;

namespace RekornTools.Avatar
{
    public sealed class RigPair : RigPair<object> { }

    [Serializable]
    public class RigPair<T>
    {
        [field: SerializeField] public T Target { get; private set; }
        [field: SerializeField] public T Source  { get; private set; }
    }
}
