using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using tweetIngestion_service.Data;

namespace tweetIngestionIntegrationTests.Controllers;

public class CustomWebApplicationFactory:WebApplicationFactory<Program>
{
  
}