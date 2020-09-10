using System;
using System.Collections.Generic;

namespace Authorization.Resources.Api
{
    public class UnauthorizedResponse : ApiResponse
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

        public UnauthorizedResponse(string message, string exKey) : base(401)
        {
            ErrorKey = exKey;
            Errors = new List<string>() { message };

        }
    }
}
