using System;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Api
{
    public class CreatedResponse : ApiResponse
    {
        public CreatedResponse(object data, int total = 0) : base(201, data, total: total)
        {
        }
    }
}

