using Data.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Booking : Base
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public enPaymentStatus Status { get; set; }
        public string PaymentIntent { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
