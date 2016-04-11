using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace SharpDev.AspNet46
{
    public class DefaultHttpCommandParser : IHttpCommandParser
    {
        public Task<ParseCommandResult> ParseAsync(HttpRequestMessage request, HttpRequestContext requestContext, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
