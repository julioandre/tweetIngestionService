namespace tweetIngestion_service.Messaging;

public class Consumer:BackgroundService
{
    private IServiceScopeFactory _serviceProvider;
    private ITaskHandler _handler;
    public Consumer(IServiceScopeFactory serviceProvider)
    {
        _serviceProvider = serviceProvider;
        

    }
    protected async  override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        _handler = scope.ServiceProvider.GetRequiredService<ITaskHandler>();
        await Task.Run(() => _handler.HandleTweets(stoppingToken));
    }
}