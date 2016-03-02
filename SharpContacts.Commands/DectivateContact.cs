using SharpDev.Annotations;

namespace SharpContacts.Commands
{
    [DomainCommand(nameof(DectivateContact), "Deactivate a contact", ContactsCommandsConstants.DeactivateContactUri, "0.1")]
    public class DectivateContact : ContactCommand
    {
    }
}