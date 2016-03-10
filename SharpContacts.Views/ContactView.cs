using SharpDev.Annotations;

namespace SharpContacts.Views
{
    [DomainView(nameof(ContactView), "Contact", ContactsViewsConstants.ContactViewUri, "0.1")]
    public class ContactView
    {
        public string ContactId { get; set; }
        public string[] Types { get; set; }
        public string DisplayName { get; set; }
        public ContactPropertyView[] Properties { get; set; }
    }
}