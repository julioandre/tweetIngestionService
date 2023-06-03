using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Cassandra;
using Cassandra.Mapping;
using Newtonsoft.Json;
using tweetIngestion_service.Models;
using ISession = Cassandra.ISession;

namespace tweetIngestion_service.Services;

public class TweetIngestionService :ITweetIngestion
{
    private ICluster _cluster;
    private static string _jsonfilepath =
        "/Users/julioandre/RiderProjects/tweetIngestion-service/tweetIngestion-service/DummyData/tweet_Data.json";

    private ISession _session;
    private IMapper _mapper;
    private string insertStatement  = "INSERT INTO  tweeter.tweets (tweetid , userid,creationtime,tweet,imageurl) VALUES (?,?,?,?,?)";
    //private string deleteStatement = "DELETE FROM tweeteer.tweets WHERE (tweetid) VAlLUES (?)";

    public TweetIngestionService()
    {
       
        var options = new Cassandra.SSLOptions(SslProtocols.Tls12, true, ValidateServerCertificate);
        options.SetHostNameResolver((ipAddress) => "julioandre1.cassandra.cosmos.azure.com");
        _cluster = Cluster.Builder()
            .WithCredentials("julioandre1", "k0CHWInzxVJ3QlT6d2zPFW6WNkkuPs0sewU633IGfPdfoV0mpMlZih3rKXNym1iUiQjqHznXOYewACDbXoKsKQ==")
            .WithPort(10350)
            .AddContactPoint("julioandre1.cassandra.cosmos.azure.com")
            .WithSSL(options)
            .Build();
        _session = _cluster.Connect("tweeter");
        _mapper = new Mapper(_session);
    }
    public void createTweet(Tweets tweet)
    {
        
        var boundStatement = _session.Prepare(insertStatement);
        try
        {
            _session.Execute(boundStatement.Bind(tweet.Id, tweet.UserID, tweet.CreationTime, tweet.tweet,
                tweet.ImageURL));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    public IEnumerable<Tweets> GetTweetsByUser(string userId)
    {
        
        var listOfTweets = _mapper.Fetch<Tweets>("Select * from tweets where userid = ?", userId);
        return listOfTweets;
    }

    public IEnumerable<IEnumerable<Tweets>> GetTweetsTimeline(IEnumerable<string> followees)
    {
        IEnumerable<IEnumerable<Tweets>> timeline = new List<IEnumerable<Tweets>>();
        foreach (var userId in followees)
        {
            try
            {
                var temp_timeline = _mapper.Fetch<Tweets>("Select * from tweets where userid = ?", userId);
                timeline.Append(temp_timeline);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching timeline for user " + userId);
            }
        }
        return timeline;
    }

    public Tweets GetTweetsById(string tweetId)
    {
        Tweets tweet = _mapper.FirstOrDefault<Tweets>("Select * from tweets where tweetid = ?", tweetId);
        return tweet;
    }

    public void DeleteTweets(string tweetId)
    {
        _mapper.Delete("DELETE FROM tweeter.tweets WHERE tweetid=? ", tweetId);
    }

    public void createMockTweets()
    {
        var _dummyData = ConvertingJson();
        foreach (var tweet in _dummyData)
        {
            var boundStatement = _session.Prepare(insertStatement);
            try
            {
                _session.Execute(boundStatement.Bind(tweet.Id, tweet.UserID, tweet.CreationTime, tweet.tweet,
                    tweet.ImageURL));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    public static List<Tweets> ConvertingJson()
    {
        using StreamReader reader = new(_jsonfilepath);
        var json = reader.ReadToEnd();
        List<Tweets> tweets = JsonConvert.DeserializeObject<List<Tweets>>(json);
        return tweets;
    }
    public static bool ValidateServerCertificate(
        object sender,
        X509Certificate certificate,
        X509Chain chain,
        SslPolicyErrors sslPolicyErrors)
    {
        if (sslPolicyErrors == SslPolicyErrors.None)
        {
         
            Console.WriteLine("No Errors");
            return true;
        }
            

        Console.WriteLine("Certificate error: {0}");
        // Do not allow this client to communicate with unauthenticated servers.
        return false;
    }
}