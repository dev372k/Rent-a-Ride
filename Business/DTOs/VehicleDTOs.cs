using Data.Commons;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs
{
    public class GetVehicleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public List<string> VehicleImages { get; set; }
        public enVehicleType Type { get; set; }
        public string Model { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime NotAvailableFrom { get; set; }
        public DateTime NotAvailableTo { get; set; }
    }

    public class CreateVehicleDTO
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public IFormFile FileImage { get; set; }
        public List<IFormFile> VehicleImages { get; set; }
        //public HttpPostedFileBase FileImage { get; set; }
        public string Description { get; set; }
        public enVehicleType Type { get; set; }
        public string VehicleModel { get; set; }
        public string Year { get; set; }
        public string Color { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
    }
}
