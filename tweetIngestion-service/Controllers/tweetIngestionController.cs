using System.Net.Security;
using System.Security.Authentication;
using Cassandra;
using Cassandra.Mapping;
using Microsoft.AspNetCore.Mvc;
using ISession = Cassandra.ISession;
using System.Security.Cryptography.X509Certificates;
using tweetIngestion_service.Models;

namespace tweetIngestion_service.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]
    public class tweetIngestionController : ControllerBase
    {
        private ICluster _cluster;
        private ISession _session;
        private IMapper _mapper;
        private string secondaryIndex = "CREATE INDEX ON tweeter.tweets (userid)";

        private string insertStatement =
            "INSERT INTO  tweeter.tweets (tweetid , userid,creationtime,tweet,imageurl) VALUES (?,?,?,?,?)";

        public tweetIngestionController()
        {
            var options = new Cassandra.SSLOptions(SslProtocols.Tls12, true, ValidateServerCertificate);
            options.SetHostNameResolver((ipAddress) => "julioandre1.cassandra.cosmos.azure.com");
            _cluster = Cluster.Builder()
                .WithCredentials("julioandre1",
                    "k0CHWInzxVJ3QlT6d2zPFW6WNkkuPs0sewU633IGfPdfoV0mpMlZih3rKXNym1iUiQjqHznXOYewACDbXoKsKQ==")
                .WithPort(10350)
                .AddContactPoint("julioandre1.cassandra.cosmos.azure.com")
                .WithSSL(options)
                .Build();
            _session = _cluster.Connect("tweeter");
            _mapper = new Mapper(_session);
        }

        [HttpPost]
        [Route("/tweets")]
        public async Task<ActionResult<Tweets>> CreateTweets(Tweets tweets)
        {

            var boundStatement = _session.Prepare(insertStatement);
            _session.Execute(boundStatement.Bind(tweets.Id, tweets.UserID, tweets.CreationTime, tweets.tweet,
                tweets.ImageURL));
            return Ok(CreatedAtRoute(nameof(tweets.Id), new { Id = tweets.Id }, tweets));
        }

        [HttpGet]
        [Route("/tweets")]

        public async Task<ActionResult<IEnumerable<Tweets>>> GetTweets(string userId)
        {
            _session.Execute(secondaryIndex);
            var listOfTweets = _mapper.Fetch<Tweets>("Select * from tweets where userid = ?", userId);
            return Ok(listOfTweets);
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
}