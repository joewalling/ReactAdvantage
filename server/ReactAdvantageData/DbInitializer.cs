using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ReactAdvantageContext _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(
            ReactAdvantageContext db,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            //_db.Database.EnsureCreated();
            _db.Logger.LogInformation("Migrating database");
            _db.Database.Migrate();

            _db.Logger.LogInformation("Seeding database");
            SeedUsers();
            SeedTasks();
        }

        private void SeedTasks()
        {
            if (!_db.Tasks.Any())
            {
                _db.Logger.LogInformation("Seeding tasks and projects");

                var project = new Project { Name = "Create a software product" };

                _db.Tasks.Add(new Task
                {
                    Name = "Create UI",
                    Description = "Create a great looking user interface",
                    DueDate = DateTime.Now,
                    Project = project
                });
                _db.Tasks.Add(new Task
                {
                    Name = "Create Business logic",
                    Description = "Create the business logic",
                    DueDate = DateTime.Now,
                    Project = project
                });

                var project2 = new Project { Name = "Create a second software product" };

                _db.Tasks.Add(new Task
                {
                    Name = "Create login form",
                    Description = "Create a great looking login form",
                    DueDate = DateTime.Now,
                    Project = project2
                });
                _db.Tasks.Add(new Task
                {
                    Name = "Create logic for login",
                    Description = "Create the logic for the login form",
                    DueDate = DateTime.Now,
                    Project = project2
                });
                
                _db.SaveChanges();
            }
        }

        private void SeedUsers()
        {
            if (!_db.Roles.Any(r => r.Name == "Administrator"))
            {
                _db.Logger.LogInformation("Seeding administrator role");
                _roleManager.CreateAsync(new IdentityRole("Administrator")).GetAwaiter().GetResult();
            }
            
            if (!_db.Users.Any())
            {
                _db.Logger.LogInformation("Seeding users");

                var users = new[]
                {
                    new User
                    {
                        UserName = "admin",
                        Email = "jwalling@wallingis.com",
                        EmailConfirmed = true,
                        IsActive = true
                    },
                    new User
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        UserName = "jdoe",
                        Email = "jdoe@123.com",
                        EmailConfirmed = true,
                        IsActive = true
                    },
                    new User
                    {
                        FirstName = "Fred",
                        LastName = "Flintstone",
                        UserName = "fflintstone",
                        Email = "fflintstone@gmail.com",
                        EmailConfirmed = true,
                        IsActive = true
                    },
                    new User
                    {
                        FirstName = "Barney",
                        LastName = "Rubble",
                        UserName = "brubble",
                        Email = "brubble@slate.com",
                        EmailConfirmed = true,
                        IsActive = true
                    }
                };

                foreach (var user in users)
                {
                    _userManager.CreateAsync(user, "Pass123$").GetAwaiter().GetResult();
                }

                _userManager.AddToRoleAsync(users[0], "Administrator").GetAwaiter().GetResult();
            }
        }
    }
}
