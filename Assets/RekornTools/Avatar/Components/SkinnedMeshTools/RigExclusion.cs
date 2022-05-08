#if UNITY_EDITOR
using System;
using UnityEngine;

namespace RekornTools.Avatar
{
    [Serializable]
    public struct RigExclusion
    {
        [field: SerializeField] public bool      DisableParenting { get; private set; }
        [field: SerializeField] public Transform AvatarBone       { get; private set; }
        [field: SerializeField] public Transform ClothBone        { get; private set; }
    }
}
#endif
