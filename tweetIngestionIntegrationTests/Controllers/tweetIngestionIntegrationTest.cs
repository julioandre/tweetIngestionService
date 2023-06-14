using System.Net;
using System.Net.Http.Json;
using Cassandra;
using tweetIngestion_service.Models;

namespace tweetIngestionIntegrationTests.Controllers;

public class tweetIngestionIntegrationTest
{
    private CustomWebApplicationFactory _factory;
    private HttpClient _client;

    public tweetIngestionIntegrationTest()
    {
        _factory = new CustomWebApplicationFactory();
        _client = new HttpClient();
    }
    [Fact]
    public async Task CreateTweet_returnOkTrue()
    {
        var newTweet = new Tweets(Guid.NewGuid().ToString(),"tguthythiyTest",DateTime.Now,  "The tweets to test Integration", "http://fakeurl");
        // {
        //     FirstName = "John",
        //     LastName = "Doe",
        //     Password = "bigP12$est",
        //     Email = "ghh975rhg@lo.com"
        // };
        var response = await _client.PostAsync("http://localhost:5107/tweets", JsonContent.Create(newTweet));
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    [Fact]
    public async Task GetTweetByUser_returnOkTrue()
    {
        var id = "e415847d-5129-4e54-96b7-20a767ecc1ff";
     
        var response = await _client.GetAsync($"http://localhost:5107/tweetsbyuser/e415847d-5129-4e54-96b7-20a767ecc1ff");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}