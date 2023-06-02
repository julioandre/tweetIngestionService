using tweetIngestion_service.Models;

namespace tweetIngestion_service.Services;

public interface ITweetIngestion
{
    void createTweet(Tweets tweet);
    IEnumerable<Tweets> GetTweetsByUser(string userId);
    public IEnumerable<IEnumerable<Tweets>> GetTweetsTimeline(IEnumerable<string> followees);
    public Tweets GetTweetsById(string tweetId);
    public void DeleteTweets(string tweetId);


}