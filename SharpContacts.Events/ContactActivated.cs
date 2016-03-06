using SharpDev.Annotations;

namespace SharpContacts.Events
{
    [DomainEvent(nameof(ContactActivated), "Contact has been activated", ContactsEventsConstants.ContactActivatedUri, "0.1")]
    public class ContactActivated : ContactEvent
    {
    }
}