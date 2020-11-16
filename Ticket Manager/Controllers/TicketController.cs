using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticket_Manager.Data;
using Ticket_Manager.Models;

namespace Ticket_Manager.Controllers
{
    public class TicketController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TicketController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Ticket> objList = _db.Ticket;
            ViewBag.Ticket = _db.Ticket.ToList();
            ViewBag.Project = _db.Project.ToList();
            return View(objList);
        }

        // Get - Create
        public IActionResult Create()
        {
            return View();
        }

        // Post - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Ticket obj)
        {
            if(ModelState.IsValid)
            {
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

        [Authorize]
        public IActionResult Test()
        {
            return View();
        }

        // Change Project
        public IActionResult ChangeProject(int id)
        {
            if (Request.Cookies.ContainsKey("CurrentProject"))
            {
                Response.Cookies.Delete("CurrentProject");
            }
            Response.Cookies.Append("CurrentProject", id.ToString());

            return RedirectToAction("Index");
        }
    }
}
