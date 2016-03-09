namespace SharpContacts
{
    public static class ContactsConstants
    {
        public const string BaseUri = "http://schemas.imodelsoft.com/";
        public const string ContactsBaseUri = "contact/";

        public const string CommandsBaseUri = ContactsBaseUri + "command/";
        public const string EventsBaseUri = ContactsBaseUri + "event/";
        public const string AggregatesBaseUri = ContactsBaseUri + "aggregate/";
        public const string SnapshotsBaseUri = ContactsBaseUri + "snapshot/";
        public const string ProjectionsBaseUri = ContactsBaseUri + "projection/";
        public const string ProcessesBaseUri = ContactsBaseUri + "process/";
        public const string QueriesBaseUri = ContactsBaseUri + "query/";
    }
}
