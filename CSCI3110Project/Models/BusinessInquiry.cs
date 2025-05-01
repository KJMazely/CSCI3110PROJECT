using System;
using System.ComponentModel.DataAnnotations;

namespace KMCSCI3110Project.Models
{
    public class BusinessInquiry
    {
        public int Id { get; set; }

        [Required]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(1000)]
        public string Inquiry { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
