using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosTec.Web.Data.Helpers;
using EventosTec.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventosTec.Web.Controllers
{
    public class AccountController : Controller
    {
            private readonly IUserHelper iuhelper;
            public AccountController(IUserHelper userHelper)
            {
                iuhelper = userHelper;
            }
            public IActionResult Login()
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Home");
                }
                return View();
            }
            [HttpPost]
            public async Task<IActionResult> Login(LoginViewModel model)
            {
                if (ModelState.IsValid)
                {
                    var result = await iuhelper.LoginAsync(model);
                    if (result.Succeeded)
                    {
                        if (Request.Query.Keys.Contains("ReturnUrl"))
                        {
                            return Redirect(Request.Query["ReturnUrl"].First());
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
            
            ModelState.AddModelError(string.Empty, "Failed to login.");

            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await iuhelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}