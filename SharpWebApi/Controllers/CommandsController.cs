using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using SharpDev.AspNet46;

namespace SharpWebApi.Controllers
{
    [RoutePrefix("api")]
    public class CommandsController : ApiController
    {
        public IHttpCommandTranslator CommandTranslator { get; set; }

        [Route("command")]
        public async Task<IHttpActionResult> Post(CancellationToken ct)
        {
            var result = await CommandTranslator.TranslateCommandAsync(Request, RequestContext, ct);

            return BadRequest(ModelState);

            return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType, 1234, GlobalConfiguration.Configuration.Formatters.JsonFormatter));

            return StatusCode(HttpStatusCode.Accepted);
            // This action receives a command and send it to be processed
            return Ok();
        }
    }
}