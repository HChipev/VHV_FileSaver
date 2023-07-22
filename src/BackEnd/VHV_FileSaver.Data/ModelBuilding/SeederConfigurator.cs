using VHV_FileSaver.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VHV_FileSaver.Data.Models;

namespace VHV_FileSaver.Data.ModelBuilding
{
    public static class SeederConfigurator
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = Roles.Administrator,
                    NormalizedName = Roles.Administrator.ToUpper()
                },
                new Role
                {
                    Id = 2,
                    Name = Roles.Employee,
                    NormalizedName = Roles.Employee.ToUpper()
                }
            );

            var password = "pass1234";
            var passwordHasher = new PasswordHasher<User>();

            var admin = new User
            {
                Id = 1,
                Email = "vasil@admin.com",
                UserName = "vasil@admin.com",
                FirstName = "Vasil",
                LastName = "Trendafilov",
                EmailConfirmed = true
            };

            admin.PasswordHash = passwordHasher.HashPassword(admin, password);
            admin.NormalizedEmail = admin.Email.ToUpper();
            admin.NormalizedUserName = admin.UserName.ToUpper();
            admin.SecurityStamp = Guid.NewGuid().ToString();

            var users = new List<User>()
            {
                admin,
            };

            modelBuilder.Entity<User>().HasData(users);

            var userRoles = new List<UserRole>
            {
                new UserRole
                {
                    UserId = users[0].Id,
                    RoleId = 1
                }
            };

            modelBuilder.Entity<UserRole>().HasData(userRoles);
        }
    }
}
