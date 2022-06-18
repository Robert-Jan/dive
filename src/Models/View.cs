namespace Dive.App.Models
{
    public class View : BaseModel
    {
        public int PostId { get; set; }
        public Post Post { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}