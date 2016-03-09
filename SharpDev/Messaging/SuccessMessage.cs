using SharpDev.Annotations;

namespace SharpDev.Messaging
{
    [DomainMessage(nameof(SuccessMessage), "Success", MessageConstants.SuccessMessageId, "0.1")]
    public class SuccessMessage
    {
        public object Result { get; set; }
    }
}
