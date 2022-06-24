using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NpgsqlTypes;

namespace Dive.App.Models
{
    public enum PostType
    {
        Question = 1,
        Answer = 2,
    }

    public class Post : BaseModel
    {
        public PostType Type { get; set; }

        public int? AcceptedAnswerId { get; set; }
        public virtual Post AcceptedAnswer { get; set; }

        public int? ParentId { get; set; }
        public virtual Post Parent { get; set; }

        [InverseProperty("Parent")]
        public virtual ICollection<Post> Anwsers { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [StringLength(300)]
        public string Title { get; set; }

        [Column(TypeName = "text")]
        public string Body { get; set; }

        public NpgsqlTsVector SearchVector { get; set; }

        public int VoteScore { get; set; } = 0;

        public int AnwsersCount { get; set; } = 0;

        public int ViewsCount { get; set; } = 0;

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        public virtual ICollection<View> Views { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }

        public Post()
        {
            Tags = new HashSet<Tag>();
            Comments = new HashSet<Comment>();
            Anwsers = new HashSet<Post>();
            Views = new HashSet<View>();
            Votes = new HashSet<Vote>();
        }
    }
}