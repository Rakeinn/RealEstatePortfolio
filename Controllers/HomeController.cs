using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstatePortfolio.Data;
using RealEstatePortfolio.Models.Entities;
using RealEstatePortfolio.Models.ViewModels;
using RealEstatePortfolio.Services;

namespace RealEstatePortfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IContentService _contentService;
        private readonly IEmailNotificationService _emailService;

        public HomeController(ApplicationDbContext db, IContentService contentService, IEmailNotificationService emailService)
        {
            _db = db;
            _contentService = contentService;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            var featured = await _db.Buildings
                .Include(b => b.Images)
                .OrderByDescending(b => b.CreatedAt)
                .Take(6)
                .ToListAsync();

            var cmsContent = await _contentService.GetPageContentsAsync("Home");
            var reviews = await _db.Reviews.OrderByDescending(r => r.CreatedAt).Take(4).ToListAsync();

            var totalCount = await _db.Buildings.CountAsync();
            var completedCount = await _db.Buildings.CountAsync(b => b.Status == ProjectStatus.Completed);
            var ongoingCount = await _db.Buildings.CountAsync(b => b.Status == ProjectStatus.Ongoing);
            var upcomingCount = await _db.Buildings.CountAsync(b => b.Status == ProjectStatus.Upcoming);

            var viewModel = new HomeViewModel
            {
                FeaturedProjects = featured,
                Content = cmsContent,
                TotalProjectsCount = totalCount,
                CompletedProjectsCount = completedCount,
                OngoingProjectsCount = ongoingCount,
                UpcomingProjectsCount = upcomingCount,
                Testimonials = reviews
            };

            ViewData["SeoTitle"] = "Premier Architectural & Real Estate Portfolio | Master Developments";
            ViewData["SeoDescription"] = "Explore world-class residential and commercial architectural landmarks engineered for sustainability, seismic resilience, and supreme luxury.";

            return View(viewModel);
        }

        public async Task<IActionResult> About()
        {
            var cmsAbout = await _contentService.GetPageContentsAsync("About");
            var cmsHome = await _contentService.GetPageContentsAsync("Home");

            ViewBag.AboutContent = cmsAbout;
            ViewBag.HomeContent = cmsHome;

            ViewData["SeoTitle"] = "Our Architectural Legacy & Philosophy | Master Real Estate";
            ViewData["SeoDescription"] = "Discover our 24-year heritage in engineering carbon-neutral, resilient, and aesthetically breathtaking architectural developments worldwide.";

            return View();
        }

        [HttpGet]
        [Route("recent-living")]
        public async Task<IActionResult> Living()
        {
            var livingProjects = await _db.Buildings
                .Include(b => b.Images)
                .Where(b => b.SisterConcern == "Recent Living Ltd")
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            var cmsHome = await _contentService.GetPageContentsAsync("Home");
            ViewBag.HomeContent = cmsHome;

            ViewData["SeoTitle"] = "Recent Living Ltd. | Better Lifestyle… - Trusted Real Estate in Bangladesh";
            ViewData["SeoDescription"] = "Recent Living Ltd. (A sister concern of Recent Group) - Creating thoughtfully designed homes and commercial spaces that enrich people's lives and contribute to better communities in Bangladesh.";

            return View(livingProjects);
        }

        public async Task<IActionResult> Contact(string? project = null)
        {
            var model = new ContactFormViewModel();
            if (!string.IsNullOrEmpty(project))
            {
                model.PreferredProjectSlug = project;
                model.Subject = $"Inquiry Regarding {project.Replace("-", " ")}";
            }

            var contactCms = await _contentService.GetPageContentsAsync("Contact");
            ViewBag.ContactContent = contactCms;

            ViewData["SeoTitle"] = "Contact Concierge & VIP Inquiries | Real Estate Portfolio";
            ViewData["SeoDescription"] = "Connect with our international real estate advisory desk for private consultations, property viewings, and commercial leasing inquiries.";

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var contactCms = await _contentService.GetPageContentsAsync("Contact");
                ViewBag.ContactContent = contactCms;
                return View(model);
            }

            var lead = new ContactLead
            {
                ClientName = model.ClientName.Trim(),
                EmailAddress = model.EmailAddress.Trim(),
                PhoneNumber = model.PhoneNumber?.Trim(),
                Subject = model.Subject.Trim(),
                Message = model.Message.Trim(),
                PreferredProjectSlug = model.PreferredProjectSlug,
                TargetedConcern = model.TargetedConcern ?? "Recent Group Corporate",
                SubmittedAt = DateTime.UtcNow,
                IsRead = false
            };

            _db.ContactLeads.Add(lead);
            await _db.SaveChangesAsync();

            // Asynchronously trigger SMTP email alert to admin without blocking UI thread
            await _emailService.SendLeadNotificationAsync(lead);

            TempData["SuccessMessage"] = "Thank you for reaching out! Your inquiry has been routed to our advisory team. A representative will contact you shortly.";
            return RedirectToAction(nameof(Contact));
        }

        [HttpPost]
        public async Task<IActionResult> SubmitLeadAjax([FromBody] ContactFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Please fill in all required fields accurately." });
            }

            var lead = new ContactLead
            {
                ClientName = model.ClientName.Trim(),
                EmailAddress = model.EmailAddress.Trim(),
                PhoneNumber = model.PhoneNumber?.Trim(),
                Subject = model.Subject.Trim(),
                Message = model.Message.Trim(),
                PreferredProjectSlug = model.PreferredProjectSlug,
                TargetedConcern = model.TargetedConcern ?? "Recent Group Corporate",
                SubmittedAt = DateTime.UtcNow,
                IsRead = false
            };

            _db.ContactLeads.Add(lead);
            await _db.SaveChangesAsync();

            // Asynchronously trigger alert
            await _emailService.SendLeadNotificationAsync(lead);

            return Json(new { success = true, message = "Inquiry successfully submitted! Our concierge desk will contact you within 24 hours." });
        }
    }
}
