using SharpDev.Annotations;

namespace SharpDev.Messaging
{
    [DomainMessage(nameof(AcceptMessage), "Accept", MessageConstants.AcceptMessageId, "0.1")]
    public class AcceptMessage
    {
    }
}