using System;

namespace VRCAvatarTools
{
    public class ListMutableAttribute : Attribute
    {
        public bool IsMutable { get; }

        public ListMutableAttribute(bool isMutable = true) => IsMutable = isMutable;
    }
}
