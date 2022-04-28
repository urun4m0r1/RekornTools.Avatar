using System;

namespace VRCAvatarTools
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ListMutableAttribute : Attribute
    {
        public const bool Default = true;

        public bool IsMutable { get; }

        public ListMutableAttribute(bool isMutable = Default) => IsMutable = isMutable;
    }
}
