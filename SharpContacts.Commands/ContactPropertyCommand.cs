namespace SharpContacts.Commands
{
    public abstract class ContactPropertyCommand : ContactCommand
    {
        public string PropertyId { get; set; }
    }
}