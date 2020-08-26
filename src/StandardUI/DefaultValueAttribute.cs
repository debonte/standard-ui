using System;

namespace System.StandardUI
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class DefaultValueAttribute : Attribute
    {
        public object? Value { get; }

        public DefaultValueAttribute(object? value)
        {
            Value = value;
        }
    }
}