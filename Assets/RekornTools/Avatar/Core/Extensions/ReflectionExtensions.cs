using System;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;

namespace RekornTools.Avatar
{
    public static class ReflectionExtensions
    {
#region Attribute
        public static readonly BindingFlags Everything = ~BindingFlags.Default;

        public static (MemberInfo, Type) GetFieldOrProperty([CanBeNull] this Type type, [CanBeNull] string name)
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
