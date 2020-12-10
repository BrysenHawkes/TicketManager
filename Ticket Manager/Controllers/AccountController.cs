using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ticket_Manager.Models;


namespace Ticket_Manager.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager)
        {
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

        [HttpPost]
        public async Task<IActionResult> SignOut ()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("SignIn", "Account");
        }
    }
}
