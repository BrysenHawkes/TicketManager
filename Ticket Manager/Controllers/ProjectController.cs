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
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
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
            return View();
        }

        // Get - Create
        public IActionResult Create()
        {
            return View();
        }

        // Post - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Project obj)
        {
            if (ModelState.IsValid)
            {
                // Give project a JoinID
                obj.JoinId = RandomString(16);
                // Add new project
                _db.Project.Add(obj);
                _db.SaveChanges();

                // Add current user to project by UserProject
                UserProject newUserProject = new UserProject(obj.Id, _userManager.GetUserId(User));
                _db.UserProject.Add(newUserProject);
                _db.SaveChanges();

                // Set Current Project Cookie
                if (Request.Cookies.ContainsKey("CurrentProject"))
                {
                    Response.Cookies.Delete("CurrentProject");
                }
                Response.Cookies.Append("CurrentProject", obj.Id.ToString());

                return RedirectToAction("Index", "Ticket");
            }
            return View(obj);
        }

        // Change Project
        public IActionResult ChangeProject(int id)
        {
            // Check if user is part of project
            if ((from up in _db.UserProject
                 where up.ProjectId == id 
                 && up.UserId == _userManager.GetUserId(User)
                 select up).Any())
            {
                // Change Current Project Cookie
                if (Request.Cookies.ContainsKey("CurrentProject"))
                {
                    Response.Cookies.Delete("CurrentProject");
                }
                Response.Cookies.Append("CurrentProject", id.ToString());

                return RedirectToAction("Index", "Ticket");
            }
            return RedirectToAction("Index", "Project");
        }

        // GET - Project
        public IActionResult Join()
        {
            return View();
        }

        // POST - Join
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Join(string JoinId)
        {
            // Check to see if Project with JoinID Exists
            Nullable<int> ProjectToJoin = (from p in _db.Project
                                           where p.JoinId == JoinId
                                           select p.Id).FirstOrDefault();
            if (ProjectToJoin == 0)
            {
                // Return error
            }
            else
            {
                // Add current user to project by UserProject
                UserProject newUserProject = new UserProject(ProjectToJoin.Value, _userManager.GetUserId(User));
                _db.UserProject.Add(newUserProject);
                _db.SaveChanges();

                // Change current project
                if (Request.Cookies.ContainsKey("CurrentProject"))
                {
                    Response.Cookies.Delete("CurrentProject");
                }
                Response.Cookies.Append("CurrentProject", ProjectToJoin.Value.ToString());
                return RedirectToAction("Index", "Ticket");
            }
            return View();
        }

        private string RandomString(int length)
        {
            string cypher = new string("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890");
            Random rnd = new Random();
            string newString = new string("");

            for (int i = 0; i < length; i++)
            {
                newString += cypher[rnd.Next(36)];
            }
            return newString;
        }
    }
}
