using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using RealEstatePortfolio.Models.Entities;

namespace RealEstatePortfolio.Models.ViewModels
{
    public class AdminProjectEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Building Name is required")]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "URL Slug is required")]
        [StringLength(150)]
        [RegularExpression(@"^[a-z0-9-]+$", ErrorMessage = "Slug must contain lowercase alphanumeric characters and hyphens only")]
        public string Slug { get; set; } = string.Empty;

        [Display(Name = "Full Address")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Latitude is required")]
        [Range(-90.0, 90.0)]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is required")]
        [Range(-180.0, 180.0)]
        public double Longitude { get; set; }

        [Required]
        public ProjectStatus Status { get; set; } = ProjectStatus.Ongoing;

        [Display(Name = "Completion Year")]
        [Range(1900, 2100)]
        public int? CompletionYear { get; set; }

        [Display(Name = "Structural Features (comma-separated)")]
        public string? StructuralFeatures { get; set; }

        [Display(Name = "Detailed Description (Rich Text)")]
        public string? LongDescription { get; set; }

        [Display(Name = "Existing Brochure PDF")]
        public string? BrochurePdfPath { get; set; }

        [Display(Name = "Upload New Brochure (PDF)")]
        public IFormFile? BrochureFile { get; set; }

        [StringLength(65)]
        [Display(Name = "SEO Meta Title (max 65 chars)")]
        public string? SeoTitle { get; set; }

        [StringLength(160)]
        [Display(Name = "SEO Meta Description (max 160 chars)")]
        public string? SeoDescription { get; set; }

        [Display(Name = "Sister Concern / Division")]
        public string? SisterConcern { get; set; } = "Recent Properties Ltd";

        [Display(Name = "Property Type")]
        public string? PropertyType { get; set; } = "Residential";

        [Display(Name = "Starting Price (USD)")]
        [Range(0, 1000000000)]
        public decimal? PriceStartingFrom { get; set; }

        [Display(Name = "Total Units")]
        public int? TotalUnits { get; set; }

        public List<BuildingImage> ExistingImages { get; set; } = new List<BuildingImage>();

        [Display(Name = "Upload Images")]
        public List<IFormFile>? NewImages { get; set; }
    }
}
