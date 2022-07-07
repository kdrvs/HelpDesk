using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using HelpDesk.Models;

namespace HelpDesk.Controllers
{
    public class PartitionsController : Controller
    {
        [Authorize]
        public IActionResult List()
        {
            PartitionsList model = new PartitionsList(HttpContext);
            ViewBag.Actions = new UserActions(model.CurrentUser).keyValuePairs;
            return View(model);
        }

        [Authorize]
        public IActionResult Edit(string p)
        {
            PartitionModel model = new PartitionModel(p, HttpContext);
            if (model.CurrentUser.ServerRole != "admin" || !model.isExist)
                return RedirectToAction("Index", "Home");
            ViewBag.Actions = new UserActions(model.CurrentUser).keyValuePairs;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(PartitionModel model)
        {
            model.CurrentUser = new UserModel(HttpContext).User;
            if (model.CurrentUser.ServerRole != "admin")
                return RedirectToAction("Index", "Home");
            model.saveChangesToDb();
            string message = model.message;
            model = new PartitionModel(model.ParId.ToString(), HttpContext);
            model.message = message;
            ViewBag.Actions = new UserActions(model.CurrentUser).keyValuePairs;            
            return View(model);
        }

        [Authorize]
        public IActionResult New()
        {
            PartitionModel model = new PartitionModel(HttpContext);
            if (model.CurrentUser.ServerRole != "admin")
                return RedirectToAction("Index", "Home");
            ViewBag.Actions = new UserActions(model.CurrentUser).keyValuePairs;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult New(PartitionModel model)
        {
            model.CurrentUser = new UserModel(HttpContext).User;
            if (model.CurrentUser.ServerRole != "admin")
                return RedirectToAction("Index", "Home");
            ViewBag.Actions = new UserActions(model.CurrentUser).keyValuePairs;
            int par = model.SaveNewPar();
            if (par <= 0)
                return View(model);
            return RedirectToAction("Edit", "Partitions", new {p = par.ToString() });

        }
    }
}