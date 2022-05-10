using System;
using UnityEngine;

namespace RekornTools.Avatar
{
    public sealed class AvatarRig : AvatarRig<Component> { }

    [Serializable]
    public class AvatarRig<T> where T : Component
    {
        [field: SerializeField] public T                   Rig    { get; private set; }
        [field: SerializeField] public RigNamingConvention Naming { get; private set; } = RigNamingConvention.Default;
    }
}
