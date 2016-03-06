using SharpDev.Annotations;

namespace SharpContacts.Events
{
    [DomainEvent(nameof(ContactTypeSet), "Contact type has been set", ContactsEventsConstants.ContactTypeSetUri, "0.1")]
    public class ContactTypeSet : ContactEvent
    {
        public string ContactType { get; set; }
    }
}