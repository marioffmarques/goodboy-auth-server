using System;
using System.Collections.Generic;

namespace Authorization.Api
{
    public class ConflictResponse : ApiResponse
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


        public ConflictResponse(string exKey, string message = null) : base(409)
        {
            ErrorKey = exKey;

            if (message != null)
            {
                Errors = new List<string> { message };
            }
        }
    }
}
