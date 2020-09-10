using System;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Api
{
    public class OkResponse : ApiResponse
    {
        public OkResponse(object data, int total = 0) : base(200, data, total: total)
        {

        }
    }
}
