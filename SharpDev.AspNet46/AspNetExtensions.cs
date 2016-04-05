using System;
using System.Collections.Generic;
using System.Web.Http.ModelBinding;

namespace SharpDev.AspNet46
{
    public static class AspNetExtensions
    {
        public static ModelStateDictionary ToModelStateErrors(this IEnumerable<KeyValuePair<string, string>> pairs, ModelStateDictionary modelState = null)
        {
            modelState = modelState ?? new ModelStateDictionary();
            foreach (var pair in pairs)
                modelState.AddModelError(pair.Key, pair.Value);
            return modelState;
        }

        public static ModelStateDictionary ToModelStateErrors(this IEnumerable<Tuple<string, string>> pairs, ModelStateDictionary modelState = null)
        {
            modelState = modelState ?? new ModelStateDictionary();
            foreach (var pair in pairs)
                modelState.AddModelError(pair.Item1, pair.Item2);
            return modelState;
        }

        public static ModelStateDictionary ToModelStateErrors(this IEnumerable<string> errors, ModelStateDictionary modelState = null)
        {
            modelState = modelState ?? new ModelStateDictionary();
            foreach (var error in errors)
                modelState.AddModelError("", error);
            return modelState;
        }
    }
}
