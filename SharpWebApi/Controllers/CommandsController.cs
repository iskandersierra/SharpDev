using System.Threading.Tasks;
using System.Web.Http;

namespace SharpWebApi.Controllers
{
    [RoutePrefix("api")]
    public class CommandsController : ApiController
    {
        [Route("command/{command}")]
        public async Task<IHttpActionResult> Post([FromUri] string command)
        {
            // This action receives a command and send it to be processed
            return Ok();
        }
    }
}