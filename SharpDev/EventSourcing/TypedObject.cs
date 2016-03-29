namespace SharpDev.EventSourcing
{
    /// <summary>
    /// Represents a CLR object tagged with a versioned domain model type
    /// </summary>
    public class TypedObject
    {
        public TypedObject(int version, object o, ObjectType type)
        {
            Version = version;
            Object = o;
            Type = type;
        }

        /// <summary>
        /// Gets the version of this Typed Object instance
        /// </summary>
        public int Version { get; }

        /// <summary>
        /// Gets the CLR object
        /// </summary>
        public object Object { get; }

        /// <summary>
        /// Gets the type of this object
        /// </summary>
        public ObjectType Type { get; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString() => $"Object: {Object}, Type: {Type}";
    }
}