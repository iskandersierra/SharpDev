using System;

namespace SharpContacts.Commands
{
    public abstract class ContactCommand
    {
        public string UniqueId { get; set; }
    }
}