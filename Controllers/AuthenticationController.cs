using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using HelpDesk.Models;

namespace HelpDesk.Controllers
{
    public class AuthenticationController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserModel userModel)
        {
            if (userModel.isValid())
            {
                var claims = new List<Claim>
                {
                    new Claim("id", userModel.User.Id.ToString()),
                    new Claim(ClaimTypes.Name, userModel.User.Name),
                    new Claim(ClaimTypes.Surname, userModel.User.Surname),
                    new Claim(ClaimTypes.Email, userModel.User.Email),
                    new Claim(ClaimTypes.Role, userModel.User.ServerRole)

                };

                var userIdentity = new ClaimsIdentity(claims, "login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);
                return RedirectToAction("Index", "Home");
            }

            return View();

        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}