using System;
using System.Collections.Generic;
using lidl_twitter_user_service.Models;

namespace lidl_twitter_user_service.Data
{
    public interface IUserRepo
    {

        bool SaveChanges();

        IEnumerable<User> getAllUsers();
        User GetUserById(int id);
        void CreateUser(User user);

        User GetUserByAuth0Id(string auth0Id);
    }
}
