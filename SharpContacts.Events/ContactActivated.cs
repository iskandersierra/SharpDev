using SharpDev.Annotations;

namespace SharpContacts.Events
{
    [DomainEvent(nameof(ContactActivated), "Contact has been activated", ContactsEventsConstants.ContactActivatedUri, "0.1", IsCreation = true)]
    public class ContactActivated : ContactEvent
    {
    }
}