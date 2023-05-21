using System.ComponentModel.DataAnnotations;

namespace tweetIngestion_service.Models;

public class TweetDTO
{
    public string Id{get; set;}
    public string UserID{get; set;}
    public DateTime CreationTime{get; set;}= DateTime.Now;
    [Required]
    [MaxLength(140)]
    public string tweet { get;set; }
    public IFormFile Image { get;set; }
}