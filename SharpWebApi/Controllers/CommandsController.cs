using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using SharpDev.AspNet46;
using SharpDev.Messaging;

namespace SharpWebApi.Controllers
{
    [RoutePrefix("api")]
    public class CommandsController : ApiController
    {
        public IHttpTenantFinder TenantFinder { get; set; }

        public IHttpCommandParserProvider ParserProvider { get; set; }

        public ICommandTranslatorProvider TranslatorProvider { get; set; }

        [Route("command")]
        public async Task<IHttpActionResult> Post(CancellationToken ct)
        {
            /**
            - TenantFinder: HttpRequest x HttpRequestContext -> Tenant
            - CommandParser: Tenant -> HttpRequest -> ParsedCommand x CommandType x Version
            - Translate: Tenant -> ParsedCommand x CommandType x Version -> CommandEnvelope
            */
            var watch = Stopwatch.StartNew();

            var tenantId = await TenantFinder.FindTenantAsync(Request, RequestContext, ct);
            if (tenantId == null)
                return StatusCode(HttpStatusCode.Forbidden);

            var parser = await ParserProvider.GetParserAsync(tenantId, ct);
            if (parser == null)
                return Unauthorized();

            var parsedCommandResult = await parser.ParseAsync(Request, RequestContext, ct);
            if (!parsedCommandResult.Success)
                return BadRequest(parsedCommandResult.Errors.ToModelStateErrors());

            var translator = await TranslatorProvider.GetTranslatorAsync(tenantId, ct);
            if (translator == null)
                return Unauthorized();

            var translation = await translator.TranslateAsync(parsedCommandResult, ct);
            if (translation.Success)
            {
                // Now send command through the ICommandSender

                watch.Stop();
                var elapsed = watch.Elapsed.ToString("G");
                return Ok($"Elapsed: {elapsed}");
            }

            switch (translation.Type)
            {
                case TranslationResultType.BadRequest:
                    return BadRequest(translation.ModelState.ToModelStateErrors());
                case TranslationResultType.NotFound:
                    return NotFound();
                case TranslationResultType.Obsolete:
                    return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Gone, translation.ModelState.ToModelStateErrors()));
                default:
                    return InternalServerError();
            }
        }
    }
}