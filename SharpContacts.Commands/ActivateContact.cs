using SharpDev.Annotations;

namespace SharpContacts.Commands
{
    [DomainCommand(nameof(ActivateContact), "Activate a contact", ContactsCommandsConstants.ActivateContactUri, "0.1", IsCreation = true)]
    public class ActivateContact : ContactCommand
    {
    }
}
