using System;
using System.Text;
using JetBrains.Annotations;
using NaughtyAttributes.Editor;
using UnityEditor;

namespace VRCAvatarTools
{
    public static class ReflectionExtensions
    {
        [NotNull] private static readonly StringBuilder Sb = new StringBuilder();

        [NotNull] private const string AutoPropertyHeader = "<";
        [NotNull] private const string AutoPropertyFooter = ">k__BackingField";

        public static bool IsAutoProperty([NotNull] string name) =>
            name.StartsWith(AutoPropertyHeader, StringComparison.Ordinal)
         && name.EndsWith(AutoPropertyFooter, StringComparison.Ordinal);

        [NotNull]
        public static string ResolveFieldName([NotNull] string name)
        {
            if (IsAutoProperty(name)) return name;

            Sb.Clear();
            Sb.Append(AutoPropertyHeader);
            Sb.Append(name);
            Sb.Append(AutoPropertyFooter);
            return Sb.ToString();
        }

        [CanBeNull]
        public static T GetAttribute<T>([CanBeNull] this SerializedProperty property) where T : Attribute =>
            PropertyUtility.GetAttribute<T>(property);

        [CanBeNull]
        [ItemCanBeNull]
        public static T[] GetAttributes<T>([CanBeNull] this SerializedProperty property) where T : Attribute =>
            PropertyUtility.GetAttributes<T>(property);
    }
}
