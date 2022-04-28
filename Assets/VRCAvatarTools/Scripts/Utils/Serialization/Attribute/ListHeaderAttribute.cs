using System;
using JetBrains.Annotations;

namespace VRCAvatarTools
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ListHeaderAttribute : Attribute
    {
        public string Header { get; set; }

        public ListHeaderAttribute([NotNull] string header) => Header = header;
    }
}
