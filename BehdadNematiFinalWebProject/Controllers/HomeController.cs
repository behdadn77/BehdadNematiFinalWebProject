using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BehdadNematiFinalWebProject.Areas.Identity.Data;
using BehdadNematiFinalWebProject.Models;

namespace BehdadNematiFinalWebProject.Controllers
{
    
    public class HomeController : Controller
    {
       
        RoleManager<IdentityRole> roleManager;
        UserManager<ApplicationUser> userManager;
        SignInManager<ApplicationUser> signInManager;
        
        public HomeController(RoleManager<IdentityRole> _roleManager,
                          UserManager<ApplicationUser> _userManager,
                          SignInManager<ApplicationUser> _signInManager)
        {
            roleManager = _roleManager;
            userManager = _userManager;
            signInManager = _signInManager;
        }
        public async Task<IActionResult> Index()
        {
            var role = await roleManager.FindByNameAsync("admins");
            if (role == null)
            {
                role = new IdentityRole("admins");
                await roleManager.CreateAsync(role);
            }

            role = await roleManager.FindByNameAsync("customers");
            if (role == null)
            {
                role = new IdentityRole("customers");
                await roleManager.CreateAsync(role);
            }

            role = await roleManager.FindByNameAsync("sellers");
            if (role == null)
            {
                role = new IdentityRole("sellers");
                await roleManager.CreateAsync(role);
            }

            var user = await userManager.FindByNameAsync("admin1@gmail.com");
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    Email = "admin1@gmail.com",
                    UserName = "admin1@gmail.com",
                    PhoneNumber = "12234",
                    firstname = "arash",
                    lastname = "arashi"
                };
                var status = await userManager.CreateAsync(user, "wer");
                if (status.Succeeded == true)
                {
                    await userManager.AddToRoleAsync(user, "admins");
                }
            }
            return View();
        }
    }
}
