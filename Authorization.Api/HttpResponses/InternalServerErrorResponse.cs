using System;
using System.Collections.Generic;

namespace Authorization.Api
{
    public class InternalServerErrorResponse : ApiResponse
    {
        /// <summary>
        /// Gets the errors associated with .
        /// </summary>
        /// <value>The errors.</value>
        public IEnumerable<string> Errors { get; }

        /// <summary>
        /// Weidert Api ErrorKey
        /// </summary>
        /// <value>The error key.</value>
        public string ErrorKey { get; }


        public InternalServerErrorResponse(string message, string exKey) : base(500)
        {
            ErrorKey = exKey;
            Errors = new List<string>() { message };
        }
    }
}
