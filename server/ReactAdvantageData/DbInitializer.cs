using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReactAdvantage.Domain.Configuration;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ReactAdvantageContext _db;
        private readonly IHostingEnvironment _environment;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private int _defaulTenantId;

        public DbInitializer(
            ReactAdvantageContext db,
            IHostingEnvironment environment,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _environment = environment;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if (_environment.IsEnvironment("Test"))
            {
                _db.Logger.LogInformation("Creating database if doesn't exist");
                _db.Database.EnsureCreated();
            }
            else
            {
                _db.Logger.LogInformation("Migrating database");
                _db.Database.Migrate();
            }

            _db.Logger.LogInformation("Seeding database");
            _defaulTenantId = SeedTenantsAndGetDefaultTenantId();
            SeedUsers();
            SeedTasks();
        }

        private int SeedTenantsAndGetDefaultTenantId()
        {
            if (!_db.Tenants.Any())
            {
                _db.Logger.LogInformation("Seeding tenants");

                _db.Tenants.Add(new Tenant
                {
                    Name = "default"
                });

                _db.SaveChanges();
            }

            return _db.Tenants.First().Id;
        }

        private void SeedTasks()
        {
            if (!_db.Tasks.Any())
            {
                _db.Logger.LogInformation("Seeding tasks and projects");

                var project = new Project { Name = "Create a software product", TenantId = _defaulTenantId };

                _db.Tasks.Add(new Task
                {
                    Name = "Create UI",
                    Description = "Create a great looking user interface",
                    DueDate = DateTime.Now,
                    TenantId = _defaulTenantId,
                    Project = project
                });
                _db.Tasks.Add(new Task
                {
                    Name = "Create Business logic",
                    Description = "Create the business logic",
                    DueDate = DateTime.Now,
                    TenantId = _defaulTenantId,
                    Project = project
                });

                var project2 = new Project { Name = "Create a second software product", TenantId = _defaulTenantId };

                _db.Tasks.Add(new Task
                {
                    Name = "Create login form",
                    Description = "Create a great looking login form",
                    DueDate = DateTime.Now,
                    TenantId = _defaulTenantId,
                    Project = project2
                });
                _db.Tasks.Add(new Task
                {
                    Name = "Create logic for login",
                    Description = "Create the logic for the login form",
                    DueDate = DateTime.Now,
                    TenantId = _defaulTenantId,
                    Project = project2
                });
                
                _db.SaveChanges();
            }
        }

        private void SeedUsers()
        {
            if (!_db.Roles.Any(r => r.Name == RoleNames.HostAdministrator))
            {
                _db.Logger.LogInformation("Seeding host administrator role");
                _roleManager.CreateAsync(new IdentityRole(RoleNames.HostAdministrator)).GetAwaiter().GetResult();
            }

            if (!_db.Roles.Any(r => r.Name == RoleNames.Administrator))
            {
                _db.Logger.LogInformation("Seeding administrator role");
                _roleManager.CreateAsync(new IdentityRole(RoleNames.Administrator)).GetAwaiter().GetResult();
            }
            
            if (!_db.Users.Any())
            {
                _db.Logger.LogInformation("Seeding users");

                var users = new[]
                {
                    new User
                    {
                        UserName = "hostAdmin",
                        Email = "jwalling@wallingis.com",
                        EmailConfirmed = true,
                        IsActive = true
                    },
                    new User
                    {
                        UserName = "admin",
                        Email = "jwalling@wallingis.com",
                        EmailConfirmed = true,
                        IsActive = true,
                        TenantId = _defaulTenantId
                    },
                    new User
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        UserName = "jdoe",
                        Email = "jdoe@123.com",
                        EmailConfirmed = true,
                        IsActive = true,
                        TenantId = _defaulTenantId
                    },
                    new User
                    {
                        FirstName = "Fred",
                        LastName = "Flintstone",
                        UserName = "fflintstone",
                        Email = "fflintstone@gmail.com",
                        EmailConfirmed = true,
                        IsActive = true,
                        TenantId = _defaulTenantId
                    },
                    new User
                    {
                        FirstName = "Barney",
                        LastName = "Rubble",
                        UserName = "brubble",
                        Email = "brubble@slate.com",
                        EmailConfirmed = true,
                        IsActive = true,
                        TenantId = _defaulTenantId
                    }
                };

                foreach (var user in users)
                {
                    using (_db.SetTenantFilterValue(user.TenantId))
                    {
                        _userManager.CreateAsync(user, "Pass123$").GetAwaiter().GetResult();

                        if (user.UserName == "admin")
                        {
                            _userManager.AddToRoleAsync(user, user.TenantId == null ? RoleNames.HostAdministrator : RoleNames.Administrator)
                                .GetAwaiter().GetResult();
                        }
                    }
                }
            }
        }
    }
}
