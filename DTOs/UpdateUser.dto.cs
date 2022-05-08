using System;
namespace lidl_twitter_user_service.DTOs
{
    public class UpdateUser : CreateUser 
    {
        public int Id { get; set; }
        
    }
}
