using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using HelpDesk.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelpDesk.Controllers
{
    public class RolesController : Controller
    {
        [Authorize]
        public IActionResult List()
        {
            RolesList model = new RolesList(HttpContext);
            if (model.CurrentUser.ServerRole != "admin")
                return RedirectToAction("Index", "Home");
            ViewBag.Actions = new UserActions(model.CurrentUser).keyValuePairs;
            return View(model);
        }

        [Authorize]
        public IActionResult Edit(string r)
        {
            int roleId = 0;
            RoleModel model = new RoleModel(r, HttpContext, out roleId);
            if (roleId <= 0 || model.CurrentUser.ServerRole != "admin")
                return RedirectToAction("Index", "Home");
            ViewBag.Actions = new UserActions(model.CurrentUser).keyValuePairs;
            ViewBag.Partition = new SelectList(model.allPartitions, "Id", "Notation");         

            return View(model);
        }
    }
}