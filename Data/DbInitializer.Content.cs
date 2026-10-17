using RealEstatePortfolio.Models.Entities;

namespace RealEstatePortfolio.Data
{
    public static partial class DbInitializer
    {
        private static async Task SeedPageContentsAsync(ApplicationDbContext context)
        {
            if (context.PageContents.Any()) return;

            var contents = new List<PageContent>
            {
                new PageContent { PageName = "Home", SectionKey = "Hero_Badge", ContentValue = "Recent Group • Better Lifestyle in Bangladesh" },
                new PageContent { PageName = "Home", SectionKey = "Hero_Title", ContentValue = "Thoughtfully Designed Homes & Iconic Spaces in Bangladesh" },
                new PageContent { PageName = "Home", SectionKey = "Hero_Subtitle", ContentValue = "Recent Group unites Recent Properties Ltd, Recent Living Ltd, and Recent Living Interior to create trusted residential apartments, commercial projects, and transparent joint ventures across Bangladesh." },
                new PageContent { PageName = "Home", SectionKey = "Hero_CTA_Text", ContentValue = "Explore Portfolio" },
                new PageContent { PageName = "Home", SectionKey = "Hero_Secondary_CTA", ContentValue = "Recent Living Ltd" },
                
                new PageContent { PageName = "Home", SectionKey = "About_Badge", ContentValue = "Recent Living Ltd. • Better Lifestyle…" },
                new PageContent { PageName = "Home", SectionKey = "About_Title", ContentValue = "A Trusted & Respected Real Estate Company in Bangladesh" },
                new PageContent { PageName = "Home", SectionKey = "About_Story", ContentValue = "<p><strong>Recent Living Ltd.</strong> (Tagline: <em>Better Lifestyle…</em>) is committed to becoming a trusted and respected real estate company in Bangladesh, creating thoughtfully designed homes and commercial spaces that enrich people\'s lives and contribute to better communities.</p><p>We envision a future where every project we develop reflects quality, trust, thoughtful design, and lasting value—creating places where people can truly enjoy a better lifestyle.</p>" },
                
                new PageContent { PageName = "Home", SectionKey = "Stats_YearsExperience", ContentValue = "18" },
                new PageContent { PageName = "Home", SectionKey = "Stats_CompletedProjects", ContentValue = "32" },
                new PageContent { PageName = "Home", SectionKey = "Stats_SquareFeetDelivered", ContentValue = "4.8M" },
                new PageContent { PageName = "Home", SectionKey = "Stats_GlobalAwards", ContentValue = "15" },

                new PageContent { PageName = "About", SectionKey = "Hero_Title", ContentValue = "Recent Living Ltd. — Better Lifestyle…" },
                new PageContent { PageName = "About", SectionKey = "Mission_Text", ContentValue = "To develop quality residential and commercial projects through trusted partnerships, responsible development, innovative design, and customer-focused service—creating lasting value for landowners, investors, customers, and communities." },
                new PageContent { PageName = "About", SectionKey = "Philosophy_Text", ContentValue = "At Recent Living Ltd., we bring together landowners, investors, professionals, architects, engineers, and customers to transform opportunities into successful real estate projects." },

                new PageContent { PageName = "Contact", SectionKey = "Office_Address", ContentValue = "Recent Group Tower, Road 11, Banani / Gulshan, Dhaka 1213, Bangladesh" },
                new PageContent { PageName = "Contact", SectionKey = "Office_Phone", ContentValue = "+880 2 9876543 / +880 1711 000000" },
                new PageContent { PageName = "Contact", SectionKey = "Office_Email", ContentValue = "info@recentgroup.com" },
                new PageContent { PageName = "Contact", SectionKey = "Operating_Hours", ContentValue = "Saturday - Thursday: 09:30 AM - 07:00 PM BST" }
            };

            await context.PageContents.AddRangeAsync(contents);
            await context.SaveChangesAsync();
        }

