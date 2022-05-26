using lidl_twitter_user_service.DTOs;

namespace lidl_twitter_user_service.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewUser(PublishedUser publishedUser);
    }
}