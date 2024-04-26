using Business.DTOs;
using Business.Interfaces;
using Data;
using Data.Commons;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        private IFileService _fileService;

        public VehicleRepo(ApplicationDBContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task Add(CreateVehicleDTO createVehicleDTO)
        {
            var fileItemResponse = await _fileService.UploadFile(createVehicleDTO.FileImage);

            var vehicle = new Vehicle
            {
                Name = createVehicleDTO.Name,
                Description = createVehicleDTO.Description,
                Image = fileItemResponse.FileName,
                Type = (enVehicleType)createVehicleDTO.Type,
                Model = createVehicleDTO.VehicleModel,
                Year = createVehicleDTO.Year,
                Color = createVehicleDTO.Color,
                Location = createVehicleDTO.Location,
                Price = createVehicleDTO.Price,
            };
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();
        }
        public IQueryable<GetVehicleDTO> Get(int vehicleType, string location, decimal price)
        {
            var vehicles = _context.Vehicles
                .Where(_ => (vehicleType != 0 ? _.Type == (enVehicleType)vehicleType : true) &&
                (!string.IsNullOrEmpty(location) ? _.Location.ToLower().Contains(location.ToLower()) : true) &&
                (price != 0 ? _.Price <= price : true) &&
                !_.IsDeleted)
                .Select(_ => new GetVehicleDTO
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
                    Image = _fileService.FileURL(_.Image),
                });
            return vehicles;
        }

        public GetVehicleDTO Get(int id)
        {
            var bookings = _context.Bookings.Where(_ => _.VehicleId == id);
            GetVehicleDTO? vehicle = _context.Vehicles.Include(_ => _.Bookings)
                .Where(_ => _.Id == id && !_.IsDeleted).Select(_ => new GetVehicleDTO
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
                    //Image = _.Image,
                    Image = _fileService.FileURL(_.Image),
                    NotAvailableFrom = bookings.Select(_ => _.From).OrderBy(_ => _).FirstOrDefault(),
                    NotAvailableTo = bookings.Select(_ => _.To).OrderByDescending(_ => _).FirstOrDefault(),
                }).FirstOrDefault();
            return vehicle;
        }
        public void Delete(int id)
        {
            var booking = _context.Vehicles.FirstOrDefault(_ => _.Id == id);
            if (booking != null)
            {
                booking.IsDeleted = true;
                _context.SaveChanges();
            }
        }
    }
}