        private static async Task SeedContactLeadsAsync(ApplicationDbContext context)
        {
            if (context.ContactLeads.Any()) return;

            var now = DateTime.UtcNow;
            var leads = new List<ContactLead>
            {
                new ContactLead { ClientName = "Engr. Rafiqul Islam", EmailAddress = "rafiqul.islam@dhakagroup.com", PhoneNumber = "+880 1711 234567", Subject = "Joint Venture Landowner Proposal - Gulshan 2", Message = "We own a 14-katha plot on Gulshan 2 North Avenue and wish to discuss a Joint Venture residential development with Recent Living Ltd.", SubmittedAt = now.AddDays(-2), IsRead = false, PreferredProjectSlug = "recent-living-crown-gulshan", TargetedConcern = "Recent Living Ltd" },
                new ContactLead { ClientName = "Tanvir Ahmed", EmailAddress = "tanvir@ahmedcapital.bd", PhoneNumber = "+880 1819 876543", Subject = "Commercial Floor Purchase - Banani", Message = "Interested in purchasing 5,000 sq ft office space developed by Recent Properties Ltd.", SubmittedAt = now.AddDays(-4), IsRead = true, PreferredProjectSlug = "recent-properties-apex-banani", TargetedConcern = "Recent Properties Ltd" },
                new ContactLead { ClientName = "Dr. Nusrat Jahan", EmailAddress = "nusrat.jahan@apollo.com.bd", PhoneNumber = "+880 1912 345678", Subject = "Turnkey Interior Fit-out for Apartment", Message = "Looking for Recent Living Interior team to furnish our duplex apartment in Bashundhara R/A.", SubmittedAt = now.AddDays(-7), IsRead = false, PreferredProjectSlug = "recent-living-solaris-bashundhara", TargetedConcern = "Recent Living Interior" },
                new ContactLead { ClientName = "Kamrul Hassan", EmailAddress = "kamrul.hassan@bproperty.bd", PhoneNumber = "+880 1611 112233", Subject = "Inquiry: 3BHK Apartment in Dhanmondi", Message = "Please send floor plans and payment schedule for Recent Living Green Sanctuary in Dhanmondi.", SubmittedAt = now.AddDays(-12), IsRead = true, PreferredProjectSlug = "recent-living-green-dhanmondi", TargetedConcern = "Recent Living Ltd" },

                // Historical leads for Chart.js
                new ContactLead { ClientName = "Barrister Asif Chowdhury", EmailAddress = "asif@chowdhurychambers.com", PhoneNumber = "+880 1713 998877", Subject = "Joint Venture Discussion - Uttara Sector 4", Message = "Landowner inquiry for 10-katha corner plot.", SubmittedAt = now.AddMonths(-1).AddDays(3), IsRead = true, TargetedConcern = "Recent Living Ltd" },
                new ContactLead { ClientName = "Shabnam Ferdous", EmailAddress = "shabnam@dhakainteriors.com", PhoneNumber = "+880 1817 665544", Subject = "Interior Consultation for Living Salon", Message = "Inquiring about custom Italian kitchen joinery.", SubmittedAt = now.AddMonths(-1).AddDays(-5), IsRead = true, TargetedConcern = "Recent Living Interior" },
                new ContactLead { ClientName = "Zubair Rahman", EmailAddress = "zubair@bengalgroup.bd", PhoneNumber = "+880 1914 556677", Subject = "Commercial Space Inquiry", Message = "Checking commercial floor availability.", SubmittedAt = now.AddMonths(-2).AddDays(10), IsRead = true, TargetedConcern = "Recent Properties Ltd" },
                new ContactLead { ClientName = "Syed Mahbubur", EmailAddress = "mahbub@apexholding.com", PhoneNumber = "+880 1715 443322", Subject = "Residential Duplex Booking", Message = "Requesting brochure for Recent Living Crown.", SubmittedAt = now.AddMonths(-2).AddDays(-4), IsRead = true, TargetedConcern = "Recent Living Ltd" }
            };

            await context.ContactLeads.AddRangeAsync(leads);
            await context.SaveChangesAsync();
        }
    }
}
