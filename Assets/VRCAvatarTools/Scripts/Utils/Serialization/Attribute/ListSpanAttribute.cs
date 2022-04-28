using System;

namespace VRCAvatarTools
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ListSpanAttribute : Attribute
    {
        public const bool Default = true;

        public bool IsSpan { get; }

        public ListSpanAttribute(bool isSpan = Default) => IsSpan = isSpan;
    }
}
