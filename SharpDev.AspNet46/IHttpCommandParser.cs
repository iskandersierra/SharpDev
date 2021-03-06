using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using SharpDev.Serialization;

namespace SharpDev.AspNet46
{
    public interface IHttpCommandParser
    {
        Task<ParseCommandResult> ParseAsync(HttpRequestMessage request, HttpRequestContext requestContext, CancellationToken ct);
    }
}