using SharpDev.Annotations;

namespace SharpContacts.Events
{
    [DomainEvent(nameof(ContactDeactivated), "Contact has been dectivated", ContactsEventsConstants.ContactDeactivatedUri, "0.1")]
    public class ContactDeactivated : ContactEvent
    {
    }
}