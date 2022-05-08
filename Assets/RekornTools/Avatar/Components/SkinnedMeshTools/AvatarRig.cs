using System;
using UnityEngine;

namespace RekornTools.Avatar
{
    [Serializable]
    public struct AvatarRig
    {
        [SerializeField] public Transform           Rig;
        [SerializeField] public RigNamingConvention Naming;
    }
}
