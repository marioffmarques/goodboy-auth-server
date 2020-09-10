using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Authorization.Api
{
    public class BadRequestResponse : ApiResponse
    {
        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>The errors.</value>
        public IEnumerable<string> Errors { get; }

        /// <summary>
        /// Weidert Api ErrorKey
        /// </summary>
        /// <value>The error key.</value>
        public string ErrorKey { get; }


        public BadRequestResponse(ModelStateDictionary modelState, string exKey, string message = null) : base(400)
        {
            ErrorKey = exKey;

            if (modelState != null)
            {
                Errors = modelState.AllErrors();
                this.Errors = modelState.Select(x => x.Key); // .SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage).ToArray();
            }
            else if (message != null)
            {
                Errors = new List<string>() { message };
            }
        }
    }

    public static class ModelStateExtensions
    {
        public static IEnumerable<string> AllErrors(this ModelStateDictionary modelState)
        {
            var result = new List<string>();
            var erroneousFields = modelState.Where(ms => ms.Value.Errors.Any())
                                            .Select(x => new { x.Key, x.Value.Errors });

            foreach (var erroneousField in erroneousFields)
            {
                var fieldKey = erroneousField.Key;
                var fieldErrors = erroneousField.Errors.First().ErrorMessage;
                result.Add(fieldKey + " : " + fieldErrors);
            }

            return result;
        }
    }
}
