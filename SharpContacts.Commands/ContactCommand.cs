using SharpDev.Annotations;

namespace SharpContacts.Commands
{
    public abstract class ContactCommand
    {
        [AggregateIdentifier]
        public string ContactId { get; set; }
    }
}