using System;
using System.Linq;
using lidl_twitter_user_service.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace lidl_twitter_user_service.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }

        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    context.Database.EnsureCreated();
                    context.Database.Migrate();
                }
                catch(Exception e)
                {
                    Console.WriteLine($"--> Could not run migrations: {e.Message}");
                }

            }

            if (!context.Users.Any())
            {
                Console.WriteLine("--> seeding data");
                context.Users.AddRange(
                    new User() {Auth0Id = "blahblah1", UserName="Tester1", MentionName = "Testboy1"},
                    new User() {Auth0Id = "blahblah2", UserName = "Tester2", MentionName = "Testboy2" },
                    new User() {Auth0Id = "blahblah3", UserName = "Tester3", MentionName = "Testboy3" }
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
