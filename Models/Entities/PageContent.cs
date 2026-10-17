using System.ComponentModel.DataAnnotations;

namespace RealEstatePortfolio.Models.Entities
{
    public class PageContent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string PageName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string SectionKey { get; set; } = string.Empty;

        public string? ContentValue { get; set; }
    }
}
