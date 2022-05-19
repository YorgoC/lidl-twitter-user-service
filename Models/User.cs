using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace lidl_twitter_user_service.Models
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string Auth0Id { get; set; }
        
        [Required]
        public string UserName { get; set; }
        
        public string MentionName { get; set; }

        public string Bio { get; set; }
        
        [Required]
        [DefaultValue("user")]
        public string Role { get; set; }
        
        public string Location { get; set; }

    }
}
