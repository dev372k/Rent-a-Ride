using Data.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Vehicle : Base
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public enVehicleType Type { get; set; } = enVehicleType.SUV;
        public string Model { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<VehicleImage> VehicleImages { get; set; }
    }
}
