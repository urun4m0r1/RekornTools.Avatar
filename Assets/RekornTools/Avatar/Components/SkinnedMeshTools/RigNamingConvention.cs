using System;
using JetBrains.Annotations;
using UnityEngine;

namespace RekornTools.Avatar
{
    public enum ModifierPosition
    {
        Front,
        End,
    }

    public enum ModifierType
    {
        None,
        Both,
        Left,
        Right,
    }

    [Serializable]
    public struct RigNamingConvention
    {
        public static RigNamingConvention Default = new RigNamingConvention
        {
            ModifierPosition = ModifierPosition.End,
            Splitter         = "_",
            ModifierLeft     = "L",
            ModifierRight    = "R",
        };

        [field: SerializeField]             public ModifierPosition ModifierPosition { get; private set; }
        [field: SerializeField] [CanBeNull] public string           Splitter         { get; private set; }
        [field: SerializeField] [CanBeNull] public string           ModifierLeft     { get; private set; }
        [field: SerializeField] [CanBeNull] public string           ModifierRight    { get; private set; }

        public RigNamingConvention(ModifierPosition modifierPosition, [CanBeNull] string splitter, [CanBeNull] string modifierLeft, [CanBeNull] string modifierRight)
        {
            ModifierPosition = modifierPosition;
            Splitter         = splitter;
            ModifierLeft     = modifierLeft;
            ModifierRight    = modifierRight;
        }

        [NotNull] public string FrontLeft  => $"{ModifierLeft}{Splitter}";
        [NotNull] public string FrontRight => $"{ModifierRight}{Splitter}";
        [NotNull] public string EndLeft    => $"{Splitter}{ModifierLeft}";
        [NotNull] public string EndRight   => $"{Splitter}{ModifierRight}";

        public string GetModifier(ModifierType type)
        {
            var pos = ModifierPosition;
            if (pos == ModifierPosition.Front && type == ModifierType.Left) return FrontLeft;
            if (pos == ModifierPosition.Front && type == ModifierType.Right) return FrontRight;
            if (pos == ModifierPosition.End   && type == ModifierType.Left) return EndLeft;
            if (pos == ModifierPosition.End   && type == ModifierType.Right) return EndRight;
            return null;
        }

        public ModifierType GetModifierType([NotNull] string name)
        {
            var isLeft  = false;
            var isRight = false;

            if (ModifierPosition == ModifierPosition.Front)
            {
                isLeft  = name.Contains(FrontLeft);
                isRight = name.Contains(FrontRight);
            }
            else
            {
                isLeft  = name.Contains(EndLeft);
                isRight = name.Contains(EndRight);
            }

            if (isLeft && isRight) return ModifierType.Both;
            else if (isLeft) return ModifierType.Left;
            else if (isRight) return ModifierType.Right;
            else return ModifierType.None;
        }

        [NotNull]
        public static string Convert([NotNull] string name, RigNamingConvention src, RigNamingConvention dst)
        {
            var type = src.GetModifierType(name);

            switch (type)
            {
                case ModifierType.None:
                    return name;
                case ModifierType.Both:
                    Debug.LogWarning($"Bone name {name} contains both left and right modifiers. Skipping.");
                    return name;
                case ModifierType.Left:
                case ModifierType.Right:
                    var from = src.GetModifier(type) ?? throw new ArgumentException("Unknown modifier type.");
                    var to   = dst.GetModifier(type) ?? throw new ArgumentException("Unknown modifier type.");
                    return ReplaceModifier(name, from, to, src.ModifierPosition, dst.ModifierPosition);
                default:
                    throw new ArgumentException("Unknown modifier type.");
            }
        }


        [NotNull]
        static string ReplaceModifier([NotNull] string name, [NotNull] string from, [NotNull] string to, ModifierPosition srcPosition, ModifierPosition dstPosition)
        {
            if (srcPosition == dstPosition)
            {
                return name.Replace(from, to);
            }
            else
            {
                if (srcPosition == ModifierPosition.Front)
                {
                    return name.Replace(from, null) + to;
                }
                else
                {
                    return to + name.Replace(from, null);
                }
            }
        }
    }
}
