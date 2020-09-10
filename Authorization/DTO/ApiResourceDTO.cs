using System;
using System.ComponentModel.DataAnnotations;

namespace Authorization.DTO
{
    public class ApiResourceDTO
    {
        public string Description { get; set; }

        public string DisplayName { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
