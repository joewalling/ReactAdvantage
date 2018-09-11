using System;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Data
{
    public static class SeedData
    {
        public static void EnsureSeedData(this ReactAdvantageContext db)
        {
            db.Logger.LogInformation("Seeding database");
            SeedUsers(db);
            SeedTasks(db);
            db.SaveChanges();
        }

 
        private static void SeedTasks(ReactAdvantageContext db)
        {
            if (!db.Tasks.Any())
            {
                db.Logger.LogInformation("Seeding tasks and projects");

                var project = new Project {Name = "Create a software product"};

                db.Tasks.Add(new Task
                {
                    Name = "Create UI",
                    Description = "Create a great looking user interface",
                    DueDate = DateTime.Now,
                    Project = project
                });
                db.Tasks.Add(new Task
                {
                    Name = "Create Business logic",
                    Description = "Create the business logic",
                    DueDate = DateTime.Now,
                    Project = project
                });

                var project2 = new Project { Name = "Create a second software product" };

                db.Tasks.Add(new Task
                {
                    Name = "Create login form",
                    Description = "Create a great looking login form",
                    DueDate = DateTime.Now,
                    Project = project2
                });
                db.Tasks.Add(new Task
                {
                    Name = "Create logic for login",
                    Description = "Create the logic for the login form",
                    DueDate = DateTime.Now,
                    Project = project2
                });
            }
        }

        private static void SeedUsers(ReactAdvantageContext db)
        {
            if (!db.Users.Any())
            {
                db.Logger.LogInformation("Seeding users");

                db.Users.Add(new User
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Name = "jdoe",
                    Email = "jdoe@123.com",
                    IsActive = true
                });

                db.Users.Add(new User
                {
                    FirstName = "Fred",
                    LastName = "Flintstone",
                    Name = "fflintstone",
                    Email = "fflintstone@gmail.com",
                    IsActive = true
                });

                db.Users.Add(new User
                {
                    FirstName = "Barney",
                    LastName = "Rubble",
                    Name = "brubble",
                    Email = "brubble@slate.com",
                    IsActive = true
                });
            }
        }
    }
}