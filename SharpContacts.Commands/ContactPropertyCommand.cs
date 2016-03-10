using FluentValidation;
using SharpDev.Annotations;

namespace SharpContacts.Commands
{
    public abstract class ContactPropertyCommand : ContactCommand
    {
        [EntityIdentifier]
        public string PropertyId { get; set; }
    }

    public class ContactPropertyCommandValidator : AbstractValidator<ContactPropertyCommand>
    {
        public ContactPropertyCommandValidator()
        {
            RuleFor(e => e.PropertyId).NotEmpty();
        }
    }
}