using SharpDev.Annotations;

namespace SharpContacts.Events
{
    public abstract class ContactPropertyEvent : ContactEvent
    {
        [EntityIdentifier]
        public string PropertyId { get; set; }
    }
}