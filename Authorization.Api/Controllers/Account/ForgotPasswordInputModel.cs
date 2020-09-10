using System;
using System.ComponentModel.DataAnnotations;

namespace Authorization.Api.Controllers
{
    public class ForgotPasswordInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
