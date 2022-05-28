using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dive.App.Models
{
    public enum PostType
    {
        Question = 1,
        Answer = 2,
    }

    public class Post
    {
        public int Id { get; set; }

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

        public virtual ICollection<Tag> Tags { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Post()
        {
            Tags = new HashSet<Tag>();
            Anwsers = new HashSet<Post>();
        }
    }
}