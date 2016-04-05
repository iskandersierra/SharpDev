using System.Threading;
using System.Threading.Tasks;

namespace SharpDev.AspNet46
{
    public interface IHttpCommandParserProvider
    {
        Task<IHttpCommandParser> GetParserAsync(string tenantId, CancellationToken ct);
    }
}