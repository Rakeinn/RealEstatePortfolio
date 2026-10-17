using RealEstatePortfolio.Models.Entities;

namespace RealEstatePortfolio.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Building> FeaturedProjects { get; set; } = new List<Building>();
        public List<Building> RecentProjects { get; set; } = new List<Building>();
        public Dictionary<string, string> Content { get; set; } = new Dictionary<string, string>();
        public int TotalProjectsCount { get; set; }
        public int CompletedProjectsCount { get; set; }
        public int OngoingProjectsCount { get; set; }
        public int UpcomingProjectsCount { get; set; }
        public List<Review> Testimonials { get; set; } = new List<Review>();
        public ContactFormViewModel ContactForm { get; set; } = new ContactFormViewModel();
    }
}
