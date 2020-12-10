using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ticket_Manager.Data;
using Ticket_Manager.Models;
using Ticket_Manager.ViewModels;

namespace Ticket_Manager.Controllers
{
    public class TicketController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public TicketController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
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
            TicketIndexViewModel ticketIndexViewModel = new TicketIndexViewModel();
            ticketIndexViewModel.Tickets = from t in _db.Ticket
                                           join u in _db.Users
                                           on t.AssignedTo equals u.Id into tu
                                           from u in tu.DefaultIfEmpty()
                                           where t.ProjectID == int.Parse(Request.Cookies["CurrentProject"])
                                           select new ListTicketViewModel
                                           {
                                               Id = t.Id,
                                               Name = t.Name,
                                               Reported = t.ReportedDate.ToShortDateString(),
                                               Assigned = u.FirstName + " " + u.LastName,
                                               Due = t.DueDate.ToShortDateString(),
                                               Priority = t.Priority,
                                               Status = t.Status
                                           };
            //ViewBag.Ticket = _db.Ticket.ToList();
            //ViewBag.Project = _db.Project.ToList();
            //ViewBag.Project = from p in _db.Project 
            //                  join up in _db.UserProject
            //                  on p.Id equals up.ProjectId
            //                  where up.UserId == _userManager.GetUserId(User)
            //                  select new ProjectViewModel
            //                  { 
            //                      Id = p.Id,
            //                      Name = p.Name
            //                  };
            //TicketIndexViewModel obj = new TicketIndexViewModel();
            //obj.Tickets = _db.Ticket.ToList();
            //obj.Projects = (List<ProjectViewModel>)(from p in _db.Project 
            //               join up in _db.UserProject
            //               on p.Id equals up.ProjectId
            //               where up.UserId == _userManager.GetUserId(User)
            //               select new ProjectViewModel
            //               {
            //                   Id = p.Id,
            //                   Name = p.Name
            //               });
            return View(ticketIndexViewModel);
        }

        // Get - Create
        public IActionResult Create()
        {
            if (!Request.Cookies.ContainsKey("CurrentProject"))
            {
                return RedirectToAction("Index", "Project");
            }
            ViewBag.Users = from up in _db.UserProject
                            join u in _db.Users
                            on up.UserId equals u.Id
                            where up.ProjectId == int.Parse(Request.Cookies["CurrentProject"])
                            select new ListPeopleViewModel
                            {
                                Id = u.Id,
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                            };
            
            return View();
        }

        // Post - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Ticket obj)
        {
            if(ModelState.IsValid)
            {
                obj.ReportedDate = DateTime.Now;
                obj.ProjectID = int.Parse(Request.Cookies["CurrentProject"]);
                obj.ReportedBy = _userManager.GetUserId(User);
                if (obj.AssignedTo == null) {
                    obj.Status = "Unassigned";
                } 
                else
                {
                    obj.Status = "Assigned";
                }
                _db.Ticket.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // Get - Edit
        public IActionResult Edit(int? id)
        {
            if (id==null || id == 0) 
            {
                return NotFound();
            }
            var obj = _db.Ticket.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        // Post - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Ticket obj)
        {
            if (ModelState.IsValid)
            {
                _db.Ticket.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // Get - Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Ticket.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        // Post - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Ticket.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Ticket.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
