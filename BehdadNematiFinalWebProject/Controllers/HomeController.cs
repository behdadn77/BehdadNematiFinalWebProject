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
            return View();
        }
    }
}
