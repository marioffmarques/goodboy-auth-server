using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Authorization.Resources.Api
{
    public class ApiResponse
    {
        /// <summary>
        /// Status Code
        /// </summary>
        /// <value>The time.</value>
        public int StatusCode { get; }

        /// <summary>
        /// Data
        /// </summary>
        /// <value>The time.</value>
        public object Data { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }

        /// <summary>
        /// Current server time
        /// </summary>
        /// <value>The time.</value>
        public long Time { get; }

        /// <summary>
        /// Total elements retrived / Total elements presented on a paginated list
        /// </summary>
        /// <value>The total.</value>
        public int Total { get; }


        public ApiResponse(int statusCode, object data = null, string message = null, int total = 0)
        {
            StatusCode = statusCode;
            Data = data;
            Message = message;
            Time = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeMilliseconds();
            Total = total;
        }
    }
}
