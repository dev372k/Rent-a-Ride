using Business.DTOs;
using Business.Interfaces;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations
{
    public class VehicleRepo : IVehicleRepo
    {
        private ApplicationDBContext _context;

        public VehicleRepo(ApplicationDBContext context)
        {
            _context = context;
        }
        public List<GetVehicleDTO> Get(string query)
        {
            List<GetVehicleDTO> vehicles = _context.Vehicles.Select(_ => new GetVehicleDTO
            {
                Id = _.Id,
                Name = _.Name,
                Description = _.Description,
                Color = _.Color,
                Model = _.Model,
                Year = _.Year,
                Location = _.Location,
                Price = _.Price,
                Type = _.Type,
                Image = _.Image
            }).ToList();
            return vehicles;
        }

        public GetVehicleDTO Get(int id)
        {
            GetVehicleDTO? vehicle = _context.Vehicles.Where(_ => _.Id == id).Select(_ => new GetVehicleDTO
            {
                Id = _.Id,
                Name = _.Name,
                Description = _.Description,
                Color = _.Color,
                Model = _.Model,
                Year = _.Year,
                Location = _.Location,
                Price = _.Price,
                Type = _.Type,
                Image = _.Image
            }).FirstOrDefault();
            return vehicle;
        }
    }
}
