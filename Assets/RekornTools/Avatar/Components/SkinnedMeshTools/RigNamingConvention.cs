using System;
using JetBrains.Annotations;
using UnityEngine;

namespace RekornTools.Avatar
{
    public enum ModifierType
    {
        Front,
        End,
    }

    [Serializable]
    public struct RigNamingConvention
    {
        public static RigNamingConvention Default = new RigNamingConvention
        {
            ModifierType  = ModifierType.End,
            Splitter      = ".",
            ModifierLeft  = "L",
            ModifierRight = "R",
        };

        [field: SerializeField]             public ModifierType ModifierType  { get; private set; }
        [field: SerializeField] [CanBeNull] public string       Splitter      { get; private set; }
        [field: SerializeField] [CanBeNull] public string       ModifierLeft  { get; private set; }
        [field: SerializeField] [CanBeNull] public string       ModifierRight { get; private set; }

        [NotNull] public string LeftFront  => $"{ModifierLeft}{Splitter}";
        [NotNull] public string RightFront => $"{ModifierRight}{Splitter}";
        [NotNull] public string LeftEnd    => $"{Splitter}{ModifierLeft}";
        [NotNull] public string RightEnd   => $"{Splitter}{ModifierRight}";

        public RigNamingConvention(ModifierType modifierType, [CanBeNull] string splitter, [CanBeNull] string modifierLeft, [CanBeNull] string modifierRight)
        {
            ModifierType  = modifierType;
            Splitter      = splitter;
            ModifierLeft  = modifierLeft;
            ModifierRight = modifierRight;
        }
    }
}
