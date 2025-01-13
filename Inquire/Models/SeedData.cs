using Inquire.Data;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;

namespace Inquire.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {

                //facem asta doar 1 data
                if (context.Roles.Any())
                {
                    return;
                }

                /*creem roluri
                Admin - administrator
                User - utilizator neinregistrat

                ->utilizator neinregistrat va fi si el

                */
                context.Roles.AddRange(
                    new IdentityRole
                    {
                        Id = "75317e87-cd4c-4b63-9a48-f79fa93d9267", Name = "Admin", NormalizedName = "Admin".ToUpper() },

                    new IdentityRole
                    {
                        Id = "b5d62de5-7a06-447e-b686-d5ffbfdcfa82", Name = "User", NormalizedName = "User".ToUpper() }
                );

                var hasher = new PasswordHasher<ApplicationUser>();

                context.Users.AddRange(
                    new ApplicationUser {
                        Id = "41d0832f-54e3-401d-8ba8-f04be6a7ff4c",
                        UserName = "admin@test.com",
                        EmailConfirmed = true,
                        NormalizedEmail = "ADMIN@TEST.COM",
                        Email = "admin@test.com",
                        NormalizedUserName = "ADMIN@TEST.COM",
                        PasswordHash = hasher.HashPassword(null,"Admin1!")
                    },

                    new ApplicationUser
                    {
                        Id = "d2a813c4-6772-47d0-bd02-7d80a78ec2cf",
                        UserName = "user@test.com",
                        EmailConfirmed = true,
                        NormalizedEmail = "USER@TEST.COM",
                        Email = "user@test.com",
                        NormalizedUserName = "USER@TEST.COM",
                        PasswordHash = hasher.HashPassword(null, "User1!")
                    }
                );

                // ASOCIEREA USER-ROLE

                //Admin
                context.UserRoles.AddRange(
                new IdentityUserRole<string>
                {
                    RoleId = "75317e87-cd4c-4b63-9a48-f79fa93d9267",
                    UserId = "41d0832f-54e3-401d-8ba8-f04be6a7ff4c"
                },

                //User
                new IdentityUserRole<string>
                {
                    RoleId = "b5d62de5-7a06-447e-b686-d5ffbfdcfa82",
                    UserId = "d2a813c4-6772-47d0-bd02-7d80a78ec2cf"
                }
                );
                context.SaveChanges();


            }

        }
    }
}
