using System.Net.Security;
using System.Security.Authentication;
using Cassandra;
using Cassandra.Mapping;
using Microsoft.AspNetCore.Mvc;
using ISession = Cassandra.ISession;
using System.Security.Cryptography.X509Certificates;
using tweetIngestion_service.Models;

namespace tweetIngestion_service.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TweetIngestionController:ControllerBase
{
    private ICluster _cluster;
    private ISession _session;
    private IMapper _mapper;

    public TweetIngestionController()
    {
        var options = new Cassandra.SSLOptions(SslProtocols.Tls12, true, ValidateServerCertificate);
        options.SetHostNameResolver((ipAddress) => "julioandre1.cassandra.cosmos.azure.com");
        _cluster = Cluster.Builder()
            .WithCredentials("julioandre1", "k0CHWInzxVJ3QlT6d2zPFW6WNkkuPs0sewU633IGfPdfoV0mpMlZih3rKXNym1iUiQjqHznXOYewACDbXoKsKQ==")
            .WithPort(10350)
            .AddContactPoint("julioandre1.cassandra.cosmos.azure.com")
            .WithSSL(options)
            .Build();
        _session = _cluster.Connect("tweeter");
        _mapper = new Mapper(_session);
    }

    [HttpGet]
    [Route("/tweets")]
    public async Task<ActionResult<IEnumerable<Tweets>>> GetTweets()
    {
        var listOfTweets = _mapper.Fetch<Tweets>("Select * from tweets");
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