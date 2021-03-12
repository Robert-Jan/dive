using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dive.Models
{
    public class Board
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}