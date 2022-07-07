using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HelpDesk.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace HelpDesk.Controllers
{
    public class AccountController : Controller
    {
        [Authorize]
        public IActionResult MyAccount()
        {
            UserModel model = new UserModel(HttpContext);
            
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult MyAccount(UserModel userModel)
        {
            userModel.User = new UserModel(HttpContext).User;
            string message;
            userModel.changePass(out message);
            ViewBag.Message = message;
            return View(userModel);
        }
    }
}