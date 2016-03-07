using SharpDev.Annotations;

namespace SharpContacts.Events
{
    public abstract class ContactEvent
    {
        [AggregateIdentifier]
        public string ContactId { get; set; }
    }
}
