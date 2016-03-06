using System;
using System.Collections.Generic;
using System.Linq;
using SharpContacts.Events;
using SharpDev.Annotations;

namespace SharpContacts.Aggregates
{
    [DomainAggregate(nameof(Contact), "Contact", ContactsAggregatesConstants.ContactUri, "0.1")]
    public class Contact
    {
        public Contact()
        {
            Types = new HashSet<string>();
            Properties = new Dictionary<string, ContactProperty>();
        }

        public string ContactId { get; private set; }
        public bool IsActive { get; private set; }
        public HashSet<string> Types { get; private set; }
        public Dictionary<string, ContactProperty> Properties { get; private set; }

        #region [ Event rehydration ]

        public void On(ContactActivated ev)
        {
            IsActive = true;
        }

        public void On(ContactDeactivated ev)
        {
            IsActive = false;
        }

        public void On(ContactTypeSet ev)
        {
            Types.Add(ev.ContactType);
        }

        public void On(ContactTypeUnset ev)
        {
            Types.Remove(ev.ContactType);
        }

        public void On(ContactPropertySet ev)
        {
            ContactProperty property;
            if (!Properties.TryGetValue(ev.PropertyId, out property))
            {
                Properties.Add(ev.PropertyId, property = new ContactProperty());
            }
            property.On(ev);
        }

        public void On(ContactPropertyUnset ev)
        {
            Properties.Remove(ev.PropertyId);
        }

        #endregion

        #region [ Snapshoting ]

        public ContactSnapshot CreateSnapshot()
        {
            return new ContactSnapshot
            {
                ContactId = ContactId,
                IsActive = IsActive,
                Types = Types.Any() ? Types.ToArray() : null,
                Properties = Properties.Any() ? Properties.Values.Select(p => p.CreateSnapshot()).ToArray() : null,
            };
        }

        public void FromSnapshot(ContactSnapshot snapshot)
        {
            ContactId = snapshot.ContactId;
            IsActive = snapshot.IsActive;
            Types = new HashSet<string>(snapshot.Types ?? Enumerable.Empty<string>());
            Properties = snapshot.Properties?.ToDictionary(p => p.PropertyId, p => new ContactProperty(p)) 
                ?? new Dictionary<string, ContactProperty>();
        }

        #endregion
    }

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

    [DomainAggregate(nameof(ContactSnapshot), "Contact snapshot", ContactsSnapshotsConstants.ContactUri, "0.1")]
    public class ContactSnapshot
    {
        public string ContactId { get; set; }
        public bool IsActive { get; set; }
        public string[] Types { get; set; }
        public ContactPropertySnapshot[] Properties { get; set; }
    }

    public class ContactPropertySnapshot
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
