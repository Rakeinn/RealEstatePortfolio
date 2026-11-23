using System.ComponentModel.DataAnnotations;

namespace RealEstatePortfolio.Models.Entities
{
    public class ContactLead
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ClientName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string EmailAddress { get; set; } = string.Empty;

        [Phone]
        [StringLength(30)]
        public string? PhoneNumber { get; set; }

        [Required]
        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;

        public string? PreferredProjectSlug { get; set; }

        public string? TargetedConcern { get; set; } = "Recent Group Corporate";
    }
}
