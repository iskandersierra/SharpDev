using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpDev.Messaging
{
    public class SingleCommandTranslatorProvider : ICommandTranslatorProvider
    {
        public SingleCommandTranslatorProvider(ICommandTranslator commandTranslator)
        {
            if (commandTranslator == null) throw new ArgumentNullException(nameof(commandTranslator));
            CommandTranslator = commandTranslator;
        }

        public ICommandTranslator CommandTranslator { get; }

        public Task<ICommandTranslator> GetTranslatorAsync(string tenantId, CancellationToken ct)
        {
            return Task.FromResult(CommandTranslator);
        }
    }
}