namespace Dive.App.Models
{
    public enum VoteType
    {
        Upvote = 1,
        Downvote = 2,
    }

    public class Vote : BaseModel
    {
        public VoteType Type { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}