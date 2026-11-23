using System.ComponentModel.DataAnnotations;

namespace RealEstatePortfolio.Models.ViewModels
{
    public class ContactFormViewModel
    {
        [Required(ErrorMessage = "Your name is required")]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string ClientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(150)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; } = string.Empty;

        [Phone]
        [StringLength(30)]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Message cannot be empty")]
        [StringLength(2000)]
        public string Message { get; set; } = string.Empty;

        public string? PreferredProjectSlug { get; set; }

        [Display(Name = "Concern / Subsidiary")]
        public string? TargetedConcern { get; set; } = "Recent Group";
    }
}
