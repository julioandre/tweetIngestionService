using System.ComponentModel.DataAnnotations;

namespace tweetIngestion_service.Models;

public class Content
{
    [Required]
    [MaxLength(140)]
    public string tweet { get;set; }
    public IFormFile Image { get;set; }
}