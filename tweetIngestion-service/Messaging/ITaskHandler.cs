namespace tweetIngestion_service.Messaging;

public interface ITaskHandler
{
    public  Task HandleTweets( CancellationToken stoppingToken);
}