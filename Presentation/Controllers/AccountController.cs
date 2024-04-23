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
using Data.Entities;

namespace Presentation.Controllers
{
    public class AccountController : Controller
    {
        private IUserRepo _userRepo;
        private IReviewRepo _reviewRepo;
        private StateHelper _stateHelper;
        private readonly INotyfService _notyf;

        public AccountController(IUserRepo userRepo,
             INotyfService notyf,
             StateHelper stateHelper,
             IReviewRepo reviewRepo)
        {
            _userRepo = userRepo;
            _notyf = notyf;
            _stateHelper = stateHelper;
            _reviewRepo = reviewRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Settings()
        {
            var user = _stateHelper.GetUserData();
            return View(new UpdateUserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
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
            });
            return RedirectToAction("Logout", "Account");
        }

        public IActionResult Reviews()
        {
            var reviews = new List<GetReviewDTO>();
            var user = _stateHelper.GetUserData();
            if(user.Role == enRole.Admin)
                reviews = _reviewRepo.Get();
            else
                reviews = _reviewRepo.Get(user.Id);

            return View(reviews);
        }

        [HttpPost]
        public IActionResult Reviews(AddReviewDTO dto)
        {
            _reviewRepo.Add(_stateHelper.GetUserData().Id, dto);
            return RedirectToAction("Reviews", "Account");
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
            else if (!SecurityHelper.ValidateHash(model.Password, user.Password))
            {
                _notyf.Error("User credentials are wrong");
                return View();
            }
            else
            {
                var usr = User;
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, model.Email),
                    new Claim(ClaimTypes.Name, model.Email),
                    new Claim(ClaimTypes.Role, ((enRole)user.Role).ToString()),
                    new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(user))
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                if (!String.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
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
    }
}
