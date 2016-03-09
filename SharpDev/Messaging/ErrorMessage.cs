using SharpDev.Annotations;

namespace SharpDev.Messaging
{
    [DomainMessage(nameof(ErrorMessage), "Error", MessageConstants.ErrorMessageId, "0.1")]
    public class ErrorMessage
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public ValidationError[] ValidationErrors { get; set; }
    }

    public class ValidationError
    {
        public string EntityId { get; set; }
        public string PropertyName { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public string RuleName { get; set; }
    }
}