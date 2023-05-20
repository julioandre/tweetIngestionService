using System.Net.Security;
using System.Security.Authentication;
using Cassandra;
using Cassandra.Mapping;
using Microsoft.AspNetCore.Mvc;
using ISession = Cassandra.ISession;
using System.Security.Cryptography.X509Certificates;

namespace tweetIngestion_service.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TweetIngestionController:ControllerBase
{
    private ICluster _cluster;
    private ISession _session;
    private IMapper _mapper;

    public TweetIngestionController(ICluster cluster, ISession session)
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
    }
    public static bool ValidateServerCertificate(
        object sender,
        X509Certificate certificate,
        X509Chain chain,
        SslPolicyErrors sslPolicyErrors)
    {
        if (sslPolicyErrors == SslPolicyErrors.None)
            return true;

        Console.WriteLine("Certificate error: {0}", sslPolicyErrors);
        // Do not allow this client to communicate with unauthenticated servers.
        return false;
    }
}