namespace tweetIngestion_service.Models;

public class tweetProcessorDTO
{
    public string UserID{get; set;}
    public IList<Tweets> followeeTweets{get; set;}

    public tweetProcessorDTO(string userID, IList<Tweets> followeeTweet)
    {
        this.UserID = userID;
        this.followeeTweets = followeeTweet;
    }
}