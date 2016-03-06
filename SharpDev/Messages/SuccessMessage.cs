using SharpDev.Annotations;

namespace SharpDev.Messages
{
    [DomainMessage(nameof(SuccessMessage), "Success", MessageConstants.SuccessMessageId, "0.1")]
    public class SuccessMessage
    {
    }
}
