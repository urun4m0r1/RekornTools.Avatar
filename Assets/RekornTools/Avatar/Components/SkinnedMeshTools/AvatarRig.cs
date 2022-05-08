using System;
using UnityEngine;

namespace RekornTools.Avatar
{
    [Serializable]
    public class AvatarRig
    {
        [SerializeField] public Transform           Rig;
        [SerializeField] public RigNamingConvention Naming = new RigNamingConvention();
    }
}
