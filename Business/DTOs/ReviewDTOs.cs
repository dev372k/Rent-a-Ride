using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs
{
    public class AddReviewDTO
    {
        public int BookingId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
    
    public class GetReviewDTO
    {
        public User User { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}
