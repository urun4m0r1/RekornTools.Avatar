#if UNITY_EDITOR
using System;
using UnityEngine;

namespace RekornTools.Avatar
{
    [Serializable]
    public struct RigNamePair
    {
        [field: SerializeField] public bool   DisableParenting { get; private set; }
        [field: SerializeField] public string AvatarBoneName   { get; private set; }
        [field: SerializeField] public string ClothBoneName    { get; private set; }
    }
}
#endif
