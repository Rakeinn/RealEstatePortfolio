using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstatePortfolio.Data;
using RealEstatePortfolio.Models.Entities;
using RealEstatePortfolio.Models.ViewModels;
using RealEstatePortfolio.Services;

namespace RealEstatePortfolio.Controllers
{
    [Authorize(Roles = "Admin, Editor")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IContentService _contentService;
        private readonly IFileService _fileService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(
            ApplicationDbContext db,
            IContentService contentService,
            IFileService fileService,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _contentService = contentService;
            _fileService = fileService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: /Admin or /Admin/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var totalProjects = await _db.Buildings.CountAsync();
            var upcoming = await _db.Buildings.CountAsync(b => b.Status == ProjectStatus.Upcoming);
            var ongoing = await _db.Buildings.CountAsync(b => b.Status == ProjectStatus.Ongoing);
            var completed = await _db.Buildings.CountAsync(b => b.Status == ProjectStatus.Completed);
            var totalLeads = await _db.ContactLeads.CountAsync();
            var unreadLeads = await _db.ContactLeads.CountAsync(l => !l.IsRead);

            // Compute monthly lead volume for past 6 months
            var now = DateTime.UtcNow;
            var months = new List<string>();
            var leadCounts = new List<int>();

            for (int i = 5; i >= 0; i--)
            {
                var monthDate = now.AddMonths(-i);
                var monthName = monthDate.ToString("MMM yyyy");
                months.Add(monthName);

                var startOfMonth = new DateTime(monthDate.Year, monthDate.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1);

                var count = await _db.ContactLeads
                    .CountAsync(l => l.SubmittedAt >= startOfMonth && l.SubmittedAt <= endOfMonth);
                leadCounts.Add(count);
            }

            var recentLeads = await _db.ContactLeads
                .OrderByDescending(l => l.SubmittedAt)
                .Take(5)
                .ToListAsync();

            var recentProjects = await _db.Buildings
                .Include(b => b.Images)
                .OrderByDescending(b => b.CreatedAt)
                .Take(5)
                .ToListAsync();

            var model = new AdminDashboardViewModel
            {
                TotalProjects = totalProjects,
                UpcomingProjects = upcoming,
                OngoingProjects = ongoing,
                CompletedProjects = completed,
                TotalLeads = totalLeads,
                UnreadLeads = unreadLeads,
                LeadMonths = months,
                LeadCounts = leadCounts,
                RecentLeads = recentLeads,
                RecentProjects = recentProjects
            };

            return View(model);
        }

        // ==========================================
        // PROJECT MANAGEMENT
        // ==========================================
        public async Task<IActionResult> Projects()
        {
            var projects = await _db.Buildings
                .Include(b => b.Images)
                .Include(b => b.Reviews)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(projects);
        }

        [HttpGet]
        public IActionResult CreateProject()
        {
            var model = new AdminProjectEditViewModel
            {
                Status = ProjectStatus.Ongoing,
                Latitude = 23.8103,
                Longitude = 90.4125,
                CompletionYear = DateTime.UtcNow.Year + 1,
                PropertyType = "Residential",
                SisterConcern = "Recent Living Ltd"
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProject(AdminProjectEditViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Slug))
            {
                model.Slug = GenerateSlug(model.Name);
            }
            else
            {
                model.Slug = GenerateSlug(model.Slug);
            }

            if (await _db.Buildings.AnyAsync(b => b.Slug.ToLower() == model.Slug.ToLower()))
            {
                ModelState.AddModelError("Slug", "A project with this URL slug already exists. Please choose a unique slug.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string? brochurePath = null;
            if (model.BrochureFile != null)
            {
                brochurePath = await _fileService.SavePdfBrochureAsync(model.BrochureFile);
            }

            var building = new Building
            {
                Name = model.Name.Trim(),
                Slug = model.Slug.Trim(),
                Address = model.Address?.Trim(),
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                Status = model.Status,
                CompletionYear = model.CompletionYear,
                SisterConcern = model.SisterConcern?.Trim() ?? "Recent Properties Ltd",
                PropertyType = model.PropertyType?.Trim(),
                PriceStartingFrom = model.PriceStartingFrom,
                TotalUnits = model.TotalUnits,
                HandoverDate = model.HandoverDate?.Trim(),
                Facing = model.Facing?.Trim(),
                Storied = model.Storied?.Trim(),
                ApartmentSize = model.ApartmentSize?.Trim(),
                LandSize = model.LandSize?.Trim(),
                ApartmentContains = model.ApartmentContains?.Trim(),
                NeighborhoodSchools = model.NeighborhoodSchools?.Trim(),
                NeighborhoodHospitals = model.NeighborhoodHospitals?.Trim(),
                NeighborhoodTransit = model.NeighborhoodTransit?.Trim(),
                StructuralFeatures = model.StructuralFeatures?.Trim(),
                LongDescription = model.LongDescription,
                BrochurePdfPath = brochurePath,
                SeoTitle = model.SeoTitle?.Trim(),
                SeoDescription = model.SeoDescription?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _db.Buildings.Add(building);
            await _db.SaveChangesAsync();

            // Handle New Images
            if (model.NewImages != null && model.NewImages.Any())
            {
                int order = 1;
                foreach (var imgFile in model.NewImages)
                {
                    var imgPath = await _fileService.SaveImageAsync(imgFile);
                    if (!string.IsNullOrEmpty(imgPath))
                    {
                        _db.BuildingImages.Add(new BuildingImage
                        {
                            BuildingId = building.Id,
                            ImagePath = imgPath,
                            IsPrimary = (order == 1),
                            DisplayOrder = order,
                            Caption = $"{building.Name} View {order}"
                        });
                        order++;
                    }
                }
                await _db.SaveChangesAsync();
            }
            else
            {
                // Fallback default placeholder
                _db.BuildingImages.Add(new BuildingImage
                {
                    BuildingId = building.Id,
                    ImagePath = "/images/properties/lumina-1.svg",
                    IsPrimary = true,
                    DisplayOrder = 1,
                    Caption = $"{building.Name} Exterior"
                });
                await _db.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = $"Project '{building.Name}' successfully published!";
            return RedirectToAction(nameof(Projects));
        }

        [HttpGet]
        public async Task<IActionResult> EditProject(int id)
        {
            var building = await _db.Buildings
                .Include(b => b.Images.OrderBy(i => i.DisplayOrder))
                .FirstOrDefaultAsync(b => b.Id == id);

            if (building == null) return NotFound();

            var model = new AdminProjectEditViewModel
            {
                Id = building.Id,
                Name = building.Name,
                Slug = building.Slug,
                Address = building.Address,
                Latitude = building.Latitude,
                Longitude = building.Longitude,
                Status = building.Status,
                CompletionYear = building.CompletionYear,
                SisterConcern = building.SisterConcern ?? "Recent Properties Ltd",
                PropertyType = building.PropertyType,
                PriceStartingFrom = building.PriceStartingFrom,
                TotalUnits = building.TotalUnits,
                HandoverDate = building.HandoverDate,
                Facing = building.Facing,
                Storied = building.Storied,
                ApartmentSize = building.ApartmentSize,
                LandSize = building.LandSize,
                ApartmentContains = building.ApartmentContains,
                NeighborhoodSchools = building.NeighborhoodSchools,
                NeighborhoodHospitals = building.NeighborhoodHospitals,
                NeighborhoodTransit = building.NeighborhoodTransit,
                StructuralFeatures = building.StructuralFeatures,
                LongDescription = building.LongDescription,
                BrochurePdfPath = building.BrochurePdfPath,
                SeoTitle = building.SeoTitle,
                SeoDescription = building.SeoDescription,
                ExistingImages = building.Images.ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProject(AdminProjectEditViewModel model)
        {
            model.Slug = GenerateSlug(model.Slug);

            if (await _db.Buildings.AnyAsync(b => b.Slug.ToLower() == model.Slug.ToLower() && b.Id != model.Id))
            {
                ModelState.AddModelError("Slug", "Another project is already using this URL slug.");
            }

            if (!ModelState.IsValid)
            {
                model.ExistingImages = await _db.BuildingImages.Where(i => i.BuildingId == model.Id).OrderBy(i => i.DisplayOrder).ToListAsync();
                return View(model);
            }

            var building = await _db.Buildings
                .Include(b => b.Images)
                .FirstOrDefaultAsync(b => b.Id == model.Id);

            if (building == null) return NotFound();

            if (model.BrochureFile != null)
            {
                var newBrochure = await _fileService.SavePdfBrochureAsync(model.BrochureFile);
                if (!string.IsNullOrEmpty(newBrochure))
                {
                    building.BrochurePdfPath = newBrochure;
                }
            }

            building.Name = model.Name.Trim();
            building.Slug = model.Slug.Trim();
            building.Address = model.Address?.Trim();
            building.Latitude = model.Latitude;
            building.Longitude = model.Longitude;
            building.Status = model.Status;
            building.CompletionYear = model.CompletionYear;
            building.SisterConcern = model.SisterConcern?.Trim() ?? "Recent Properties Ltd";
            building.PropertyType = model.PropertyType?.Trim();
            building.PriceStartingFrom = model.PriceStartingFrom;
            building.TotalUnits = model.TotalUnits;
            building.HandoverDate = model.HandoverDate?.Trim();
            building.Facing = model.Facing?.Trim();
            building.Storied = model.Storied?.Trim();
            building.ApartmentSize = model.ApartmentSize?.Trim();
            building.LandSize = model.LandSize?.Trim();
            building.ApartmentContains = model.ApartmentContains?.Trim();
            building.NeighborhoodSchools = model.NeighborhoodSchools?.Trim();
            building.NeighborhoodHospitals = model.NeighborhoodHospitals?.Trim();
            building.NeighborhoodTransit = model.NeighborhoodTransit?.Trim();
            building.StructuralFeatures = model.StructuralFeatures?.Trim();
            building.LongDescription = model.LongDescription;
            building.SeoTitle = model.SeoTitle?.Trim();
            building.SeoDescription = model.SeoDescription?.Trim();

            // Handle new images upload
            if (model.NewImages != null && model.NewImages.Any())
            {
                int maxOrder = building.Images.Any() ? building.Images.Max(i => i.DisplayOrder) : 0;
                foreach (var imgFile in model.NewImages)
                {
                    var imgPath = await _fileService.SaveImageAsync(imgFile);
                    if (!string.IsNullOrEmpty(imgPath))
                    {
                        maxOrder++;
                        _db.BuildingImages.Add(new BuildingImage
                        {
                            BuildingId = building.Id,
                            ImagePath = imgPath,
                            IsPrimary = !building.Images.Any(),
                            DisplayOrder = maxOrder,
                            Caption = $"{building.Name} View {maxOrder}"
                        });
                    }
                }
            }

            await _db.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Project '{building.Name}' successfully updated!";
            return RedirectToAction(nameof(Projects));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var building = await _db.Buildings
                .Include(b => b.Images)
                .Include(b => b.Reviews)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (building != null)
            {
                _db.Buildings.Remove(building);
                await _db.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Project '{building.Name}' has been removed.";
            }

            return RedirectToAction(nameof(Projects));
        }

        // ==========================================
        // GALLERY & SORTABLE JS IMAGE MANAGEMENT
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> ManageImages(int id)
        {
            var building = await _db.Buildings
                .Include(b => b.Images.OrderBy(i => i.DisplayOrder))
                .FirstOrDefaultAsync(b => b.Id == id);

            if (building == null) return NotFound();

            return View(building);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadGalleryImages(int buildingId, List<IFormFile> images)
        {
            var building = await _db.Buildings.Include(b => b.Images).FirstOrDefaultAsync(b => b.Id == buildingId);
            if (building == null) return NotFound();

            if (images != null && images.Any())
            {
                int currentMax = building.Images.Any() ? building.Images.Max(i => i.DisplayOrder) : 0;
                foreach (var imgFile in images)
                {
                    var path = await _fileService.SaveImageAsync(imgFile);
                    if (!string.IsNullOrEmpty(path))
                    {
                        currentMax++;
                        _db.BuildingImages.Add(new BuildingImage
                        {
                            BuildingId = buildingId,
                            ImagePath = path,
                            IsPrimary = !building.Images.Any(),
                            DisplayOrder = currentMax,
                            Caption = $"{building.Name} Gallery Photo {currentMax}"
                        });
                    }
                }
                await _db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Images uploaded successfully!";
            }

            return RedirectToAction(nameof(ManageImages), new { id = buildingId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPrimaryImage(int imageId, int buildingId)
        {
            var images = await _db.BuildingImages.Where(i => i.BuildingId == buildingId).ToListAsync();
            foreach (var img in images)
            {
                img.IsPrimary = (img.Id == imageId);
            }
            await _db.SaveChangesAsync();
            TempData["SuccessMessage"] = "Primary cover image updated!";
            return RedirectToAction(nameof(ManageImages), new { id = buildingId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage(int imageId, int buildingId)
        {
            var img = await _db.BuildingImages.FirstOrDefaultAsync(i => i.Id == imageId && i.BuildingId == buildingId);
            if (img != null)
            {
                _db.BuildingImages.Remove(img);
                await _db.SaveChangesAsync();
                _fileService.DeleteFile(img.ImagePath);
                TempData["SuccessMessage"] = "Image deleted.";
            }
            return RedirectToAction(nameof(ManageImages), new { id = buildingId });
        }

        // SortableJS Drag & Drop AJAX Endpoint
        [HttpPost]
        public async Task<IActionResult> UpdateImageOrder([FromBody] List<ImageOrderDto> orderItems)
        {
            if (orderItems == null || !orderItems.Any())
            {
                return BadRequest(new { success = false, message = "No order data supplied." });
            }

            var ids = orderItems.Select(o => o.Id).ToList();
            var images = await _db.BuildingImages.Where(i => ids.Contains(i.Id)).ToListAsync();

            foreach (var item in orderItems)
            {
                var target = images.FirstOrDefault(i => i.Id == item.Id);
                if (target != null)
                {
                    target.DisplayOrder = item.DisplayOrder;
                }
            }

            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Gallery order persisted successfully." });
        }

        // ==========================================
        // DYNAMIC CONTENT ENGINE (CMS)
        // ==========================================
        public new async Task<IActionResult> Content(string page = "Home")
        {
            ViewBag.SelectedPage = page;
            var contents = await _db.PageContents
                .Where(c => c.PageName.ToLower() == page.ToLower())
                .OrderBy(c => c.SectionKey)
                .ToListAsync();

            return View(contents);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateContent(int id, string pageName, string sectionKey, string contentValue)
        {
            await _contentService.UpdateContentAsync(pageName, sectionKey, contentValue);
            TempData["SuccessMessage"] = $"Content for '{sectionKey}' saved successfully!";
            return RedirectToAction(nameof(Content), new { page = pageName });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateContent(string pageName, string sectionKey, string contentValue)
        {
            if (!string.IsNullOrWhiteSpace(pageName) && !string.IsNullOrWhiteSpace(sectionKey))
            {
                await _contentService.UpdateContentAsync(pageName.Trim(), sectionKey.Trim(), contentValue ?? "");
                TempData["SuccessMessage"] = $"New section '{sectionKey}' created!";
            }
            return RedirectToAction(nameof(Content), new { page = pageName });
        }

        // ==========================================
        // LEAD INBOX
        // ==========================================
        public async Task<IActionResult> Leads(string? filter = "all", string? search = null)
        {
            var query = _db.ContactLeads.AsQueryable();

            if (filter == "unread")
                query = query.Where(l => !l.IsRead);
            else if (filter == "read")
                query = query.Where(l => l.IsRead);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(l => l.ClientName.ToLower().Contains(term) 
                                      || l.EmailAddress.ToLower().Contains(term) 
                                      || l.Subject.ToLower().Contains(term));
            }

            var leads = await query.OrderByDescending(l => l.SubmittedAt).ToListAsync();

            ViewBag.CurrentFilter = filter;
            ViewBag.Search = search;
            ViewBag.TotalUnread = await _db.ContactLeads.CountAsync(l => !l.IsRead);

            return View(leads);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleLeadReadStatus(int id)
        {
            var lead = await _db.ContactLeads.FindAsync(id);
            if (lead == null) return NotFound();

            lead.IsRead = !lead.IsRead;
            await _db.SaveChangesAsync();

            return Json(new { success = true, isRead = lead.IsRead });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLead(int id)
        {
            var lead = await _db.ContactLeads.FindAsync(id);
            if (lead != null)
            {
                _db.ContactLeads.Remove(lead);
                await _db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Lead entry deleted.";
            }
            return RedirectToAction(nameof(Leads));
        }

        // ==========================================
        // USER & STAFF MANAGEMENT (Admin Only)
        // ==========================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<AdminUserViewModel>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                userList.Add(new AdminUserViewModel
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email ?? "",
                    UserName = u.UserName ?? "",
                    Roles = roles.ToList(),
                    CreatedAt = u.CreatedAt
                });
            }

            return View(userList);
        }

        private static string GenerateSlug(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return Guid.NewGuid().ToString("N")[..8];
            var str = text.ToLowerInvariant().Trim();
            var sb = new System.Text.StringBuilder();
            bool prevDash = false;

            foreach (char c in str)
            {
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevDash = false;
                }
                else if (c == ' ' || c == '-' || c == '_')
                {
                    if (!prevDash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevDash = true;
                    }
                }
            }

            var result = sb.ToString().Trim('-');
            return string.IsNullOrEmpty(result) ? "project-" + Guid.NewGuid().ToString("N")[..6] : result;
        }
    }

    public class ImageOrderDto
    {
        public int Id { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class AdminUserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; }
    }
}
