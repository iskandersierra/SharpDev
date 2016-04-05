using System.Threading;
using System.Threading.Tasks;

namespace SharpDev.Messaging
{
    public interface ICommandTranslatorProvider
    {
        Task<ICommandTranslator> GetTranslatorAsync(string tenantId, CancellationToken ct);
    }
}