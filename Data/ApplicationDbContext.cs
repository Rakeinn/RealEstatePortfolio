using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstatePortfolio.Models.Entities;

namespace RealEstatePortfolio.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Building> Buildings => Set<Building>();
        public DbSet<BuildingImage> BuildingImages => Set<BuildingImage>();
        public DbSet<ContactLead> ContactLeads => Set<ContactLead>();
        public DbSet<PageContent> PageContents => Set<PageContent>();
        public DbSet<Review> Reviews => Set<Review>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Building configuration
            builder.Entity<Building>(entity =>
            {
                entity.HasIndex(b => b.Slug).IsUnique();
                entity.Property(b => b.Name).IsRequired().HasMaxLength(150);
                entity.Property(b => b.Slug).IsRequired().HasMaxLength(150);
                entity.Property(b => b.SeoTitle).HasMaxLength(65);
                entity.Property(b => b.SeoDescription).HasMaxLength(160);
                entity.Property(b => b.PriceStartingFrom).HasColumnType("decimal(18,2)");

                entity.HasMany(b => b.Images)
                      .WithOne(i => i.Building)
                      .HasForeignKey(i => i.BuildingId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(b => b.Reviews)
                      .WithOne(r => r.Building)
                      .HasForeignKey(r => r.BuildingId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // BuildingImage configuration
            builder.Entity<BuildingImage>(entity =>
            {
                entity.HasIndex(i => new { i.BuildingId, i.DisplayOrder });
            });

            // ContactLead configuration
            builder.Entity<ContactLead>(entity =>
            {
                entity.HasIndex(l => l.SubmittedAt);
                entity.HasIndex(l => l.IsRead);
            });

            // PageContent configuration
            builder.Entity<PageContent>(entity =>
            {
                entity.HasIndex(c => new { c.PageName, c.SectionKey }).IsUnique();
            });
        }
    }
}
