namespace SharpContacts.Events
{
    public static class ContactsEventsConstants
    {
        public const string ContactActivatedUri = ContactsConstants.EventsBaseUri + "contact-activated";
        public const string ContactDeactivatedUri = ContactsConstants.EventsBaseUri + "contact-deactivated";
        public const string ContactTypeSetUri = ContactsConstants.EventsBaseUri + "contact-type-set";
        public const string ContactTypeUnsetUri = ContactsConstants.EventsBaseUri + "contact-type-unset";
        public const string ContactPropertySetUri = ContactsConstants.EventsBaseUri + "contact-property-set";
        public const string ContactPropertyUnsetUri = ContactsConstants.EventsBaseUri + "contact-property-unset";
    }
}