using System.Collections.Generic;

namespace Dive.App.Models
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public Tag()
        {
            Posts = new HashSet<Post>();
        }
    }
}