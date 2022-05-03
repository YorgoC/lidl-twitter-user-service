using System;
using lidl_twitter_user_service.Models;
using Microsoft.EntityFrameworkCore;

namespace lidl_twitter_user_service.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
        }

        public DbSet<User> Users { get; set; }

    }
}
