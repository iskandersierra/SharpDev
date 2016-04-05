using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpDev.AspNet46
{
    public class SingleHttpCommandParserProvider : IHttpCommandParserProvider
    {
        public SingleHttpCommandParserProvider(IHttpCommandParser commandParser)
        {
            if (commandParser == null) throw new ArgumentNullException(nameof(commandParser));
            CommandParser = commandParser;
        }

        public IHttpCommandParser CommandParser { get; }

        public Task<IHttpCommandParser> GetParserAsync(string tenantId, CancellationToken ct)
        {
            return Task.FromResult(CommandParser);
        }
    }
}