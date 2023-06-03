using System.Net.Security;
using System.Security.Authentication;
using Cassandra;
using Cassandra.Mapping;
using Microsoft.AspNetCore.Mvc;
using ISession = Cassandra.ISession;
using System.Security.Cryptography.X509Certificates;
using tweetIngestion_service.Models;
using tweetIngestion_service.Services;

namespace tweetIngestion_service.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]
    public class tweetIngestionController : ControllerBase
    {
        private ICluster _cluster;
        private ISession _session;
        private IMapper _mapper;
        private ITweetIngestion _tweetIngestion;
        public tweetIngestionController(ITweetIngestion tweetIngestion)
        {
           _tweetIngestion = tweetIngestion;
        }

        [HttpPost]
        [Route("/tweets")]
        public async Task<ActionResult<Tweets>> CreateTweets(Tweets tweets)
        {

            _tweetIngestion.createTweet(tweets);
            return Ok(CreatedAtRoute(nameof(tweets.Id), new { Id = tweets.Id }, tweets));
        }
        [HttpPost]
        [Route("/tweetscreatemock")]
        public async Task<ActionResult> CreateTweets()
        {

            _tweetIngestion.createMockTweets();
            return Ok();
        }

        [HttpGet]
        [Route("/timelinetweets")]

        public async Task<ActionResult<IEnumerable<Tweets>>> GetTimelineTweets(IEnumerable<string> userIds)
        {
            var timeline = _tweetIngestion.GetTweetsTimeline(userIds);
            return Ok(timeline);
        }
        [HttpGet]
        [Route("/tweetsbyuser")]

        public async Task<ActionResult<IEnumerable<Tweets>>> GetTweetsByUser(string userId)
        {
            var tweets = _tweetIngestion.GetTweetsByUser(userId);
            return Ok(tweets);
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