using System;
using System.ComponentModel.DataAnnotations;

namespace lidl_twitter_user_service.DTOs
{
    public class create_user
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }    
    }
}
