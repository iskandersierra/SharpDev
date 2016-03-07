using SharpDev.Annotations;

namespace SharpContacts.Commands
{
    public abstract class ContactPropertyCommand : ContactCommand
    {
        [EntityIdentifier]
        public string PropertyId { get; set; }
    }
}