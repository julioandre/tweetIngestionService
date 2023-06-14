
using Microsoft.AspNetCore.Mvc;
using KafkaFlow.Producers;
using tweetIngestion_service.Models;
using tweetIngestion_service.Services;

namespace tweetIngestion_service.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]
    public class tweetIngestionController : ControllerBase
    {
      
        private ITweetIngestion _tweetIngestion;
        private IProducerAccessor _producer;

        public tweetIngestionController(ITweetIngestion tweetIngestion, IProducerAccessor producer)
        {
            _tweetIngestion = tweetIngestion;
            _producer = producer;

        }

        [HttpPost]
        [Route("/tweets")]
        public  ActionResult<Tweets> CreateTweets(TweetDTO tweets)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _tweetIngestion.createTweet(tweets);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
            
            var producer = _producer.GetProducer("update-timeline");
            producer.ProduceAsync("key",tweets);
            
            return Ok(nameof(tweets.UserID)+ "Has been sent successfully");
        }
        

        [HttpGet]
        [Route("/timelinetweets")]

        public ActionResult<IEnumerable<Tweets>> GetTimelineTweets(IEnumerable<string> userIds)
        {
            var timeline = _tweetIngestion.GetTweetsTimeline(userIds);
            return Ok(timeline);
        }
        [HttpGet]
        [Route("/tweetsbyuser/{userId}")]

        public ActionResult<IEnumerable<Tweets>> GetTweetsByUser(string userId)
        {
            Console.WriteLine(userId);
            var tweets = _tweetIngestion.GetTweetsByUser(userId);
            return Ok(tweets);
        }

        
    }
}