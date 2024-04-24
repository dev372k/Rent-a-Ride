using Data.Commons;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs
{
    public class CreateBookingDTO
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int VehicleId { get; set; }
        public int UserId { get; set; } = 0;
    }

    public class GetBookingDTO
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public bool IsReview { get; set; } 
        public string Status { get; set; }
        public GetUserDTO User { get; set; }
        public GetVehicleDTO Vehicle { get; set; }
    }
}
