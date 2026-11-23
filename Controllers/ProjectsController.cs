using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstatePortfolio.Data;
using RealEstatePortfolio.Models.Entities;
using RealEstatePortfolio.Models.ViewModels;

namespace RealEstatePortfolio.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProjectsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /Projects, /Projects/Index
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] ProjectsFilterViewModel filter)
        {
            var query = _db.Buildings
                .Include(b => b.Images)
                .AsQueryable();

            // 1. Status Filter
            if (filter.Status.HasValue)
            {
                query = query.Where(b => b.Status == filter.Status.Value);
            }

            // 2. Completion Year Filter
            if (filter.CompletionYear.HasValue)
            {
                query = query.Where(b => b.CompletionYear == filter.CompletionYear.Value);
            }

            // 3. Structural Feature Filter
            if (!string.IsNullOrWhiteSpace(filter.StructuralFeature))
            {
                var featureTerm = filter.StructuralFeature.Trim();
                query = query.Where(b => b.StructuralFeatures != null && b.StructuralFeatures.Contains(featureTerm));
            }

            // 4. Property Type Filter
            if (!string.IsNullOrWhiteSpace(filter.PropertyType))
            {
                query = query.Where(b => b.PropertyType != null && b.PropertyType.Contains(filter.PropertyType));
            }

            // 5. Sister Concern Filter
            if (!string.IsNullOrWhiteSpace(filter.SisterConcern))
            {
                query = query.Where(b => b.SisterConcern == filter.SisterConcern.Trim());
            }

            // 6. Search Term Filter
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var term = filter.SearchTerm.Trim().ToLower();
                query = query.Where(b => b.Name.ToLower().Contains(term) 
                                      || (b.Address != null && b.Address.ToLower().Contains(term)) 
                                      || (b.StructuralFeatures != null && b.StructuralFeatures.ToLower().Contains(term)));
            }

            // 7. Sorting
            query = filter.SortBy switch
            {
                "name_asc" => query.OrderBy(b => b.Name),
                "price_asc" => query.OrderBy(b => b.PriceStartingFrom ?? 0),
                "price_desc" => query.OrderByDescending(b => b.PriceStartingFrom ?? 0),
                "year_desc" => query.OrderByDescending(b => b.CompletionYear ?? 0),
                _ => query.OrderByDescending(b => b.CreatedAt)
            };

            var projects = await query.ToListAsync();

            // Populate filter metadata lists
            var allBuildings = await _db.Buildings.AsNoTracking().ToListAsync();
            var availableYears = allBuildings
                .Where(b => b.CompletionYear.HasValue)
                .Select(b => b.CompletionYear!.Value)
                .Distinct()
                .OrderBy(y => y)
                .ToList();

            var availableFeatures = allBuildings
                .Where(b => !string.IsNullOrWhiteSpace(b.StructuralFeatures))
                .SelectMany(b => b.StructuralFeatures!.Split(new[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(f => f.Trim())
                .Distinct()
                .OrderBy(f => f)
                .Take(15)
                .ToList();

            var availableTypes = allBuildings
                .Where(b => !string.IsNullOrWhiteSpace(b.PropertyType))
                .Select(b => b.PropertyType!.Trim())
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            var availableConcerns = allBuildings
                .Where(b => !string.IsNullOrWhiteSpace(b.SisterConcern))
                .Select(b => b.SisterConcern!.Trim())
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            if (!availableConcerns.Any())
            {
                availableConcerns = new List<string> { "Recent Properties Ltd", "Recent Living Ltd", "Recent Living Interior" };
            }

            filter.Projects = projects;
            filter.TotalResultsCount = projects.Count;
            filter.AvailableYears = availableYears;
            filter.AvailableFeatures = availableFeatures;
            filter.AvailablePropertyTypes = availableTypes;
            filter.AvailableSisterConcerns = availableConcerns;

            ViewData["SeoTitle"] = "Recent Group Portfolio | Developments by Recent Properties, Living & Interiors";
            ViewData["SeoDescription"] = "Browse landmark projects across Recent Properties Ltd, Recent Living Ltd, and Recent Living Interior.";

            return View(filter);
        }

        // AJAX API Endpoint for Instant Client-Side Dynamic Filtering
        [HttpGet]
        [Route("Projects/FilterApi")]
        public async Task<IActionResult> FilterApi(
            string? searchTerm, 
            ProjectStatus? status, 
            int? completionYear, 
            string? structuralFeature, 
            string? propertyType, 
            string? sisterConcern,
            string? sortBy)
        {
            var query = _db.Buildings
                .Include(b => b.Images)
                .AsQueryable();

            if (status.HasValue)
                query = query.Where(b => b.Status == status.Value);

            if (completionYear.HasValue)
                query = query.Where(b => b.CompletionYear == completionYear.Value);

            if (!string.IsNullOrWhiteSpace(structuralFeature))
                query = query.Where(b => b.StructuralFeatures != null && b.StructuralFeatures.Contains(structuralFeature.Trim()));

            if (!string.IsNullOrWhiteSpace(propertyType))
                query = query.Where(b => b.PropertyType != null && b.PropertyType.Contains(propertyType.Trim()));

            if (!string.IsNullOrWhiteSpace(sisterConcern))
                query = query.Where(b => b.SisterConcern == sisterConcern.Trim());

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim().ToLower();
                query = query.Where(b => b.Name.ToLower().Contains(term) 
                                      || (b.Address != null && b.Address.ToLower().Contains(term)) 
                                      || (b.StructuralFeatures != null && b.StructuralFeatures.ToLower().Contains(term)));
            }

            query = sortBy switch
            {
                "name_asc" => query.OrderBy(b => b.Name),
                "price_asc" => query.OrderBy(b => b.PriceStartingFrom ?? 0),
                "price_desc" => query.OrderByDescending(b => b.PriceStartingFrom ?? 0),
                "year_desc" => query.OrderByDescending(b => b.CompletionYear ?? 0),
                _ => query.OrderByDescending(b => b.CreatedAt)
            };

            var list = await query.Select(b => new
            {
                id = b.Id,
                name = b.Name,
                slug = b.Slug,
                address = b.Address,
                status = (int)b.Status,
                statusText = b.Status.ToString(),
                completionYear = b.CompletionYear,
                propertyType = b.PropertyType,
                sisterConcern = b.SisterConcern ?? "Recent Properties Ltd",
                priceStartingFrom = b.PriceStartingFrom,
                latitude = b.Latitude,
                longitude = b.Longitude,
                structuralFeatures = b.StructuralFeatures,
                primaryImage = b.Images.OrderBy(i => i.DisplayOrder).Where(i => i.IsPrimary).Select(i => i.ImagePath).FirstOrDefault() 
                               ?? b.Images.OrderBy(i => i.DisplayOrder).Select(i => i.ImagePath).FirstOrDefault() 
                               ?? "/images/properties/lumina-1.svg"
            }).ToListAsync();

            return Json(new { count = list.Count, projects = list });
        }

        // SEO Routing: /projects/{slug}
        [HttpGet]
        [Route("projects/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return NotFound();
            }

            var building = await _db.Buildings
                .Include(b => b.Images.OrderBy(i => i.DisplayOrder))
                .Include(b => b.Reviews.OrderByDescending(r => r.CreatedAt))
                .FirstOrDefaultAsync(b => b.Slug.ToLower() == slug.ToLower());

            if (building == null)
            {
                return NotFound();
            }

            // Query up to 3 similar projects matching status or property type (excluding current building)
            var similarProjects = await _db.Buildings
                .Include(b => b.Images)
                .Where(b => b.Id != building.Id && (b.Status == building.Status || b.PropertyType == building.PropertyType))
                .OrderByDescending(b => b.CreatedAt)
                .Take(3)
                .ToListAsync();

            // Fallback if less than 3
            if (similarProjects.Count < 3)
            {
                var additional = await _db.Buildings
                    .Include(b => b.Images)
                    .Where(b => b.Id != building.Id && !similarProjects.Select(s => s.Id).Contains(b.Id))
                    .Take(3 - similarProjects.Count)
                    .ToListAsync();
                similarProjects.AddRange(additional);
            }

            // Inject Dynamic Meta Tags into Layout
            ViewData["SeoTitle"] = !string.IsNullOrWhiteSpace(building.SeoTitle) 
                ? building.SeoTitle 
                : $"{building.Name} | Luxury Real Estate & Architectural Specifications";

            ViewData["SeoDescription"] = !string.IsNullOrWhiteSpace(building.SeoDescription) 
                ? building.SeoDescription 
                : $"Explore {building.Name} located at {building.Address}. High-performance structural engineering, premium finishes, and exclusive floor plans.";

            ViewData["OgImage"] = building.PrimaryImage?.ImagePath ?? "/images/properties/lumina-1.svg";

            var viewModel = new ProjectDetailViewModel
            {
                Building = building,
                SimilarProjects = similarProjects,
                InquiryForm = new ContactFormViewModel
                {
                    PreferredProjectSlug = building.Slug,
                    Subject = $"Inquiry Regarding {building.Name}"
                }
            };

            return View(viewModel);
        }

        // Brochure Download
        [HttpGet]
        public async Task<IActionResult> DownloadBrochure(int id)
        {
            var building = await _db.Buildings.FindAsync(id);
            if (building == null || string.IsNullOrWhiteSpace(building.BrochurePdfPath))
            {
                TempData["ErrorMessage"] = "Brochure PDF is currently being updated for this property.";
                return RedirectToAction(nameof(Index));
            }

            var cleanPath = building.BrochurePdfPath.TrimStart('/');
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", cleanPath.Replace('/', Path.DirectorySeparatorChar));

            if (!System.IO.File.Exists(fullPath))
            {
                var defaultBrochure = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "brochures", "the-lumina-horizon-tower-brochure.pdf");
                if (System.IO.File.Exists(defaultBrochure))
                {
                    var fileBytes = await System.IO.File.ReadAllBytesAsync(defaultBrochure);
                    return File(fileBytes, "application/pdf", $"{building.Slug}-brochure.pdf");
                }
                TempData["ErrorMessage"] = "Brochure file is unavailable.";
                return RedirectToAction("Details", new { slug = building.Slug });
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(fullPath);
            return File(bytes, "application/pdf", $"{building.Slug}-brochure.pdf");
        }
    }
}
