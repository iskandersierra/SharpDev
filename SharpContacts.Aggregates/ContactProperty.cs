using System;
using SharpContacts.Events;

namespace SharpContacts.Aggregates
{
    public class ContactProperty
    {
        public ContactProperty()
        {
        }

        public ContactProperty(ContactPropertySnapshot snapshot)
        {
            PropertyId = snapshot.PropertyId;
            Type = snapshot.Type;
            Subtype = snapshot.Subtype;
            Value = snapshot.Value;
            NotBefore = snapshot.NotBefore;
            NotAfter = snapshot.NotAfter;
            CultureName = snapshot.CultureName;
            Preferred = snapshot.Preferred;
            ContentType = snapshot.ContentType;
            Where = snapshot.Where;
        }

        public string PropertyId { get; private set; }

        /// <summary>
        /// Indicates the type of the property
        /// </summary>
        public string Type { get; private set; }
        /// <summary>
        /// Represents a sub-type to make easier to interpret the property value, like Home, Work subtypes in a Phone type
        /// </summary>
        public string Subtype { get; private set; }
        /// <summary>
        /// Value of the property, encoded as a string
        /// </summary>
        public string Value { get; private set; }
        /// <summary>
        /// Optionally indicates wether this property value is valid not before this date has passed
        /// </summary>
        public DateTimeOffset? NotBefore { get; private set; }
        /// <summary>
        /// Optionally indicates wether this property value is valid not after this date has passed
        /// </summary>
        public DateTimeOffset? NotAfter { get; private set; }
        /// <summary>
        /// Optionally indicates the culture used for this property value
        /// </summary>
        public string CultureName { get; private set; }
        /// <summary>
        /// Optionally indicates the preference to use this property value over others of the same type
        /// </summary>
        public byte? Preferred { get; private set; }
        /// <summary>
        /// Optionally indicates the media type related with this property if it represents a link to another resource like a CV or Picture
        /// </summary>
        public string ContentType { get; private set; }
        /// <summary>
        /// Optionally indicates a geo-location where this property makes sense
        /// </summary>
        public string Where { get; private set; }

        public void On(ContactPropertySet ev)
        {
            PropertyId = ev.PropertyId;
            Type = ev.Type;
            Subtype = ev.Subtype;
            Value = ev.Value;
            NotBefore = ev.NotBefore;
            NotAfter = ev.NotAfter;
            CultureName = ev.CultureName;
            Preferred = ev.Preferred;
            ContentType = ev.ContentType;
            Where = ev.Where;
        }

        public ContactPropertySnapshot CreateSnapshot()
        {
            return new ContactPropertySnapshot
            {
                PropertyId = PropertyId,
                Type = Type,
                Subtype = Subtype,
                Value = Value,
                NotBefore = NotBefore,
                NotAfter = NotAfter,
                CultureName = CultureName,
                Preferred = Preferred,
                ContentType = ContentType,
                Where = Where,

            };
        }
    }
}