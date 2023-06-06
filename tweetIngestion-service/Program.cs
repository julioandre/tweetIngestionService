using Cassandra;
using KafkaFlow;
using KafkaFlow.Serializer;
using tweetIngestion_service.Data;
using Microsoft.EntityFrameworkCore;
using tweetIngestion_service.Messaging;
using tweetIngestion_service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<ITweetIngestion, TweetIngestionService>();
builder.Services.AddScoped<ITaskHandler, TaskHandler>();
builder.Services.AddHostedService<Consumer>();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseCassandra(connectionString:"HostName=julioandre1.cassandra.cosmos.azure.com;Username=julioandre1;Password=k0CHWInzxVJ3QlT6d2zPFW6WNkkuPs0sewU633IGfPdfoV0mpMlZih3rKXNym1iUiQjqHznXOYewACDbXoKsKQ==;Port=10350",defaultKeyspace:"tweeter",cassandraOptionsAction:null));
builder.Services.AddKafka(kafka => kafka.AddCluster(cluster =>
{
    const string topicname = "tweetProcessorTopic";
    const string topicname2 = "newTweetTopic";
    Console.WriteLine("Creating Topic"); 
    cluster.WithBrokers(new[] { "127.0.0.1:9092" }).CreateTopicIfNotExists(topicname,2,4)
        .AddProducer("put-timeline", producer => producer.DefaultTopic(topicname)
            .AddMiddlewares(middlewares => middlewares.AddSerializer<JsonCoreSerializer>()))
        .CreateTopicIfNotExists(topicname2, 2, 3)
        .AddProducer("update-timeline", producer => producer.DefaultTopic(topicname2)
            .AddMiddlewares(middlewares => middlewares.AddSerializer<JsonCoreSerializer>()));

}));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//SeedingData.InitializeData(app);
app.UseAuthorization();

app.MapControllers();

app.Run();