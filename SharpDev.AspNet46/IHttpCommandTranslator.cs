using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace SharpDev.AspNet46
{
    public interface IHttpCommandTranslator
    {
        Task<TranslationResult> TranslateCommandAsync(HttpRequestMessage request, HttpRequestContext requestContext, CancellationToken ct);
    }
}
