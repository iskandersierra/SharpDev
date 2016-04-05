using System.Web.Http.ModelBinding;
using SharpDev.Messaging;

namespace SharpDev.AspNet46
{
    public class TranslationResult
    {
        public static TranslationResult Ok(CommandEnvelope command)
        {
            return new TranslationResult(command, TranslationResultType.Ok, null);
        }

        public static TranslationResult NewVersionSuggested(CommandEnvelope command, string suggestedVersion)
        {
            return new TranslationResult(command, TranslationResultType.NewVersionSuggested, suggestedVersion);
        }

        public static TranslationResult UnsupportedMediaType(ModelStateDictionary modelState)
        {
            return new TranslationResult(TranslationResultType.UnsupportedMediaType, modelState, null);
        }

        public static TranslationResult BadRequest(ModelStateDictionary modelState)
        {
            return new TranslationResult(TranslationResultType.BadRequest, modelState, null);
        }

        public static TranslationResult NotFound()
        {
            return new TranslationResult(TranslationResultType.NotFound, null, null);
        }

        public static TranslationResult Obsolete(string suggestedVersion)
        {
            return new TranslationResult(TranslationResultType.Obsolete, null, suggestedVersion);
        }

        private TranslationResult(CommandEnvelope commandEnvelope, TranslationResultType type, string suggestedVersion)
        {
            Type = type;
            CommandEnvelope = commandEnvelope;
            SuggestedVersion = suggestedVersion;
        }

        private TranslationResult(TranslationResultType type, ModelStateDictionary modelState, string suggestedVersion)
        {
            Type = type;
            ModelState = modelState;
            SuggestedVersion = suggestedVersion;
        }

        public bool Success => CommandEnvelope != null;
        public TranslationResultType Type { get; }
        public CommandEnvelope CommandEnvelope { get; }
        public ModelStateDictionary ModelState { get; }
        public string SuggestedVersion { get; }
    }
}