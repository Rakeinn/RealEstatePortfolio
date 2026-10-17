using RealEstatePortfolio.Models.Entities;

namespace RealEstatePortfolio.Models.ViewModels
{
    public class ProjectsFilterViewModel
    {
        public string? SearchTerm { get; set; }
        public ProjectStatus? Status { get; set; }
        public int? CompletionYear { get; set; }
        public string? StructuralFeature { get; set; }
        public string? PropertyType { get; set; }
        public string? SisterConcern { get; set; }
        public string? SortBy { get; set; } = "newest"; // newest, name_asc, price_asc, price_desc, year_desc

        public List<Building> Projects { get; set; } = new List<Building>();
        public List<int> AvailableYears { get; set; } = new List<int>();
        public List<string> AvailableFeatures { get; set; } = new List<string>();
        public List<string> AvailablePropertyTypes { get; set; } = new List<string>();
        public List<string> AvailableSisterConcerns { get; set; } = new List<string>();

        public int TotalResultsCount { get; set; }
    }
}
