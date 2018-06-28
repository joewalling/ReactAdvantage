
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ReactAdvantage.Domain.Entities;

namespace ReactAdvantage.Data.EntityFramework
{
    public static   class UsersSeedData
    {
        
       
            public static void EnsureSeedData(this ReactAdvantageContext db)
            {
           
            //Seed data into users table
            if (!db.Users.Any())
                {
                    var user = new User
                    {
                        Name = "admin",
                        FirstName= "admin",
                        Email= "jwalling@jwallingis.com",
                        EmailConfirm=true,
                        Active=true,
                        LastName= "admin",
                        Roles= "admin"


                    };
                var user1 = new User
                {
                    Name = "admin2",
                    FirstName = "admin2",
                    Email = "jwalling@jwallingis2.com",
                    EmailConfirm = true,
                    Active = true,
                    LastName = "admin2",
                    Roles = "admin"

                };
                var user2= new User
                {
                    Name = "admin3",
                    FirstName = "admin3",
                    Email = "jwalling@jwallingis3.com",
                    EmailConfirm = true,
                    Active = true,
                    LastName = "admin3",
                    Roles = "admin"


                };
                var user3 = new User
                {
                    Name = "admin4",
                    FirstName = "admin4",
                    Email = "jwalling@jwallingis.com",
                    EmailConfirm = true,
                    Active = true,
                    LastName = "admin4",
                    Roles = "admin"

                };
                var user4 = new User
                {
                    Name = "admin5",
                    FirstName = "admin5",
                    Email = "jwalling@jwallingis5.com",
                    EmailConfirm = true,
                    Active = true,
                    LastName = "admin",
                    Roles = "admin"


                };
            
              
             
                db.Users.Add(user);
                db.Users.Add(user1);
             
                db.Users.Add(user2);
               
                db.Users.Add(user3);
             
                db.Users.Add(user4);
                db.SaveChanges();
                }
          
           
        }
    }
}
