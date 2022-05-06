using System;

namespace RekornTools.Avatar
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class ReadOnlyListAttribute : Attribute { }
}
