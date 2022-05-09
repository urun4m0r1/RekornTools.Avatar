using System;
using UnityEngine;

namespace RekornTools.Avatar
{
    [Serializable]
    public struct RigPair
    {
        [field: SerializeField] public Transform Avatar { get; private set; }
        [field: SerializeField] public Transform Cloth  { get; private set; }
    }
}
