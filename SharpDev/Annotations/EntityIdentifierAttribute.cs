using System;

namespace SharpDev.Annotations
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class EntityIdentifierAttribute : Attribute
    {
    }
}