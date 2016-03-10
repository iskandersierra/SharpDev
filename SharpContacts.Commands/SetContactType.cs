using FluentValidation;
using SharpDev.Annotations;

namespace SharpContacts.Commands
{
    [DomainCommand(nameof(SetContactType), "Set contact type", ContactsCommandsConstants.SetContactTypeUri, "0.1")]
    public class SetContactType : ContactCommand
    {
        public string ContactType { get; set; }
    }

    public class SetContactTypeValidator : AbstractValidator<SetContactType>
    {
        public SetContactTypeValidator()
        {
            RuleFor(e => e.ContactType).NotEmpty();
        }
    }
}