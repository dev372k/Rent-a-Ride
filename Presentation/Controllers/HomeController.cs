using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using System.Diagnostics;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IVehicleRepo _vehicleRepo;
        private IReviewRepo _reviewRepo;
        private StateHelper _stateHelper;

        public HomeController(IVehicleRepo vehicleRepo,
             IReviewRepo reviewRepo,
            StateHelper stateHelper
            )
        {
            _vehicleRepo = vehicleRepo;
            _reviewRepo = reviewRepo;
            _stateHelper = stateHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Reviews")]
        public IActionResult Reviews()
        {
            var reviews = _reviewRepo.Get();
            return View(reviews);
        }

        [HttpGet("ContactUs")]
        public IActionResult ContactUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
