using System;

namespace VRCAvatarTools
{
    public class ListSpanAttribute : Attribute
    {
        public bool IsSpan { get; }

        public ListSpanAttribute(bool isSpan = true) => IsSpan = isSpan;
    }
}
