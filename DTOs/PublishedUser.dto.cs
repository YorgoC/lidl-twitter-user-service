namespace lidl_twitter_user_service.DTOs
{
    public class PublishedUser
    {
        public int Id { get; set; }
        
        public string Auth0Id { get; set; }
        
        public string ProfilePicture { get; set; }

        public string UserName { get; set; }
        
        public string MentionName { get; set; }
        
        public string Event { get; set; }
    }
}