using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dive.App.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public virtual Post Post { get; set; }

        public virtual User User { get; set; }

        [Column(TypeName = "text")]
        public string Body { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}