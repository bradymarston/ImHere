using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Services.Dtos
{
    public class StudentDto
    {
        public int Id { get; set; }
        public int StudentTypeId { get; set; } = 1;
        public bool IsMethodist { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Differentiator { get; set; }
        [Phone]
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string StudentTypeDescription { get; set; }

        public string DisplayName => $"{FirstName} {LastName}" + (string.IsNullOrWhiteSpace(Differentiator) ? "" : $" ({Differentiator})"); 
    }
}
