using Cassandra;
using tweetIngestion_service.Data;
using Microsoft.EntityFrameworkCore;
using tweetIngestion_service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<ITweetIngestion, TweetIngestionService>();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseCassandra(connectionString:"HostName=julioandre1.cassandra.cosmos.azure.com;Username=julioandre1;Password=k0CHWInzxVJ3QlT6d2zPFW6WNkkuPs0sewU633IGfPdfoV0mpMlZih3rKXNym1iUiQjqHznXOYewACDbXoKsKQ==;Port=10350",defaultKeyspace:"tweeter",cassandraOptionsAction:null));

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