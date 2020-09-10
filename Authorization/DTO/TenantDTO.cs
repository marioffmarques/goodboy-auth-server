using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Authorization.DTO
{
    public class TenantDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;

        public string Host { get; set; }
    }
}
