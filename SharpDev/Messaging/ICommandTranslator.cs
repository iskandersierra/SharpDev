using System.Threading;
using System.Threading.Tasks;

namespace SharpDev.Messaging
{
    public interface ICommandTranslator
    {
        Task<TranslationResult> TranslateAsync(ParseCommandResult parsedCommand, CancellationToken ct);
    }
}