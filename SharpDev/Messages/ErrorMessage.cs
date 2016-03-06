using SharpDev.Annotations;

namespace SharpDev.Messages
{
    [DomainMessage(nameof(ErrorMessage), "Error", MessageConstants.ErrorMessageId, "0.1")]
    public class ErrorMessage
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public ValidationError[] ValidationErrors { get; set; }
    }
}