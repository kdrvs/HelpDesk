using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HelpDesk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using HelpDesk.Data;


namespace HelpDesk.Controllers
{
    public class UsersController : Controller
    {
        [Authorize]
        public IActionResult List()
        {            
            UsersListModel model = new UsersListModel(HttpContext);
            if (model.CurrentUser.ServerRole != "admin")
                return RedirectToAction("Index", "Home");
            ViewBag.Actions = new UserActions(model.CurrentUser).keyValuePairs;
            return View(model);
        }

        [Authorize]
        public IActionResult Edit(string u)
        {
            if (u == null)
                return RedirectToAction("Index", "Home");
            UserModel model = new UserModel(u);
            if(model.User == null)
                return RedirectToAction("Index", "Home");
            var currentUser = new UserModel(HttpContext).User;
            if(currentUser.ServerRole != "admin")
                return RedirectToAction("Index", "Home");
            ViewBag.CurrentUser = currentUser;
            ViewBag.Actions = new UserActions(currentUser).keyValuePairs;
            ViewBag.ServerRole = new SelectList(UserModel.getRolesFromDb(), "ServerRole", "ServerRole", selectedValue: model.User.ServerRole);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(UserModel userModel)
        {
            userModel.User = new UserModel(userModel.UserId.ToString()).User;
            string message;
            int userId = userModel.saveChangeToDb(out message);
            if (userId > 0)
                userModel = new UserModel(userId.ToString());
            ViewBag.Message = message;
            var currentUser = new UserModel(HttpContext).User;
            if (currentUser.ServerRole != "admin")
                return RedirectToAction("Index", "Home");
            ViewBag.CurrentUser = currentUser;
            ViewBag.Actions = new UserActions(currentUser).keyValuePairs;
            ViewBag.ServerRole = new SelectList(UserModel.getRolesFromDb(), "ServerRole", "ServerRole", selectedValue: userModel.User.ServerRole);
            return View(userModel);
        }

        [Authorize]
        public IActionResult New()
        {
            var currentUser = new UserModel(HttpContext).User;
            if (currentUser.ServerRole != "admin")
                return RedirectToAction("Index", "Home");
            ViewBag.CurrentUser = currentUser;
            ViewBag.Actions = new UserActions(currentUser).keyValuePairs;
            ViewBag.ServerRole = new SelectList(UserModel.getRolesFromDb(), "ServerRole", "ServerRole");
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult New(UserModel userModel)
        {
            var currentUser = new UserModel(HttpContext).User;
            if (currentUser.ServerRole != "admin")
                return RedirectToAction("Index", "Home");
            ViewBag.CurrentUser = currentUser;
            ViewBag.Actions = new UserActions(currentUser).keyValuePairs;
            int userId = userModel.saveNewUser();
            if (userId <= 0)
            {
                ViewBag.ServerRole = new SelectList(UserModel.getRolesFromDb(), "ServerRole", "ServerRole", selectedValue: userModel.ServerRole);
                return View(userModel);
            }
            
            return RedirectToAction("Edit", "Users", new { u = userId.ToString() });
        }
    }
}