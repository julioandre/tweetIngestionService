using Cassandra.Mapping;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using tweetIngestion_service.Data;
using tweetIngestion_service.Models;

namespace tweetIngestion_service.Services;

public class SeedingData
{
    private static string _jsonfilepath =
        "/Users/julioandre/RiderProjects/tweetIngestion-service/tweetIngestion-service/DummyData/tweet_Data.json";
   

    public static void InitializeData(WebApplication app)
    {
        
        using (var serviceScope = app.Services.CreateScope())
        {
                
            SeedData(serviceScope.ServiceProvider.GetService<ApplicationDbContext>(),serviceScope.ServiceProvider.GetService<ITweetIngestion>());
        }
    }

    public static List<Tweets> ConvertingJson()
    {
        using StreamReader reader = new(_jsonfilepath);
        var json = reader.ReadToEnd();
        List<Tweets> tweets = JsonConvert.DeserializeObject<List<Tweets>>(json);
        return tweets;
    }

    public static void SeedData(ApplicationDbContext context, ITweetIngestion tweetIngestion)
    {
        
        var _dummyData = ConvertingJson();
        foreach (var tweet in _dummyData)
        {
            
           tweetIngestion.createTweet(tweet);
           
        }
    }
}