using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SharpDev.AspNet46
{
    public interface IHttpCommandMediaTypeFinder
    {
        Task<MediaTypeInfo> FindMediaTypeAsync(HttpRequestMessage request, CancellationToken ct);
    }
}