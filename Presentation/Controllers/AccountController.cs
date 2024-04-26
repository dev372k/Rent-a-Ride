using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Data.Commons;
using System.Security.Claims;
using Business.Interfaces;
using AspNetCoreHero.ToastNotification.Abstractions;
using Business.DTOs;
using Business.Implementations;
using Data.Helpers;
using Newtonsoft.Json;
using Presentation.Models;
using X.PagedList;
using Data.Entities;

namespace Presentation.Controllers
{
    public class AccountController : Controller
    {
        private IUserRepo _userRepo;
        private IReviewRepo _reviewRepo;
        private IBookingRepo _bookingRepo;
        private StateHelper _stateHelper;
        private readonly INotyfService _notyf;

        public AccountController(IUserRepo userRepo,
             INotyfService notyf,
             StateHelper stateHelper,
             IReviewRepo reviewRepo,
             IBookingRepo bookingRepo)
        {
            _userRepo = userRepo;
            _notyf = notyf;
            _stateHelper = stateHelper;
            _reviewRepo = reviewRepo;
            _bookingRepo = bookingRepo;
        }
        public IActionResult Index()
        {
            var user = _stateHelper.GetUserData();

            if (user.Role == enRole.Admin)
            {
                (int, int) userCount = _userRepo.Count();
                (int, int, string) bookingCount = _bookingRepo.Count();
                return View(new DashboardModel
                {
                    ActiveUsers = userCount.Item1,
                    DisableUsers = userCount.Item2,
                    TotalsUsers = userCount.Item1 + userCount.Item2,
                    TotalBookings = bookingCount.Item1,
                    ActiveBookings = bookingCount.Item2,
                    TotalRevenue = bookingCount.Item3,
                });
            }
            else
            {
                var  role = _stateHelper.GetUserData();
                (int, int, double) bookingCount = _bookingRepo.UserBookingCount(role.Id);
                return View(new DashboardModel
                {
                    TotalBookings = bookingCount.Item1,
                    ActiveBookings = bookingCount.Item2,
                    TotalReviewed = bookingCount.Item3,
                });
            }
        }

        public IActionResult Settings()
        {
            var user = _stateHelper.GetUserData();
            return View(new UpdateUserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                City = user.City,
                Country = user.Country,
                //Password = user.Password
            });
        }

        [HttpPost]
        public IActionResult Settings(UpdateUserDTO dto)
        {
            var user = _stateHelper.GetUserData();
            _userRepo.Update(new UpdateUserDTO
            {
                Id = user.Id,
                Name = dto.Name,
                Email = user.Email,
                Address = dto.Address,
                City = dto.City,
                Country = dto.Country,
            });
            return RedirectToAction("Logout", "Account");
        }

        public IActionResult Reviews()
        {
            var reviews = new List<GetReviewDTO>();
            var user = _stateHelper.GetUserData();
            if (user.Role == enRole.Admin)
                reviews = _reviewRepo.Get();
            //else
            //    reviews = _reviewRepo.Get(user.Id);

            return View(reviews);
        }

        [HttpPost]
        public IActionResult Reviews(AddReviewDTO dto)
        {
            _reviewRepo.Add(dto.BookingId, dto);
            return RedirectToAction("Booking", "Account");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterDTO model)
        {
            try
            {
                _userRepo.Add(model);
                _notyf.Success("User registered successfully.");
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return View();
            }
        }

        [HttpGet]
        public IActionResult Login(string? ReturnUrl = null)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginDTO model)
        {
            var user = _userRepo.Get(model.Email);

            if (user == null)
            {
                _notyf.Error("User not found");
                return View();
            }
            else if(user.IsDeleted)
            {
                _notyf.Error("User is locked. Please contact Administrator");
                return View();
            }
            else if (!SecurityHelper.ValidateHash(model.Password, user.Password))
            {
                _notyf.Error("User credentials are wrong");
                return View();
            }
            else
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, model.Email),
                    new Claim(ClaimTypes.Name, model.Email),
                    new Claim(ClaimTypes.Role, ((enRole)user.Role).ToString()),
                    new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(user))
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                if (user.Role == enRole.Admin)
                    return RedirectToAction("Index", "Account");
                else if (!String.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet("Logout"), Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult Users(int pageNumber = 1)
        {
            var user = _stateHelper.GetUserData();
            var users = _userRepo.Get().Where(_ => _.Id != user.Id);
            return View(users.ToPagedList(pageNumber, CustomConstants.PageSize));
        }

        [HttpPost]
        public IActionResult UpdateStatus(int userId, string status)
        {
            bool _status = true;
            if (status == "on")
                _status = false;
            _userRepo.UpdateStatus(userId, _status);
            return RedirectToAction("Users", "Account");
        }
    }
}
