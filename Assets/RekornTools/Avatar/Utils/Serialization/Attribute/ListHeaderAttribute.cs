using System;
using JetBrains.Annotations;

namespace RekornTools.Avatar
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ListHeaderAttribute : Attribute
    {
        public string Header { get; set; }

        public ListHeaderAttribute([NotNull] string header) => Header = header;
    }
}
