using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HelpDesk.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace HelpDesk.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {

            return RedirectToAction("Index", "Requests");
        }

       
        //public IActionResult Privacy()
        //{
           
        //    return View();
        //}

        
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
