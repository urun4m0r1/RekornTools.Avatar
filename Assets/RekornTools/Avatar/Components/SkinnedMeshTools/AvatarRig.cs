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
        [field: SerializeField] public T                   Rig    { get; private set; }
        [field: SerializeField] public RigNamingConvention Naming { get; private set; } = RigNamingConvention.Default;
    }
}
