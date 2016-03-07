using System.Collections.Generic;
using System.Linq;
using SharpContacts.Commands;
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

        #region [ Rehydration ]

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

        #region [ Command handling ]

        public IEnumerable<object> When(ActivateContact cmd)
        {
            if (!IsActive)
                yield return new ContactActivated {ContactId = cmd.ContactId};
        }

        public IEnumerable<object> When(DeactivateContact cmd)
        {
            if (IsActive)
                yield return new ContactDeactivated {ContactId = cmd.ContactId};
        }

        public IEnumerable<object> When(ContactTypeSet cmd)
        {
            if (!Types.Contains(cmd.ContactType))
                yield return new ContactTypeSet { ContactId = cmd.ContactId, ContactType = cmd.ContactType };
        }

        public IEnumerable<object> When(ContactTypeUnset cmd)
        {
            if (Types.Contains(cmd.ContactType))
                yield return new ContactTypeUnset { ContactId = cmd.ContactId, ContactType = cmd.ContactType };
        }

        public IEnumerable<object> When(SetContactProperty cmd)
        {
            yield return new ContactPropertySet
            {
                ContactId = cmd.ContactId,
                PropertyId = cmd.PropertyId,
                Type = cmd.Type,
                Subtype = cmd.Subtype,
                Value = cmd.Value,
                NotBefore = cmd.NotBefore,
                NotAfter = cmd.NotAfter,
                CultureName = cmd.CultureName,
                Preferred = cmd.Preferred,
                ContentType = cmd.ContentType,
                Where = cmd.Where,
            };
        }

        public IEnumerable<object> When(UnsetContactProperty cmd)
        {
            if (Properties.ContainsKey(cmd.PropertyId))
            {
                yield return new ContactPropertyUnset {ContactId = cmd.ContactId, PropertyId = cmd.PropertyId};
            }
        }

        #endregion
    }
}
