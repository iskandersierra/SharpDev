namespace SharpContacts.Commands
{
    public static class ContactsCommandsConstants
    {
        public const string ActivateContactUri = ContactsConstants.CommandsBaseUri + "activate-contact";
        public const string DeactivateContactUri = ContactsConstants.CommandsBaseUri + "deactivate-contact";
        public const string SetContactTypeUri = ContactsConstants.CommandsBaseUri + "set-contact-type";
        public const string UnsetContactTypeUri = ContactsConstants.CommandsBaseUri + "unset-contact-type";
        public const string SetContactPropertyUri = ContactsConstants.CommandsBaseUri + "set-contact-property";
        public const string UnsetContactPropertyUri = ContactsConstants.CommandsBaseUri + "unset-contact-property";
    }
}
