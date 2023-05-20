using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace tweetIngestion_service.Models;

[PrimaryKey("Id")]
public class TweetEntity
{
    
    public string Id{get; set;}
    public string UserID{get; set;}
    public DateTime CreationTime{get; set;}= DateTime.Now;
    [Required]
    [MaxLength(140)]
    public string tweet { get;set; }
    public string ImageURL { get;set; }
}