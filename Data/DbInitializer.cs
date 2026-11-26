using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstatePortfolio.Models.Entities;
using System.Text;

namespace RealEstatePortfolio.Data
{
    public static partial class DbInitializer
    {
        public static async Task InitializeAsync(
            ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            IWebHostEnvironment env)
        {
            await context.Database.EnsureCreatedAsync();
            await EnsureBuildingColumnsAsync(context);

            await SeedRolesAndUsersAsync(userManager, roleManager);
            GeneratePlaceholderAssets(env.WebRootPath);
            await SeedPageContentsAsync(context);
            await SeedBuildingsAsync(context);
            await SeedContactLeadsAsync(context);
        }

        private static async Task EnsureBuildingColumnsAsync(ApplicationDbContext context)
        {
            var conn = context.Database.GetDbConnection();
            if (conn.State != System.Data.ConnectionState.Open)
                await conn.OpenAsync();

            var existingCols = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "PRAGMA table_info('Buildings');";
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    existingCols.Add(reader.GetString(1));
                }
            }

            var newColumns = new[]
            {
                "HandoverDate",
                "Facing",
                "Storied",
                "ApartmentSize",
                "LandSize",
                "ApartmentContains",
                "NeighborhoodSchools",
                "NeighborhoodHospitals",
                "NeighborhoodTransit"
            };

            foreach (var col in newColumns)
            {
                if (!existingCols.Contains(col))
                {
                    using var alterCmd = conn.CreateCommand();
                    alterCmd.CommandText = $"ALTER TABLE Buildings ADD COLUMN {col} TEXT NULL;";
                    await alterCmd.ExecuteNonQueryAsync();
                }
            }
        }

        private static async Task SeedRolesAndUsersAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Editor" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminEmail = "admin@realestate.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Master Administrator",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, "Admin@123456!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            var editorEmail = "editor@realestate.com";
            var editorUser = await userManager.FindByEmailAsync(editorEmail);
            if (editorUser == null)
            {
                editorUser = new ApplicationUser
                {
                    UserName = editorEmail,
                    Email = editorEmail,
                    FullName = "Senior Portfolio Editor",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(editorUser, "Editor@123456!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(editorUser, "Editor");
                }
            }
        }
    }
}
