using SharpDev.Annotations;

namespace SharpContacts.Events
{
    [DomainEvent(nameof(ContactPropertyUnset), "Contact property has been unset", ContactsEventsConstants.ContactPropertyUnsetUri, "0.1")]
    public class ContactPropertyUnset : ContactPropertyEvent
    {
    }
}