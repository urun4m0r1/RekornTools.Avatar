#if UNITY_EDITOR
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
    public class RigNamingConvention
    {
        [field: SerializeField] public ModifierType ModifierType  { get; private set; } = ModifierType.End;
        [field: SerializeField] public string       Splitter      { get; private set; } = ".";
        [field: SerializeField] public string       ModifierLeft  { get; private set; } = "L";
        [field: SerializeField] public string       ModifierRight { get; private set; } = "R";

        [NotNull] public string LeftFront  => $"{ModifierLeft}{Splitter}";
        [NotNull] public string RightFront => $"{ModifierRight}{Splitter}";
        [NotNull] public string LeftEnd    => $"{Splitter}{ModifierLeft}";
        [NotNull] public string RightEnd   => $"{Splitter}{ModifierRight}";
    }
}
#endif
