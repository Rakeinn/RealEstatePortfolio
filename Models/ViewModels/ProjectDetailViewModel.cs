using RealEstatePortfolio.Models.Entities;

namespace RealEstatePortfolio.Models.ViewModels
{
    public class ProjectDetailViewModel
    {
        public Building Building { get; set; } = null!;
        public List<Building> SimilarProjects { get; set; } = new List<Building>();
        public ContactFormViewModel InquiryForm { get; set; } = new ContactFormViewModel();
    }
}
