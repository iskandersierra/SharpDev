using SharpDev.Annotations;

namespace SharpContacts.Aggregates
{
    [DomainAggregate(nameof(ContactSnapshot), "Contact snapshot", ContactsSnapshotsConstants.ContactUri, "0.1")]
    public class ContactSnapshot
    {
        public string ContactId { get; set; }
        public bool IsActive { get; set; }
        public string[] Types { get; set; }
        public ContactPropertySnapshot[] Properties { get; set; }
    }
}