using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ticket_Manager.Data;
using Ticket_Manager.Models;
using Ticket_Manager.ViewModels;

namespace Ticket_Manager.Controllers
{
    public class PeopleController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public PeopleController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
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
            if (!Request.Cookies.ContainsKey("CurrentProject"))
            {
                return RedirectToAction("Index", "Project");
            }

            PeopleIndexViewModel peopleIndexViewModel = new PeopleIndexViewModel();
            // Select all users on the current project
            peopleIndexViewModel.PeopleInProject = from up in _db.UserProject  
                                                               join u in _db.Users
                                                               on up.UserId equals u.Id
                                                               where up.ProjectId == int.Parse(Request.Cookies["CurrentProject"])
                                                               select new ListPeopleViewModel
                                                               {
                                                                   FirstName = u.FirstName,
                                                                   LastName = u.LastName,
                                                                   Email = u.Email,
                                                                   UserProjectId = up.Id
                                                               };
            // Get Join ID for the project
            peopleIndexViewModel.JoinId = (from p in _db.Project
                                           where p.Id == int.Parse(Request.Cookies["CurrentProject"])
                                          select p.JoinId).FirstOrDefault();
            return View(peopleIndexViewModel);
        }
    }
}
