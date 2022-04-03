using System;

namespace VRCAvatarTools
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ListHeaderAttribute : Attribute
    {
        public string Header { get; }

        public ListHeaderAttribute(string header) => Header = header;
    }
}
