using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs
{
    public class CreateTransactionDTO
    {
        public decimal Amount { get; set; }
        public int BookingId { get; set; }
        public int UserId { get; set; }
    }
}
