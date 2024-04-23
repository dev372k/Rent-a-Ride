using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Review : Base
    {
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; } = 0;
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
