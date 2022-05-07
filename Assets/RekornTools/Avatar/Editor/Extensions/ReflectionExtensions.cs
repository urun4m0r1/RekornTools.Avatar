using System;
using System.Reflection;
using JetBrains.Annotations;
using UnityEditor;

namespace RekornTools.Avatar.Editor
{
    public static class ReflectionExtensions
    {
#region Attribute
        public static readonly BindingFlags Everything = ~BindingFlags.Default;

        [CanBeNull]
        public static T GetAttribute<T>([CanBeNull] this SerializedProperty property, bool inherit = true) where T : Attribute
        {
            var attributes = property.GetAttributes<T>(inherit);
            return attributes?.Length > 0 ? attributes[0] : null;
        }

        [CanBeNull]
        public static T[] GetAttributes<T>([CanBeNull] this SerializedProperty property, bool inherit = true) where T : Attribute
        {
            var member = property?.GetFieldOrProperty();
            return member?.GetCustomAttributes(typeof(T), inherit) as T[];
        }

        [CanBeNull]
        static MemberInfo GetFieldOrProperty([NotNull] this SerializedProperty property)
        {
            var target = property.serializedObject?.targetObject;
            var paths  = property.propertyPath?.Split('.');
            if (target == null || paths == null) return null;

            var        type   = target.GetType();
            MemberInfo member = null;
            foreach (var name in paths)
            {
                (member, type) = type.GetFieldOrProperty(name);
            }

            return member;
        }
#endregion // Attribute
    }
}
