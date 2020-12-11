using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ticket_Manager.Data;
using Ticket_Manager.Models;
using Ticket_Manager.ViewModels;

namespace Ticket_Manager.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(ApplicationDbContext db,
                                 UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(Register obj)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = obj.Email, Email = obj.Email, FirstName = obj.FirstName, LastName = obj.LastName };
                var result = await userManager.CreateAsync(user, obj.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "project");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(obj);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignInAsync(SignIn obj)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(obj.Email, obj.Password, obj.RememberMe, false);
                
                if (result.Succeeded)
                {
                    return RedirectToAction("index", "project");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(obj);
        }
        
        public async Task<IActionResult> SignOut ()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("SignIn", "Account");
        }

        public IActionResult Info()
        {
            NameViewModel nameViewModel = (from u in _db.Users
                                          where u.Id == userManager.GetUserId(User)
                                          select new NameViewModel
                                          {
                                              FirstName = u.FirstName,
                                              LastName = u.LastName
                                          }).FirstOrDefault();
            return View(nameViewModel);
        }

        [HttpPost]
        public IActionResult Info(NameViewModel obj)
        {
            if (ModelState.IsValid) 
            {
                ApplicationUser applicationUser = (from u in _db.Users
                                                  where u.Id == userManager.GetUserId(User)
                                                  select u).FirstOrDefault();
                applicationUser.FirstName = obj.FirstName;
                applicationUser.LastName = obj.LastName;
                _db.Users.Update(applicationUser);
                _db.SaveChanges();
                return RedirectToAction("Index","Project");
            }
            return View(obj);
        }
    }
}
