using RealEstatePortfolio.Models.Entities;

namespace RealEstatePortfolio.Services
{
    public interface IEmailNotificationService
    {
        Task SendLeadNotificationAsync(ContactLead lead);
    }
}
