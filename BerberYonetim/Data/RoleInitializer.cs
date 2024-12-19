using Microsoft.AspNetCore.Identity;

namespace BerberYonetim.Data
{
    public static class RoleInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Admin rolü oluştur
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Admin kullanıcısı oluştur
            var adminUser = await userManager.FindByEmailAsync("admin@berberyonetim.com");
            if (adminUser == null)
            {
                var newAdmin = new IdentityUser
                {
                    UserName = "admin@berberyonetim.com",
                    Email = "admin@berberyonetim.com",
                    EmailConfirmed = true
                };

                string adminPassword = "Admin123!";
                var result = await userManager.CreateAsync(newAdmin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}
