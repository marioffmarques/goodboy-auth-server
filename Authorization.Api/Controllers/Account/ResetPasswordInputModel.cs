using System;
using System.ComponentModel.DataAnnotations;

namespace Authorization.Api.Controllers
{
    public class ResetPasswordInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
