using System.ComponentModel.DataAnnotations;

namespace tweetIngestion_service.Models;

public class TweetEntity
{
    public string Id{get; set;}
    public string UserID{get; set;}
    public DateTime CreationTime{get; set;}= DateTime.Now;
    [Required] 
    public Content Content{get; set;}
}