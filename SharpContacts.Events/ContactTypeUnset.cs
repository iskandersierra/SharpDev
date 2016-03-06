using SharpDev.Annotations;

namespace SharpContacts.Events
{
    [DomainEvent(nameof(ContactTypeUnset), "Contact type has been unset", ContactsEventsConstants.ContactTypeUnsetUri, "0.1")]
    public class ContactTypeUnset : ContactEvent
    {
        public string ContactType { get; set; }
    }
}