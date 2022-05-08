using System;
using AutoMapper;
using lidl_twitter_user_service.Models;
using lidl_twitter_user_service.DTOs;

namespace lidl_twitter_user_service.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            //source -> target
            CreateMap<User, ReadUser>();
            CreateMap<CreateUser, User>();
        }
    }
}
