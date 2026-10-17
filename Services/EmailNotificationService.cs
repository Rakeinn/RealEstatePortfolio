using System.Net;
using System.Net.Mail;
using RealEstatePortfolio.Models.Entities;

namespace RealEstatePortfolio.Services
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailNotificationService> _logger;

        public EmailNotificationService(IConfiguration configuration, ILogger<EmailNotificationService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public Task SendLeadNotificationAsync(ContactLead lead)
        {
            // Trigger asynchronous background task without blocking UI request thread
            _ = Task.Run(async () =>
            {
                try
                {
                    var smtpHost = _configuration["Smtp:Host"] ?? "smtp.example.com";
                    var smtpPort = int.TryParse(_configuration["Smtp:Port"], out var port) ? port : 587;
                    var smtpUser = _configuration["Smtp:Username"];
                    var smtpPass = _configuration["Smtp:Password"];
                    var fromAddress = _configuration["Smtp:FromAddress"] ?? "alerts@realestateportfolio.com";
                    var adminEmail = _configuration["Smtp:AdminEmail"] ?? "admin@realestate.com";

                    _logger.LogInformation("Async Email Alert triggered for new Lead from {ClientName} ({EmailAddress}) regarding {Subject}",
                        lead.ClientName, lead.EmailAddress, lead.Subject);

                    // If SMTP credentials configured, attempt sending via SmtpClient
                    if (!string.IsNullOrEmpty(smtpUser) && !string.IsNullOrEmpty(smtpPass))
                    {
                        using var client = new SmtpClient(smtpHost, smtpPort)
                        {
                            Credentials = new NetworkCredential(smtpUser, smtpPass),
                            EnableSsl = true
                        };

                        var mailMessage = new MailMessage
                        {
                            From = new MailAddress(fromAddress, "Real Estate Portfolio Alerts"),
                            Subject = $"[New Inquiry] {lead.Subject} from {lead.ClientName}",
                            Body = $@"
New Lead Submission Details:
------------------------------------------
Client Name:    {lead.ClientName}
Email Address:  {lead.EmailAddress}
Phone Number:   {lead.PhoneNumber ?? "Not provided"}
Subject:        {lead.Subject}
Project:        {lead.PreferredProjectSlug ?? "General"}
Date:           {lead.SubmittedAt:yyyy-MM-dd HH:mm:ss UTC}

Message:
{lead.Message}
------------------------------------------
View in Admin Dashboard: /Admin/Leads
",
                            IsBodyHtml = false
                        };
                        mailMessage.To.Add(adminEmail);

                        await client.SendMailAsync(mailMessage);
                        _logger.LogInformation("SMTP Email alert successfully dispatched to {AdminEmail}", adminEmail);
                    }
                    else
                    {
                        // Simulated SMTP notification log for development environments
                        _logger.LogInformation("[SIMULATED SMTP NOTIFICATION DISPATCHED] To: {AdminEmail} | Subject: New Lead from {ClientName} | Phone: {Phone} | Message: {Msg}",
                            adminEmail, lead.ClientName, lead.PhoneNumber ?? "N/A", lead.Message);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Background SMTP email alert dispatch encountered an error. Lead was safely saved in database.");
                }
            });

            return Task.CompletedTask;
        }
    }
}
