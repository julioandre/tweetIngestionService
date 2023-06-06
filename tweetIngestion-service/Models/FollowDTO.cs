namespace tweetIngestion_service.Models;

public class FollowDTO
{
    public string followeeId { get; set; }
   
    public string followerId { get; set; }
    
    public DateTime followDate { get; set; }
}