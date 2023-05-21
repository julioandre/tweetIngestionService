using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Cassandra;
using Cassandra.Mapping;
using tweetIngestion_service.Models;
using ISession = Cassandra.ISession;

namespace tweetIngestion_service.Services;

public class TweetIngestionService :ITweetIngestion
{
    private ICluster _cluster;
    private ISession _session;
    private IMapper _mapper;
    private string insertStatement  = "INSERT INTO  tweeteer.tweets (tweetid , userid,creationtime,tweet,imageurl) VALUES (?,?,?,?,?)";


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
       _session.Execute(boundStatement.Bind(tweet.Id,tweet.UserID,tweet.CreationTime,tweet.tweet,tweet.ImageURL));
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