namespace SharpContacts.Events
{
    public abstract class ContactPropertyEvent : ContactEvent
    {
        public string PropertyId { get; set; }
    }
}