﻿using SharpDev.Annotations;

namespace SharpContacts.Commands
{
    [DomainCommand(nameof(UnsetContactType), "Unset contact type", ContactsCommandsConstants.UnsetContactTypeUri, "0.1")]
    public class UnsetContactType : ContactCommand
    {
        public string ContactType { get; set; }
    }
}