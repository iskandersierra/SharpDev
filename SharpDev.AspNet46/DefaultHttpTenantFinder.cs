using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace SharpDev.AspNet46
{
    public class DefaultHttpTenantFinder : IHttpTenantFinder
    {
        public DefaultHttpTenantFinder(string tenantId)
        {
            if (tenantId == null) throw new ArgumentNullException(nameof(tenantId));

            TenantId = tenantId;
        }

        public string TenantId { get; }

        public Task<string> FindTenantAsync(HttpRequestMessage request, HttpRequestContext requestContext, CancellationToken ct)
        {
            return Task.FromResult(TenantId);
        }
    }
}