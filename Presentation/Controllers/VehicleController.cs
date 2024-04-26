using Business.DTOs;
using Business.Implementations;
using Business.Interfaces;
using Data.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using X.PagedList;

namespace Presentation.Controllers
{
    public class VehicleController : Controller
    {

        private IVehicleRepo _vehicleRepo;

        public VehicleController(IVehicleRepo vehicleRepo
            )
        {
            _vehicleRepo = vehicleRepo;
        }

        [HttpGet("Vehicles")]
        public IActionResult Vehicles(int vehicleType = 0, string location = "", decimal price = 0, int pageNumber = 1)
        {
            return View(_vehicleRepo.Get(vehicleType, location, price).ToPagedList(pageNumber, CustomConstants.PageSize));
        }

        [HttpGet("Vehicle/Detail/{id}")]
        public IActionResult Detail(int id)
        {
            return View(_vehicleRepo.Get(id));
        }
        
        [HttpGet("Vehicle/List"),Authorize]
        public IActionResult List(int vehicleType = 0, string location = "", decimal price = 0, int pageNumber = 1)
        {
            return View(_vehicleRepo.Get(vehicleType, location, price).ToPagedList(pageNumber, CustomConstants.PageSize));
        }

        [HttpGet, Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, Authorize]
        public IActionResult Create(CreateVehicleDTO model)
        {
            _vehicleRepo.Add(model);
            return RedirectToAction("List", "Vehicle");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            _vehicleRepo.Delete(Id);
            return RedirectToAction("List", "Vehicle");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Details(int Id)
        {
            GetVehicleDTO vehicle = new();
            vehicle = _vehicleRepo.Get(Id);
            return View(vehicle);
        }
    }
}
