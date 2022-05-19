using System;
namespace lidl_twitter_user_service.DTOs
{
    public class ReadUser
    {
    
        public int Id { get; set; }
  
        public string UserName { get; set; }

        public string MentionName { get; set; }

        public string Bio { get; set; }
        
        public string Role { get; set; }
        
        public string Location { get; set; }
    }
}
