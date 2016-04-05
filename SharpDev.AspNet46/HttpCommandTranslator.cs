using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace SharpDev.AspNet46
{
    public class HttpCommandTranslator : IHttpCommandTranslator
    {
        public IHttpCommandMediaTypeFinder MediaTypeFinder { get; set; }

        public HttpConfiguration HttpConfiguration { get; set; }

        public async Task<TranslationResult> TranslateCommandAsync(HttpRequestMessage request, HttpRequestContext requestContext, CancellationToken ct)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            /**
            - PrincipalFinder: HttpRequest x HttpRequestContext -> Tenant
            - ProcessSchema: Tenant -> HttpRequest -> StructuredCommand x CommandTypeInfo (type x version)
            - Translate: Tenant -> CommandTypeInfo -> CommandEnvelope
            */

            var mediaType = await MediaTypeFinder.FindMediaTypeAsync(request, ct);

            var modelState = new ModelStateDictionary();

            if (mediaType == null)
            {
                modelState.AddModelError("", "Could not find media type");
            }
            else
            {
                if (mediaType.DomainModel == null)
                    modelState.AddModelError("", "Missing domain model");
                if (mediaType.Version == null)
                    modelState.AddModelError("", "Missing domain model version");
            }
            if (!modelState.IsValid)
                return TranslationResult.UnsupportedMediaType(modelState);

        }
    }
}