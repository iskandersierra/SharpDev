using SharpDev.Annotations;

namespace SharpContacts.Commands
{
    [DomainCommand(nameof(UnsetContactProperty), "Unset contact property", ContactsCommandsConstants.UnsetContactPropertyUri, "0.1")]
    public class UnsetContactProperty : ContactPropertyCommand
    {
    }
}