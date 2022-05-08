using System;
using JetBrains.Annotations;
using UnityEngine;

namespace RekornTools.Avatar
{
    public sealed class AvatarRig : AvatarRig<Component>
    {
        [NotNull] public const string RigFieldName    = nameof(Rig);
        [NotNull] public const string NamingFieldName = nameof(Naming);
    }

    [Serializable]
    public class AvatarRig<T> where T : Component
    {
        [SerializeField] public T                   Rig;
        [SerializeField] public RigNamingConvention Naming = new RigNamingConvention();
    }
}
