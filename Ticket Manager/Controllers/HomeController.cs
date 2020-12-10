using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Ticket_Manager.Data;
using Ticket_Manager.Models;
using Ticket_Manager.ViewModels;

namespace Ticket_Manager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;



        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            _db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.Project = from p in _db.Project
                              join up in _db.UserProject
                              on p.Id equals up.ProjectId
                              where up.UserId == _userManager.GetUserId(User)
                              select new ListProjectViewModel
                              {
                                  Id = p.Id,
                                  Name = p.Name
                              };
            base.OnActionExecuting(context);
        }

        public IActionResult Index()
        {
            ViewBag.UserId = _userManager.GetUserId(User);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
