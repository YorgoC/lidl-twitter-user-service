using System;
using System.Collections.Generic;
using System.Linq;
using lidl_twitter_user_service.Models;

namespace lidl_twitter_user_service.Data
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;

        public UserRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateUser(User user)
        {
            if (user==null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _context.Users.Add(user);
        }

        public IEnumerable<User> getAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public bool SaveChanges()
        {
            //If something has changed, return true
            return (_context.SaveChanges() >= 0);
        }
    }
}
