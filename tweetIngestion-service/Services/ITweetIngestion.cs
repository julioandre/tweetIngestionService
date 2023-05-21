using tweetIngestion_service.Models;

namespace tweetIngestion_service.Services;

public interface ITweetIngestion
{
    void createTweet(Tweets tweet);


}