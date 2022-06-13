using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace lidl_twitter_user_service.DTOs
{
    public class CreateUser
    {
        [Required]
        public string Auth0Id { get; set; }
        public string UserName { get; set; }
        public string MentionName { get; set; }
        public string Bio { get; set; }
        public string ProfilePicture { get; set; }
    }
}
