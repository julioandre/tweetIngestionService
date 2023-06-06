
using AutoMapper;
using Confluent.Kafka;
using KafkaFlow.Producers;
using Newtonsoft.Json;
using tweetIngestion_service.Models;
using tweetIngestion_service.Services;

namespace tweetIngestion_service.Messaging;

public class TaskHandler:ITaskHandler
{
    private readonly ILogger<string> _logger;
    private IProducerAccessor _producer;
    private readonly string bootstrapServers = "127.0.0.1:9092";
    private ITweetIngestion _tweetIngestion;
    private readonly IMapper _mapper;

    public TaskHandler( ITweetIngestion tweetIngestion, ILogger<string> logger, IMapper mapper,IProducerAccessor producerAccessor)
    {
        _logger = logger;
        _tweetIngestion = tweetIngestion;
        _mapper = mapper;
        _producer = producerAccessor;

    }
    public async Task HandleTweets(CancellationToken stoppingToken)
    {
        
        string topic = "tweetTopic";
        string groupId = "tweet_group";
        string followerId;
        IList<string> followeeList = new List<string>();
        var config = new ConsumerConfig {GroupId = groupId, BootstrapServers = bootstrapServers, AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest};
        try
        {
            using (var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumerBuilder.Subscribe(topic);
                
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        Console.WriteLine("Starting to Consume");
                        var consumer = consumerBuilder.Consume(stoppingToken);
                        var orderRequest = JsonConvert.DeserializeObject<List<FollowDTO>>(consumer.Message.Value);
                        Console.WriteLine("Consuming");
                        
                        //var followees1 = JsonConvert.DeserializeObject<List<FollowEntity>>(followees.ToList());

                        foreach (var followee in orderRequest)
                        {
                            followeeList.Add(followee.followeeId);
                            //Console.WriteLine(followee.followeeId);
                        }

                        if (followeeList.Count() > 0)
                        {
                            
                            var stuff =_tweetIngestion.GetTweetsTimeline(followeeList);
                            var timelineInfo = new tweetProcessorDTO(orderRequest[0].followerId, stuff);
                            var producer = _producer.GetProducer("put-timeline");
                            await producer.ProduceAsync("key",timelineInfo);
                            
                            
                        }

                    }
                    
                }catch (OperationCanceledException) {
                    consumerBuilder.Close();
                }
            }
        
        }catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }
}