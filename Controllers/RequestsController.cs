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
    public class RequestsController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            
            List<String> partitions;
            using (HelpdeskContext db = new HelpdeskContext())
            {
                partitions = db.Partitions.Select(p => p.Notation).ToList();
                
            }

            ViewBag.Partitions = new SelectList(partitions);            
            
            ListRequestsModel model = new ListRequestsModel(HttpContext);
            ViewBag.Actions = new UserActions(model.User).keyValuePairs;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Index(string Partitions)
        {
            if (Partitions == null)         
                return RedirectToAction("Index", "Home");
            
            List<String> partitions;
            using (HelpdeskContext db = new HelpdeskContext())
            {
                partitions = db.Partitions.Select(p => p.Notation).ToList();
            }
            ViewBag.Partitions = new SelectList(partitions);
            
            ListRequestsModel model = new ListRequestsModel(HttpContext, Partitions);
            ViewBag.Actions = new UserActions(model.User).keyValuePairs;
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Par(string P)
        {
            if (P == null)
                return RedirectToAction("Index", "Home");

            List<Partitions> partitions;
            using (HelpdeskContext db = new HelpdeskContext())
            {
                partitions = db.Partitions.ToList();
            }
            ViewBag.Partitions = new SelectList(partitions, "Notation", "Notation", selectedValue:P);

            ListRequestsModel model = new ListRequestsModel(HttpContext, P);
            ViewBag.Actions = new UserActions(model.User).keyValuePairs;
            return View("Index", model);           
        }

        [Authorize]
        [HttpGet]
        public IActionResult Request(string val)
        {
            int requestId;
            if(!int.TryParse(val, out requestId))
            {
                return RedirectToAction("Index", "Home");
            }

            RequestModel model = new RequestModel(requestId, HttpContext);
            if(model.Request.Id == 0 || model.Creator is null) 
                return RedirectToAction("Index", "Home");
            StatusHandler statuses = new StatusHandler(model.Request);
            ViewBag.Status = new SelectList(statuses.StatusesOfPartition, "Id", "Notation", selectedValue:statuses.CurrentStatus.Id.ToString());
            ViewBag.Actions = new UserActions(model.CurrentUser.User).keyValuePairs;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Request(RequestModel requestModel)
        {
            
            if(requestModel.saveChangeFromToDb()<0)
                return RedirectToAction("Requests", "Request", new { val = requestModel.Request.Id.ToString() });

            RequestModel model = new RequestModel(requestModel.RequestId, HttpContext);
            if (model.Request.Id == 0 || model.Creator is null)
                return RedirectToAction("Index", "Home");
            StatusHandler statuses = new StatusHandler(model.Request);
            ViewBag.Status = new SelectList(statuses.StatusesOfPartition, "Id", "Notation", selectedValue: statuses.CurrentStatus.Id.ToString());
            ViewBag.Actions = new UserActions(model.CurrentUser.User).keyValuePairs;
            return View(model);
        }

        [Authorize]
        public IActionResult New()
        {
            ViewBag.Partitions = new SelectList(new PartitionsHandler().Partitions, "Id", "Notation");

            NewRequestModel model = new NewRequestModel(HttpContext);
            ViewBag.Actions = new UserActions(model.CurrentUser.User).keyValuePairs;
            return View(model);
        }


        [Authorize]
        public IActionResult Edit(NewRequestModel model)
        {
            model.CurrentUser = new UserModel(HttpContext);
            ViewBag.Partitions = new SelectList(new PartitionsHandler().Partitions, "Id", "Notation");
            ViewBag.Actions = new UserActions(model.CurrentUser.User).keyValuePairs;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult New(NewRequestModel newRequest)
        {
            newRequest.CurrentUser = new UserModel(HttpContext);
            string message;
            int requestId = newRequest.saveChangeToDb(out message);
            if (requestId <= 0)
            {
                newRequest.ErrorMessage = message;
                //return RedirectToAction("Edit", "Request", new { model = newRequest });
                return RedirectToAction("Edit", "Requests", newRequest);
            }
            return RedirectToAction("Request", "Requests", new { val = requestId });
        }

    }
}