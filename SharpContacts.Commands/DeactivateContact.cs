using SharpDev.Annotations;

namespace SharpContacts.Commands
{
    [DomainCommand(nameof(DeactivateContact), "Deactivate a contact", ContactsCommandsConstants.DeactivateContactUri, "0.1")]
    public class DeactivateContact : ContactCommand
    {
    }
}