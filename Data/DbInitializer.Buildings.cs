using RealEstatePortfolio.Models.Entities;

namespace RealEstatePortfolio.Data
{
    public static partial class DbInitializer
    {
        private static async Task SeedBuildingsAsync(ApplicationDbContext context)
        {
            if (context.Buildings.Any()) return;

            var b1 = new Building
            {
                Name = "Recent Living Crown Heights",
                Slug = "recent-living-crown-heights",
                Address = "Road 79, Gulshan 2, Dhaka, Bangladesh",
                Latitude = 23.7925,
                Longitude = 90.4078,
                Status = ProjectStatus.Completed,
                CompletionYear = 2024,
                SisterConcern = "Recent Living Ltd",
                PropertyType = "Residential",
                PriceStartingFrom = 45000000m,
                TotalUnits = 14,
                HandoverDate = "Handed Over in December 2024",
                Facing = "South Facing",
                Storied = "B2+G+ 12 Floors",
                ApartmentSize = "3200 Sq. Ft. | 4200 Sq. Ft.",
                LandSize = "12 Katha",
                ApartmentContains = "04 Beds, 04 Bath (Attached) & 1 Common, 04 Balconies, Formal Living, Family Living, Dining, Kitchen, Servant Room",
                NeighborhoodSchools = "Scholastica Senior Campus, Gulshan 2 ~0.6 km\nAmerican International School Dhaka (AISD), Baridhara ~1.4 km\nManarat Dhaka International School & College ~0.9 km",
                NeighborhoodHospitals = "United Hospital Limited, Gulshan 2 ~0.5 km\nEvercare Hospital Dhaka, Bashundhara ~3.2 km\nLabaid Diagnostic, Gulshan 2 ~0.7 km",
                NeighborhoodTransit = "Gulshan 2 Circle Bus Stop ~0.4 km\nBanani Railway Station ~2.1 km\nHazrat Shahjalal International Airport ~6.5 km",
                StructuralFeatures = "BNBC Code Earthquake-Resistant RCC Frame, European Double-Glazed Soundproof Glass, 100% Full Power Generator Backup, Rooftop Infinity Lap Pool, Imported Marble Finishes",
                LongDescription = @"<h3>Sophisticated Luxury Living in the Heart of Gulshan 2</h3><p>Developed by <strong>Recent Living Ltd.</strong> (<em>Better Lifestyle…</em>), <strong>Recent Living Crown Heights</strong> stands as an iconic residential landmark in Dhaka's premier diplomatic zone. Crafted to enrich people's lives with thoughtful architecture, natural ventilation, and uncompromising safety.</p><h4>Signature Features & Amenities:</h4><ul><li>Single-unit luxury floorplates (4,200 sq ft) offering total privacy</li><li>Rooftop infinity swimming pool overlooking the Gulshan lake</li><li>Dedicated standby generator with 100% load capacity</li><li>State-of-the-art multi-tier biometric security and CCTV surveillance</li><li>Turnkey interior styling by <strong>Recent Living Interior</strong></li></ul>",
                BrochurePdfPath = "/brochures/the-lumina-horizon-tower-brochure.pdf",
                SeoTitle = "Recent Living Crown Heights | Luxury Apartments Gulshan 2 Dhaka",
                SeoDescription = "Experience a better lifestyle at Recent Living Crown Heights in Gulshan 2, Dhaka. Developed by Recent Living Ltd.",
                CreatedAt = DateTime.UtcNow.AddMonths(-8)
            };

            var b2 = new Building
            {
                Name = "Recent Properties Waterfront Marina",
                Slug = "recent-properties-waterfront-marina",
                Address = "Diplomatic Zone, Baridhara, Dhaka, Bangladesh",
                Latitude = 23.7998,
                Longitude = 90.4225,
                Status = ProjectStatus.Ongoing,
                CompletionYear = 2026,
                SisterConcern = "Recent Properties Ltd",
                PropertyType = "Residential",
                PriceStartingFrom = 68000000m,
                TotalUnits = 18,
                HandoverDate = "Handover on June 2026",
                Facing = "South-East Facing (Lake View)",
                Storied = "B+G+ 10 Floors",
                ApartmentSize = "2850 Sq. Ft. | 3600 Sq. Ft.",
                LandSize = "10 Katha",
                ApartmentContains = "04 Beds, 03 Bath (Attached) & 1 Common, 03 Balconies, Living, Dining, Kitchen, Prayer Room",
                NeighborhoodSchools = "American International School Dhaka (AISD), Baridhara ~0.5 km\nBritish Primary School, Baridhara ~0.8 km\nCanadian International School Bangladesh ~1.1 km",
                NeighborhoodHospitals = "United Hospital Limited, Gulshan 2 ~1.2 km\nEvercare Hospital Dhaka, Bashundhara ~2.5 km\nPraava Health, Banani ~2.0 km",
                NeighborhoodTransit = "Notun Bazar Bus Stand ~0.7 km\nKuril Bishwa Road ~2.4 km\nHazrat Shahjalal International Airport ~5.8 km",
                StructuralFeatures = "Deep Substructure Piling, Lakefront Green Terraces, Sub-Zero VRF Air-Conditioning, High-Speed Mitsubishi Elevators, Smart Home Automation",
                LongDescription = @"<h3>Serene Waterfront Living in Baridhara</h3><p>Constructed by <strong>Recent Properties Ltd</strong>, the structural civil development wing of <strong>Recent Group</strong>, this premier lakeside tower provides tranquil water vistas and green terraces.</p><h4>Unrivaled Waterfront Lifestyle:</h4><ul><li>Panoramic unobstructed views of the Baridhara lake</li><li>Double-height landscaped entry reception and resident lounge</li><li>Multi-car mechanical parking system with EV fast chargers</li><li>Earthquake resilient RCC foundation adhering to BNBC standards</li></ul>",
                BrochurePdfPath = "/brochures/marina-azure-brochure.pdf",
                SeoTitle = "Recent Properties Waterfront Marina | Baridhara Dhaka",
                SeoDescription = "Prestige lakefront residences in Baridhara, Dhaka by Recent Properties Ltd (Recent Group).",
                CreatedAt = DateTime.UtcNow.AddMonths(-6)
            };

            var b3 = new Building
            {
                Name = "Recent Living Solaris Eco-Sanctuary",
                Slug = "recent-living-solaris-eco-sanctuary",
                Address = "Block I, Central Avenue, Bashundhara R/A, Dhaka, Bangladesh",
                Latitude = 23.8167,
                Longitude = 90.4350,
                Status = ProjectStatus.Upcoming,
                CompletionYear = 2027,
                SisterConcern = "Recent Living Ltd",
                PropertyType = "Residential",
                PriceStartingFrom = 28000000m,
                TotalUnits = 24,
                HandoverDate = "Handover on December 2027",
                Facing = "East Facing",
                Storied = "B+G+ 09 Floors",
                ApartmentSize = "1850 Sq. Ft. | 2200 Sq. Ft.",
                LandSize = "10 Katha",
                ApartmentContains = "03 Beds, 02 Bath (Attached) & 1 Common, 03 Balconies, Living, Dining, Kitchen",
                NeighborhoodSchools = "North South University (NSU), Bashundhara R/A ~0.9 km\nIndependent University Bangladesh (IUB) ~1.0 km\nViqarunnisa Noon School, Bashundhara Branch ~0.6 km",
                NeighborhoodHospitals = "Evercare Hospital Dhaka, Bashundhara R/A ~0.8 km\nBashundhara Eye Hospital & Research Institute ~1.2 km\nUnited Hospital Limited, Gulshan 2 ~3.8 km",
                NeighborhoodTransit = "Bashundhara Gate Bus Stop ~1.1 km\nKuril Flyover / Bishwa Road ~1.6 km\nHazrat Shahjalal International Airport ~4.9 km",
                StructuralFeatures = "Solar Rooftop Net-Metering, Rainwater Harvesting, Hydroponic Green Vertical Facade, Thermal Insulated Exterior Walls, Children's Eco-Play Area",
                LongDescription = @"<h3>Green Living & Community Harmony in Bashundhara</h3><p>Conceived and spearheaded by <strong>Recent Living Ltd.</strong> (<em>Better Lifestyle…</em>), <strong>Solaris Eco-Sanctuary</strong> embodies our vision of creating thoughtfully designed homes that enrich people's lives and contribute to better communities.</p><h4>Sustainable Living Highlights:</h4><ul><li>Rooftop solar panels reducing community electricity bills by 40%</li><li>Rainwater harvesting and groundwater recharging system</li><li>Cross-ventilation architecture lowering indoor temperatures naturally</li><li>Spacious 3 & 4 bedroom family apartments (2,400 - 3,100 sq ft)</li></ul>",
                BrochurePdfPath = "/brochures/solaris-eco-sanctuary-brochure.pdf",
                SeoTitle = "Recent Living Solaris Eco-Sanctuary | Bashundhara R/A Dhaka",
                SeoDescription = "Green biophilic apartments in Bashundhara R/A Dhaka by Recent Living Ltd. Better Lifestyle…",
                CreatedAt = DateTime.UtcNow.AddMonths(-3)
            };

            var b4 = new Building
            {
                Name = "Recent Properties Apex Pinnacle Tower",
                Slug = "recent-properties-apex-pinnacle-tower",
                Address = "Road 11 / Kamal Ataturk Avenue, Banani, Dhaka, Bangladesh",
                Latitude = 23.7937,
                Longitude = 90.4046,
                Status = ProjectStatus.Completed,
                CompletionYear = 2023,
                SisterConcern = "Recent Properties Ltd",
                PropertyType = "Commercial",
                PriceStartingFrom = 95000000m,
                TotalUnits = 12,
                HandoverDate = "Handed Over in November 2023",
                Facing = "North-East Facing (Corner Plot)",
                Storied = "2B+G+ 14 Floors",
                ApartmentSize = "2400 Sq. Ft. | 4800 Sq. Ft. (Full Floor)",
                LandSize = "15 Katha",
                ApartmentContains = "Executive Reception, Open-Plan Office Floor, Conference Suite, Server Room, Pantry, Separate Restrooms",
                NeighborhoodSchools = "BRAC University, Mohakhali ~1.8 km\nAIUB Banani Campus ~0.4 km\nBanani Bidyaniketan School & College ~0.6 km",
                NeighborhoodHospitals = "Praava Health, Banani Road 17 ~0.4 km\nYork Hospital, Banani ~0.6 km\nUnited Hospital Limited, Gulshan 2 ~1.5 km",
                NeighborhoodTransit = "Kamal Ataturk Avenue Bus Stop ~0.1 km\nBanani Railway Station ~0.5 km\nHazrat Shahjalal International Airport ~6.2 km",
                StructuralFeatures = "Steel Diagrid Framework, Column-Free Floorplates, Double-Glazed Low-E Glass Curtain Wall, 3 Basements for Ample Parking, NFPA-Compliant Fire Fighting",
                LongDescription = @"<h3>Prime Corporate Headquarters in Banani Commercial Hub</h3><p>Standing prominently in Banani, <strong>Apex Pinnacle Tower</strong> is a Grade-A commercial landmark engineered by <strong>Recent Properties Ltd</strong>. Featuring column-free office floor plates and energy-efficient curtain glass.</p>",
                BrochurePdfPath = "/brochures/apex-pinnacle-brochure.pdf",
                SeoTitle = "Apex Pinnacle Tower | Commercial Space Banani Dhaka",
                SeoDescription = "Grade-A commercial offices in Banani Dhaka by Recent Properties Ltd (Recent Group).",
                CreatedAt = DateTime.UtcNow.AddMonths(-12)
            };

            var b5 = new Building
            {
                Name = "Recent Living Aurora Glasshouse",
                Slug = "recent-living-aurora-glasshouse",
                Address = "Road 27 (Old), Dhanmondi, Dhaka, Bangladesh",
                Latitude = 23.7540,
                Longitude = 90.3768,
                Status = ProjectStatus.Ongoing,
                CompletionYear = 2025,
                SisterConcern = "Recent Living Interior",
                PropertyType = "Mix",
                PriceStartingFrom = 52000000m,
                TotalUnits = 10,
                HandoverDate = "Handover on October 2025",
                Facing = "South Facing",
                Storied = "B+G+ 09 Floors",
                ApartmentSize = "2150 Sq. Ft. | 2650 Sq. Ft.",
                LandSize = "9 Katha",
                ApartmentContains = "03 Beds, 03 Bath (Attached) & 1 Common, 03 Balconies, Double-Height Living, Dining, Kitchen, Commercial Ground Floor",
                NeighborhoodSchools = "Dhanmondi Govt. Boys' High School ~0.5 km\nMaple Leaf International School ~0.7 km\nUniversity of Liberal Arts Bangladesh (ULAB) ~0.6 km",
                NeighborhoodHospitals = "Labaid Specialized Hospital, Dhanmondi ~0.9 km\nIbn Sina Hospital, Dhanmondi ~0.6 km\nSquare Hospitals Ltd., Panthapath ~1.4 km",
                NeighborhoodTransit = "Dhanmondi 27 Bus Stop ~0.2 km\nKarwan Bazar Metro Rail Station ~2.0 km\nFarmgate Metro Rail Station ~1.8 km",
                StructuralFeatures = "Teak & Oak Artisanal Joinery, Custom Italian Kitchen Cabinetry, Private Rooftop Garden, Soundproof Acoustic Partitioning, Smart Ambient Lighting",
                LongDescription = @"<h3>Bespoke Craftsmanship & Thoughtful Spatial Design</h3><p>A flagship interior architecture collaboration between <strong>Recent Living Ltd.</strong> and <strong>Recent Living Interior</strong>. Situated in serene Dhanmondi, featuring handcrafted wooden joinery, imported sanitary fixtures, and customizable turnkey layouts.</p>",
                BrochurePdfPath = "/brochures/aurora-glasshouse-brochure.pdf",
                SeoTitle = "Recent Living Aurora Glasshouse | Dhanmondi Dhaka",
                SeoDescription = "Bespoke handcrafted duplex residences in Dhanmondi, Dhaka by Recent Living Interior and Recent Living Ltd.",
                CreatedAt = DateTime.UtcNow.AddMonths(-4)
            };

            var b6 = new Building
            {
                Name = "Recent Living Vanguard Horizon",
                Slug = "recent-living-vanguard-horizon",
                Address = "Sector 4, Jashimuddin Avenue, Uttara, Dhaka, Bangladesh",
                Latitude = 23.8683,
                Longitude = 90.3980,
                Status = ProjectStatus.Upcoming,
                CompletionYear = 2027,
                SisterConcern = "Recent Living Ltd",
                PropertyType = "Residential",
                PriceStartingFrom = 32000000m,
                TotalUnits = 20,
                HandoverDate = "Handover on September 2027",
                Facing = "North Facing",
                Storied = "B+G+ 09 Floors",
                ApartmentSize = "1650 Sq. Ft. | 1950 Sq. Ft.",
                LandSize = "8.5 Katha",
                ApartmentContains = "03 Beds, 02 Bath (Attached) & 1 Common, 03 Balconies, Living, Dining, Kitchen",
                NeighborhoodSchools = "Uttara High School & College, Sector 7 ~0.8 km\nMilestone College, Uttara ~1.4 km\nScholastica Uttara Campus ~1.2 km",
                NeighborhoodHospitals = "Bangladesh Medical College Hospital, Uttara ~0.7 km\nPopular Diagnostic Centre Ltd., Sector 4 ~0.3 km\nLubana General Hospital, Sector 13 ~1.5 km",
                NeighborhoodTransit = "Jashimuddin Bus Stop ~0.2 km\nUttara North / Center Metro Station ~1.6 km\nHazrat Shahjalal International Airport ~2.1 km",
                StructuralFeatures = "Earthquake Resilient Foundation, Resident Gymnasium, Community Hall, Modern Fire Safety Sprinklers, High-Speed Dual Elevators",
                LongDescription = @"<h3>Elevating Modern Urban Living in Uttara</h3><p>Poised in the heart of Uttara Sector 4, <strong>Recent Living Vanguard Horizon</strong> is developed by <strong>Recent Living Ltd.</strong> (<em>Better Lifestyle…</em>) under a transparent Joint Venture partnership with local landowners.</p>",
                BrochurePdfPath = "/brochures/vanguard-harbor-crest-brochure.pdf",
                SeoTitle = "Recent Living Vanguard Horizon | Uttara Dhaka",
                SeoDescription = "Modern residential condominiums in Uttara Sector 4 Dhaka by Recent Living Ltd.",
                CreatedAt = DateTime.UtcNow.AddMonths(-2)
            };

            await context.Buildings.AddRangeAsync(b1, b2, b3, b4, b5, b6);
            await context.SaveChangesAsync();

            var images = new List<BuildingImage>
            {
                new BuildingImage { BuildingId = b1.Id, ImagePath = "/images/properties/lumina-1.svg", IsPrimary = true, DisplayOrder = 1, Caption = "Exterior architectural facade on Road 79, Gulshan 2" },
                new BuildingImage { BuildingId = b1.Id, ImagePath = "/images/properties/lumina-2.svg", IsPrimary = false, DisplayOrder = 2, Caption = "Grand double-height living salon with panoramic vistas" },
                new BuildingImage { BuildingId = b1.Id, ImagePath = "/images/properties/lumina-3.svg", IsPrimary = false, DisplayOrder = 3, Caption = "Rooftop heated lap pool & relaxation deck" },
                new BuildingImage { BuildingId = b1.Id, ImagePath = "/images/properties/lumina-plan.svg", IsPrimary = false, DisplayOrder = 4, Caption = "Master floor plan & structural layout (4,200 sq ft)" },

                new BuildingImage { BuildingId = b2.Id, ImagePath = "/images/properties/marina-1.svg", IsPrimary = true, DisplayOrder = 1, Caption = "Baridhara lakeside elevation & lush green frontage" },
                new BuildingImage { BuildingId = b2.Id, ImagePath = "/images/properties/marina-2.svg", IsPrimary = false, DisplayOrder = 2, Caption = "Cantilevered infinity pool overlooking Baridhara lake" },
                new BuildingImage { BuildingId = b2.Id, ImagePath = "/images/properties/marina-3.svg", IsPrimary = false, DisplayOrder = 3, Caption = "Master suite with lakefront balcony" },

                new BuildingImage { BuildingId = b3.Id, ImagePath = "/images/properties/solaris-1.svg", IsPrimary = true, DisplayOrder = 1, Caption = "Cascading biophilic vertical forest exterior in Bashundhara" },
                new BuildingImage { BuildingId = b3.Id, ImagePath = "/images/properties/solaris-2.svg", IsPrimary = false, DisplayOrder = 2, Caption = "Sustainable atrium lobby with natural daylighting" },
                new BuildingImage { BuildingId = b3.Id, ImagePath = "/images/properties/solaris-3.svg", IsPrimary = false, DisplayOrder = 3, Caption = "Rooftop solar garden and community recreation space" },

                new BuildingImage { BuildingId = b4.Id, ImagePath = "/images/properties/apex-1.svg", IsPrimary = true, DisplayOrder = 1, Caption = "Banani commercial corridor landmark tower" },
                new BuildingImage { BuildingId = b4.Id, ImagePath = "/images/properties/apex-2.svg", IsPrimary = false, DisplayOrder = 2, Caption = "Column-free Grade-A office floor layout" },
                new BuildingImage { BuildingId = b4.Id, ImagePath = "/images/properties/apex-3.svg", IsPrimary = false, DisplayOrder = 3, Caption = "Rooftop boardroom & executive lounge" },

                new BuildingImage { BuildingId = b5.Id, ImagePath = "/images/properties/aurora-1.svg", IsPrimary = true, DisplayOrder = 1, Caption = "Dhanmondi Road 27 duplex elevation" },
                new BuildingImage { BuildingId = b5.Id, ImagePath = "/images/properties/aurora-2.svg", IsPrimary = false, DisplayOrder = 2, Caption = "Private rooftop garden terrace" },
                new BuildingImage { BuildingId = b5.Id, ImagePath = "/images/properties/aurora-3.svg", IsPrimary = false, DisplayOrder = 3, Caption = "Artisanal woodwork living lounge by Recent Living Interior" },

                new BuildingImage { BuildingId = b6.Id, ImagePath = "/images/properties/vanguard-1.svg", IsPrimary = true, DisplayOrder = 1, Caption = "Uttara Sector 4 modern residential facade" },
                new BuildingImage { BuildingId = b6.Id, ImagePath = "/images/properties/vanguard-2.svg", IsPrimary = false, DisplayOrder = 2, Caption = "Spacious family living room with open terrace" },
                new BuildingImage { BuildingId = b6.Id, ImagePath = "/images/properties/vanguard-3.svg", IsPrimary = false, DisplayOrder = 3, Caption = "Community gymnasium and children's play lounge" }
            };

            await context.BuildingImages.AddRangeAsync(images);

            var reviews = new List<Review>
            {
                new Review { BuildingId = b1.Id, AuthorName = "Dr. Shamsul Alam", Rating = 5, Comment = "Recent Living Ltd. has truly lived up to their promise of a 'Better Lifestyle'. The quality of construction and peaceful living in Gulshan 2 are exemplary.", CreatedAt = DateTime.UtcNow.AddMonths(-3) },
                new Review { BuildingId = b1.Id, AuthorName = "Farhana Begum (Landowner)", Rating = 5, Comment = "As a Joint Venture landowner, my experience with Recent Living Ltd. was completely transparent and professional. Handover was on schedule!", CreatedAt = DateTime.UtcNow.AddMonths(-2) },
                new Review { BuildingId = b2.Id, AuthorName = "Mustafa K. Chowdhury", Rating = 5, Comment = "The lakefront views and engineering quality in Baridhara are second to none. Recent Properties Ltd executed flawlessly.", CreatedAt = DateTime.UtcNow.AddMonths(-1) },
                new Review { BuildingId = b5.Id, AuthorName = "Nabeel Ahsan", Rating = 5, Comment = "Recent Living Interior's woodwork in our Dhanmondi duplex is pure luxury. The finish and attention to detail are remarkable.", CreatedAt = DateTime.UtcNow.AddMonths(-2) }
            };

            await context.Reviews.AddRangeAsync(reviews);
            await context.SaveChangesAsync();
        }
    }
}
