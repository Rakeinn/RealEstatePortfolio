using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstatePortfolio.Models.Entities
{
    public class Building
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Slug { get; set; } = string.Empty; // SEO URL

        public string? Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public ProjectStatus Status { get; set; }
        public int? CompletionYear { get; set; }
        public string? StructuralFeatures { get; set; }
        public string? LongDescription { get; set; } // Rich Text
        public string? BrochurePdfPath { get; set; } // Downloadable PDF

        [StringLength(65)]
        public string? SeoTitle { get; set; }

        [StringLength(160)]
        public string? SeoDescription { get; set; }

        // Sister Concern / Subsidiary
        [StringLength(100)]
        public string? SisterConcern { get; set; } = "Recent Properties Ltd"; // Recent Properties Ltd, Recent Living Ltd, Recent Living Interior

        // Additional helpful fields for rich portfolio rendering
        public string? PropertyType { get; set; } = "Residential"; // Residential, Commercial, Mix
        public decimal? PriceStartingFrom { get; set; }
        public int? TotalUnits { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Detailed Property Highlights & Apartment Specifications
        [StringLength(100)]
        public string? HandoverDate { get; set; }

        [StringLength(100)]
        public string? Facing { get; set; }

        [StringLength(100)]
        public string? Storied { get; set; }

        [StringLength(200)]
        public string? ApartmentSize { get; set; }

        [StringLength(100)]
        public string? LandSize { get; set; }

        public string? ApartmentContains { get; set; }

        // Neighborhood Connectivity (newline-separated)
        public string? NeighborhoodSchools { get; set; }
        public string? NeighborhoodHospitals { get; set; }
        public string? NeighborhoodTransit { get; set; }

        public virtual ICollection<BuildingImage> Images { get; set; } = new List<BuildingImage>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        [NotMapped]
        public BuildingImage? PrimaryImage => Images?.OrderBy(i => i.DisplayOrder).FirstOrDefault(i => i.IsPrimary) ?? Images?.OrderBy(i => i.DisplayOrder).FirstOrDefault();

        [NotMapped]
        public List<string> FeatureList => string.IsNullOrWhiteSpace(StructuralFeatures) 
            ? new List<string>() 
            : StructuralFeatures.Split(new[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();

        [NotMapped]
        public List<string> ApartmentContainsList => string.IsNullOrWhiteSpace(ApartmentContains)
            ? new List<string>()
            : ApartmentContains.Split(new[] { ',', ';', '|', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();

        [NotMapped]
        public List<string> NeighborhoodSchoolsList => SplitLines(NeighborhoodSchools);

        [NotMapped]
        public List<string> NeighborhoodHospitalsList => SplitLines(NeighborhoodHospitals);

        [NotMapped]
        public List<string> NeighborhoodTransitList => SplitLines(NeighborhoodTransit);

        private static List<string> SplitLines(string? value) => string.IsNullOrWhiteSpace(value)
            ? new List<string>()
            : value.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).Where(s => s.Length > 0).ToList();
    }
}
