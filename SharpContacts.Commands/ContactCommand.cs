using System;

namespace SharpContacts.Commands
{
    public abstract class ContactCommand
    {
        public string ContactId { get; set; }
    }
}