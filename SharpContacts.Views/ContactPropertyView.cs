using System;

namespace SharpContacts.Views
{
    public class ContactPropertyView
    {
        public string PropertyId { get; set; }

        /// <summary>
        /// Indicates the type of the property
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Represents a sub-type to make easier to interpret the property value, like Home, Work subtypes in a Phone type
        /// </summary>
        public string Subtype { get; set; }
        /// <summary>
        /// Value of the property, encoded as a string
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Optionally indicates wether this property value is valid not before this date has passed
        /// </summary>
        public DateTimeOffset? NotBefore { get; set; }
        /// <summary>
        /// Optionally indicates wether this property value is valid not after this date has passed
        /// </summary>
        public DateTimeOffset? NotAfter { get; set; }
        /// <summary>
        /// Optionally indicates the culture used for this property value
        /// </summary>
        public string CultureName { get; set; }
        /// <summary>
        /// Optionally indicates the preference to use this property value over others of the same type
        /// </summary>
        public byte? Preferred { get; set; }
        /// <summary>
        /// Optionally indicates the media type related with this property if it represents a link to another resource like a CV or Picture
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// Optionally indicates a geo-location where this property makes sense
        /// </summary>
        public string Where { get; set; }
    }
}
