using System;
using System.ComponentModel.DataAnnotations;

namespace Authorization.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }

        public Guid TenantId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(5)]
        public string Password { get; set; }
    }
}
