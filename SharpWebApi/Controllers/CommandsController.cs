using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using SharpDev.AspNet46;
using SharpDev.EventSourcing;
using SharpDev.Messaging;

namespace SharpWebApi.Controllers
{
    [RoutePrefix("api")]
    public class CommandsController : ApiController
    {
        public IHttpTenantFinder TenantFinder { get; set; }

        public IHttpCommandParserProvider ParserProvider { get; set; }

        public ICommandTranslatorProvider TranslatorProvider { get; set; }

        public ICommandSenderProvider CommandSenderProvider { get; set; }

        [Route("command")]
        public async Task<IHttpActionResult> Post(CancellationToken ct)
        {
            /**
            - TenantFinder:  HttpRequest x HttpRequestContext -> Tenant
            - CommandParser: Tenant -> HttpRequest -> ParsedCommand x CommandType x Version
            - Translator:    Tenant -> ParsedCommand x CommandType x Version -> CommandEnvelope
            - Sender: Tenant -> CommandEnvelope -> HttpResult
            */

            // Get tenantId for current request
            var tenantId = await TenantFinder.FindTenantAsync(Request, RequestContext, ct);
            if (tenantId == null)
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Forbidden, "Unknown Tenant".ToModelStateErrors()));

            //var multi = await Request.Content.ReadAsMultipartAsync();

            // Parse incomming request as a command schema
            var parser = await ParserProvider.GetParserAsync(tenantId, ct);
            if (parser == null)
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, "Can not parse request".ToModelStateErrors()));

            var parsedCommandResult = await parser.ParseAsync(Request, RequestContext, ct);
            if (!parsedCommandResult.Success)
                return BadRequest(parsedCommandResult.Errors.ToModelStateErrors());

            // Validate parsed command
            var translator = await TranslatorProvider.GetTranslatorAsync(tenantId, ct);
            if (translator == null)
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, "Can not recognize command".ToModelStateErrors()));

            var translation = await translator.TranslateAsync(parsedCommandResult, ct);
            if (translation.Success)
            {
                // Now send command through the ICommandSender
                var sender = await CommandSenderProvider.GetSenderAsync(tenantId, ct);
                if (sender == null)
                    return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError,
                            "Can not start processing command".ToModelStateErrors()));

                var sendResult = await sender.SendCommandAsync(translation.CommandEnvelope, ct);
                if (sendResult.Success)
                {
                    switch (sendResult.Type)
                    {
                        case SendCommandResultType.Completed:
                            return Ok();
                        case SendCommandResultType.Accepted:
                            return StatusCode(HttpStatusCode.Accepted);
                        default:
                            return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError,
                                "Invalid response processing command".ToModelStateErrors()));
                    }
                }
                else
                {
                    switch (sendResult.Type)
                    {
                        case SendCommandResultType.ValidationFailed:
                            return BadRequest(sendResult.Errors.ToModelStateErrors());
                        case SendCommandResultType.BusinessPreconditionFailed:
                            return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Forbidden, sendResult.Errors.ToModelStateErrors()));
                        case SendCommandResultType.ProcessingError:
                            return InternalServerError(sendResult.Exception);
                        case SendCommandResultType.BusinessPostconditionFailed:
                            return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Forbidden, sendResult.Errors.ToModelStateErrors()));
                        case SendCommandResultType.PersistenceFailed:
                            return InternalServerError(sendResult.Exception);
                        default:
                            return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError,
                                "Invalid response processing command".ToModelStateErrors()));
                    }
                }
            }
            else
            {
                switch (translation.Type)
                {
                    case TranslationResultType.BadRequest:
                        return BadRequest(translation.ModelState.ToModelStateErrors());
                    case TranslationResultType.NotFound:
                        return NotFound();
                    case TranslationResultType.Obsolete:
                        return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Gone, translation.ModelState.ToModelStateErrors()));
                    default:
                        return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid response translating command".ToModelStateErrors()));
                }
            }
        }
    }
}