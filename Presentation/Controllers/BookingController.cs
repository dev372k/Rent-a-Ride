using AspNetCoreHero.ToastNotification.Abstractions;
using Business.DTOs;
using Business.Implementations;
using Business.Interfaces;
using Data.Commons;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Presentation.Models;
using Stripe;
using Stripe.Checkout;
using X.PagedList;

namespace Presentation.Controllers
{
    public class BookingController : Controller
    {
        private IVehicleRepo _vehicleRepo;
        private IBookingRepo _bookingRepo;
        private IUserRepo _userRepo;
        private IConfiguration _config;
        private ITransactionRepo _transactionRepo;
        private INotyfService _notyf;
        private StateHelper _stateHelper;
        public BookingController(IVehicleRepo vehicleRepo,
            IBookingRepo bookingRepo,
            IUserRepo userRepo,
            ITransactionRepo transactionRepo,
            StateHelper stateHelper,
            INotyfService notyf,
            IConfiguration config)
        {
            _vehicleRepo = vehicleRepo;
            _bookingRepo = bookingRepo;
            _userRepo = userRepo;
            _stateHelper = stateHelper;
            _config = config;
            _transactionRepo = transactionRepo;
            _notyf = notyf;
        }

        [Authorize]
        [HttpGet("Account/Booking")]
        public IActionResult Booking(string query = "", int pageNumber = 1)
        {
            return View(_bookingRepo.Get(_stateHelper.GetUserData().Id).ToPagedList(pageNumber, CustomConstants.PageSize));
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index(int vehicleId)
        {
            var vehicle = _vehicleRepo.Get(vehicleId);
            ViewBag.Vehicle = vehicle;
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            _bookingRepo.Delete(Id);
            return RedirectToAction("Booking", "Account");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Details(int Id)
        {
            var vehicle = _bookingRepo.Get(_stateHelper.GetUserData().Id).FirstOrDefault(_ => _.Id == Id);
            //var vehicle = _bookingRepo.Get(Id).FirstOrDefault(_ => _.Id == Id);
            return View(vehicle);
        }


        [HttpPost]
        public async Task<IActionResult> Index(CreateBookingDTO model)
        {
            try
            {
                TimeSpan difference = model.To.Subtract(model.From);
                StripeConfiguration.ApiKey = _config.GetSection("Stripe:SecretKey").Value;
                var price = (model.Price * 100);
                var user = _userRepo.Get(_stateHelper.GetUserData().Email);
                model.UserId = user.Id;
                var booking = _bookingRepo.Add(model);
                var transaction = _transactionRepo.Add(new CreateTransactionDTO
                {
                    Amount = price,
                    BookingId = booking.Id,
                    UserId = user.Id
                });

                var priceOptions = new PriceCreateOptions
                {
                    Currency = "GBP",
                    UnitAmount = (long?)price,
                    ProductData = new PriceProductDataOptions { Name = $"Rent Vehicle - {Guid.NewGuid()}" },
                };
                var priceService = new PriceService().Create(priceOptions);

                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = _config.GetSection("Stripe:SuccessURL").Value + "?session_id={CHECKOUT_SESSION_ID}&booking_id=" + booking.Id + "&transaction_id=" + transaction,
                    CancelUrl = $"{_config.GetSection("Stripe:CancelURL").Value}",
                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
                    {
                        new Stripe.Checkout.SessionLineItemOptions
                        {
                            Price = priceService.Id,
                            Quantity = difference.Days,
                        },
                    },
                    Mode = "payment",
                };
                var checkoutService = new Stripe.Checkout.SessionService().Create(options);

                return Redirect(checkoutService.Url);
            }
            catch (Exception)
            {
                _notyf.Error("An error occurred");
                return RedirectToAction("Index", "Account", new { vehicleId = model.VehicleId });
            }
        }

        [HttpGet("Success")]
        public async Task<IActionResult> Success([FromQuery] string session_id, [FromQuery] int booking_id = 0, [FromQuery] int transaction_id = 0)
        {
            var session = new SessionService().Get(session_id);

            enPaymentStatus status = enPaymentStatus.Unpaid;
            if (session.Status.ToLower() == enStripeSessionStatus.Complete.ToString().ToLower())
                status = enPaymentStatus.Paid;

            _bookingRepo.Update(booking_id, status, session.PaymentIntentId);
            _transactionRepo.Update(transaction_id, status);

            //_notyf.Success("Booking created successfully");
            return RedirectToAction("Index", "Account");
        }

        [HttpGet("Cancel")]
        public async Task<IActionResult> Cancel()
        {
            //_notyf.Error("There is an error payment method.");
            return RedirectToAction("Index", "Account");
        }
    }
}
