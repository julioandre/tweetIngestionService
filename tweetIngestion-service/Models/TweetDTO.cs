using System.ComponentModel.DataAnnotations;

namespace tweetIngestion_service.Models;

public class TweetDTO
{
    public string UserID{get; set;}
    [Required]
    [MaxLength(140)]
    public string tweet { get;set; }
    public string ImageUrl { get;set; }
}