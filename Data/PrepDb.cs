using System;
using System.Linq;
using lidl_twitter_user_service.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace lidl_twitter_user_service.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }

        }

        private static void SeedData(AppDbContext context)
        {

            if (!context.Users.Any())
            {
                Console.WriteLine("--> seeding data");
                context.Users.AddRange(
                    new User() {Email="test1@gmail.nl", UserName="Tester1"},
                    new User() { Email = "test2@gmail.nl", UserName = "Tester2" },
                    new User() { Email = "test3@gmail.nl", UserName = "Tester3" }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> we already have data");
            }

        }
    }
}
