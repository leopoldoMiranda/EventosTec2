﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EventosTec.Web.Data.Helpers;
using EventosTec.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EventosTec.Web.Controllers
{
    public class AccountController : Controller
    {
            private readonly IUserHelper iuhelper;
            private readonly IConfiguration config;//----hecho
        public AccountController(IUserHelper userHelper,IConfiguration configuration)//----hecho
        {
                iuhelper = userHelper;
                config = configuration;//----hecho
        }

        [HttpPost]//----agregado-------------
        public async Task<IActionResult> CreateToken([FromBody]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await iuhelper.GetUserByEMailAsync(model.Username);
                if (user != null)
                {
                    var result = await iuhelper.ValidatePasswordAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                        };
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Tokens:key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            config["Tokens:Issuer"],
                            config["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };
                        return Created(string.Empty, results);
                    }
                }

            }
            return BadRequest();
        }
//----------------------------------------------------------------

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