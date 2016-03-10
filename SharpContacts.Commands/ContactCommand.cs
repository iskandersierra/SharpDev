using FluentValidation;
using SharpDev.Annotations;

namespace SharpContacts.Commands
{
    public abstract class ContactCommand
    {
        [AggregateIdentifier]
        public string ContactId { get; set; }
    }

    public class ContactCommandValidator : AbstractValidator<ContactCommand>
    {
        public ContactCommandValidator()
        {
            RuleFor(e => e.ContactId).NotEmpty();
        }
    }
}