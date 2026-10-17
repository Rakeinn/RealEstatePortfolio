using RealEstatePortfolio.Models.Entities;

namespace RealEstatePortfolio.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalProjects { get; set; }
        public int UpcomingProjects { get; set; }
        public int OngoingProjects { get; set; }
        public int CompletedProjects { get; set; }
        public int TotalLeads { get; set; }
        public int UnreadLeads { get; set; }

        // Chart Data
        public List<string> LeadMonths { get; set; } = new List<string>();
        public List<int> LeadCounts { get; set; } = new List<int>();

        public List<ContactLead> RecentLeads { get; set; } = new List<ContactLead>();
        public List<Building> RecentProjects { get; set; } = new List<Building>();
    }
}
