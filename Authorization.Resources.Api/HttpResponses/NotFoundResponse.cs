using System;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Resources.Api
{
    public class NotFoundResponse : ApiResponse
    {
        /// <summary>
        /// Weidert Api ErrorKey
        /// </summary>
        /// <value>The error key.</value>
        public string ErrorKey { get; }

        public NotFoundResponse(string exKey) : base(404)
        {
            ErrorKey = exKey;
        }
    }
}
