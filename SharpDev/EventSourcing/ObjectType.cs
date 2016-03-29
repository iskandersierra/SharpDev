using System;
using System.Diagnostics.Contracts;

namespace SharpDev.EventSourcing
{
    /// <summary>
    /// Represents a versioned object type
    /// </summary>
    public sealed class ObjectType
    {
        /// <summary>
        /// Creates a new instance of <see cref="ObjectType"/>
        /// </summary>
        /// <param name="domainModel"></param>
        /// <param name="version"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ObjectType(string domainModel, Version version)
        {
            if (domainModel == null) throw new ArgumentNullException(nameof(domainModel));
            if (version == null) throw new ArgumentNullException(nameof(version));

            DomainModel = domainModel;
            Version = version;
        }

        /// <summary>
        /// Gets the DomainModel of this object type
        /// </summary>
        [Pure]
        public string DomainModel { get; }
        /// <summary>
        /// Gets the version of this object type
        /// </summary>
        [Pure]
        public Version Version { get; }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other">The object to compare with the current object. </param>
        /// <returns></returns>
        private bool Equals(ObjectType other) => string.Equals(DomainModel, other.DomainModel) && Equals(Version, other.Version);

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ObjectType) obj);
        }

        /// <summary>
        /// Serves as the default hash function. 
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((DomainModel?.GetHashCode() ?? 0)*397) ^ 
                       (Version?.GetHashCode() ?? 0);
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString() => $"DomainModel: {DomainModel}, Version: {Version}";
    }
}