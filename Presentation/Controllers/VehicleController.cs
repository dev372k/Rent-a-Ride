using Business.DTOs;
using Business.Interfaces;
using Data.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult List()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateVehicleDTO model)
        {
            return View();
        }
    }
}
