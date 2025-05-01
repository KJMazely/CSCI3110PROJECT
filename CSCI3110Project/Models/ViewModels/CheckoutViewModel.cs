using System;
using System.ComponentModel.DataAnnotations;

namespace KMCSCI3110Project.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public int VehicleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalCost { get; set; }

        // Payment fields
        [Required]
        [CreditCard]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }

        [Required]
        [Display(Name = "Expiration Date")]
        public string Expiration { get; set; }

        [Required]
        [Display(Name = "CVV")]
        public string CVV { get; set; }

        [Required]
        [Display(Name = "Name on Card")]
        public string NameOnCard { get; set; }
    }
}
