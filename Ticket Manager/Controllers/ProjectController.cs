using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ticket_Manager.Data;
using Ticket_Manager.Models;

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
            if (Request.Cookies.ContainsKey("CurrentProject"))
            {
                Response.Cookies.Delete("CurrentProject");
            }
            Response.Cookies.Append("CurrentProject", id.ToString());

            return RedirectToAction("Index","Ticket");
        }
    }
}
