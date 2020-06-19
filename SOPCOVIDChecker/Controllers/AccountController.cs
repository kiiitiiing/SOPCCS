using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SOPCOVIDChecker.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using SOPCOVIDChecker.Data;
using SOPCOVIDChecker.Models;
using SOPCOVIDChecker.Models.AccountViewModels;

namespace SOPCOVIDChecker.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        private readonly IConfiguration _configuration;

        private readonly SOPCCContext _context;

        public AccountController(IUserService userService, IConfiguration configuration, SOPCCContext context)
        {
            _userService = userService;
            _configuration = configuration;
            _context = context;
        }

        public IActionResult Register()
        {
            ViewBag.Facilities = new SelectList(_context.Facility.ToList(), "Id", "Name");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserModel model)
        {
            ViewBag.Facilities = new SelectList(_context.Facility.ToList(), "Id", "Name");
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                if (await _userService.RegisterUserAsync(model, "RHU"))
                {
                    return PartialView(model);
                }
            }

            ViewBag.Errors = errors;
            return PartialView(model);
        }

        public partial class ChangeLoginViewModel
        {
            public int Id { get; set; }
            public string UserLastname { get; set; }
        }

        // GET
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View(new LoginModel
                {
                    ReturnUrl = returnUrl
                });
            }
            else
            {

                if (User.FindFirstValue(ClaimTypes.Role).Equals("RHU"))
                    return RedirectToAction("SopIndex", "Sop");
                else if (User.FindFirstValue(ClaimTypes.Role).Equals("PESU"))
                    return RedirectToAction("PesuStatus", "Pesu");
                else if (User.FindFirstValue(ClaimTypes.Role).Equals("RESU"))
                    return RedirectToAction("ResuIndex", "Resu");
                else if (User.FindFirstValue(ClaimTypes.Role).Equals("LAB"))
                    return RedirectToAction("LabIndex", "Result");
                else if (User.FindFirstValue(ClaimTypes.Role).Equals("admin"))
                    return RedirectToAction("Index", "Admin");
                else
                    return NotFound();
            }
        }

        public IActionResult ModalLoading()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var (isValid, user) = await _userService.ValidateUserCredentialsAsync(model.Username, model.Password);

                if (isValid)
                {
                    await LoginAsync(user, model.RememberMe);
                    if (user.UserLevel.Equals("RHU"))
                        return RedirectToAction("SopIndex", "Sop");
                    else if (user.UserLevel.Equals("PESU"))
                        return RedirectToAction("PesuStatus", "Pesu");
                    else if (user.UserLevel.Equals("RESU"))
                        return RedirectToAction("ResuIndex", "Resu");
                    else if (user.UserLevel.Equals("LAB"))
                        return RedirectToAction("LabIndex", "Result");
                    else if (user.UserLevel.Equals("admin"))
                        return RedirectToAction("Index", "Admin");
                }
                else
                {
                    if (user == null)
                    {
                        ModelState.AddModelError("Username", "User does not exists");
                        ViewBag.Username = "invalid";
                    }
                    else
                    {
                        ModelState.AddModelError("Username", "Wrong Password");
                        ViewBag.Password = "invalid";
                    }
                }
            }
            ViewBag.Result = false;
            return View(model);
        }
        public async Task<IActionResult> Logout(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!_configuration.GetValue<bool>("Account:ShowLogoutPrompt"))
            {
                return await Logout();
            }

            return View();
        }
        public IActionResult AccessDenied(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        public IActionResult NotFound()
        {
            return View();
        }

        public IActionResult Cancel(string returnUrl)
        {
            if (isUrlValid(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            return RedirectToAction("Login", "Account");
        }

        #region Helpers
        private bool isUrlValid(string returnUrl)
        {
            return !string.IsNullOrWhiteSpace(returnUrl) && Uri.IsWellFormedUriString(returnUrl, UriKind.Relative);
        }

        private async Task LoginAsync(Sopusers user, bool rememberMe)
        {
            var properties = new AuthenticationProperties
            {
                AllowRefresh = false,
                IsPersistent = rememberMe
            };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.GivenName, user.Fname),
                new Claim(ClaimTypes.Surname, user.Lname),
                new Claim(ClaimTypes.Role, user.UserLevel),
                new Claim("Facility", user.FacilityId.ToString()),
                new Claim("FacilityName", user.Facility.Name),
                new Claim("Province", user.Province.ToString()),
                new Claim("Muncity", user.Muncity.AddressCheck()),
                new Claim("Barangay", user.Barangay.AddressCheck()),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal, properties);
        }

        public int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        public int UserFacility => int.Parse(User.FindFirstValue("Facility"));
        public int UserProvince => int.Parse(User.FindFirstValue("Province"));
        public int UserMuncity => int.Parse(User.FindFirstValue("Muncity"));
        public int UserBarangay => int.Parse(User.FindFirstValue("Barangay"));
        public string UserName => User.FindFirstValue(ClaimTypes.GivenName) + " " + User.FindFirstValue(ClaimTypes.Surname);
        #endregion
    }
}
