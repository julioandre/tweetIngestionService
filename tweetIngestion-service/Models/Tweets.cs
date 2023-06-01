using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace tweetIngestion_service.Models;

[PrimaryKey("Id")]
public class Tweets
{
    
    public string Id{get; set;}
    
    public string UserID{get; set;}
    public DateTime CreationTime{get; set;}= DateTime.Now;
    [Required]
    [MaxLength(140)]
    public string tweet { get;set; }  
    
    
    public string? ImageURL { get;set; }

    public Tweets(string Id, string UserID, DateTime CreationTime, string tweet, string imageUrl)
    {
        this.Id = Id;
        this.UserID = UserID;
        this.CreationTime = CreationTime;
        this.tweet = tweet;
        this.ImageURL = imageUrl;
    }
}