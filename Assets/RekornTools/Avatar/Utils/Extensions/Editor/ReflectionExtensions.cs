using System;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;
using UnityEditor;

namespace RekornTools.Avatar
{
    public static class ReflectionExtensions
    {
#region Attribute
        static readonly BindingFlags Everything = ~BindingFlags.Default;

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

        static (MemberInfo, Type) GetFieldOrProperty([CanBeNull] this Type type, [CanBeNull] string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return (null, type);

            var fieldInfo = type?.GetField(name, Everything);
            if (fieldInfo != null) return (fieldInfo, fieldInfo.FieldType);

            var propertyInfo = type?.GetProperty(name, Everything);
            if (propertyInfo != null) return (propertyInfo, propertyInfo.PropertyType);

            return (null, type);
        }
#endregion // Attribute

#region Property
        [NotNull] static readonly StringBuilder Sb = new StringBuilder();

        [NotNull] const string AutoPropertyHeader = "<";
        [NotNull] const string AutoPropertyFooter = ">k__BackingField";

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
#endregion // Property
    }
}
