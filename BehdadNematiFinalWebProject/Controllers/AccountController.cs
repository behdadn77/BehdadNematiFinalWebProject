using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Areas.Identity.Data;
using Shop.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Shop.Controllers
{
    public class AccountController : Controller
    {
       private readonly RoleManager<IdentityRole> rolemanager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> usermanager;
        private readonly ILogger<LogoutModel> logger;
       
        public AccountController(SignInManager<ApplicationUser> _signInManager,
                              UserManager<ApplicationUser> _usermanager,
                              RoleManager<IdentityRole> _rolemanager,
                              ILogger<LogoutModel> _logger
            )
        {
            signInManager = _signInManager;
            usermanager = _usermanager;
            rolemanager = _rolemanager;
            logger = _logger;
            
        }
        //-------------Login-----------------//
        public IActionResult Login(string ReturnUrl=null)
        {
            return View(ReturnUrl);
        }


        public async Task<IActionResult> LoginConfirm(LoginViewModel obj, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await usermanager.FindByNameAsync(obj.Email);
            if (user != null)
            {
                var status = await signInManager.PasswordSignInAsync(user, obj.Password, obj.RememberMe, false);
                var s = User.Identity.IsAuthenticated;
                if (status.Succeeded == true)
                {
                    return Redirect(returnUrl);
                }
            }

            // User Doesn't exist
            return Json("User Doesn't exist");
            
        }

        //-------------Google Authenication---------------//
        public IActionResult GoogleLogin()
        {
            string redirectAction = Url.Action("RedirectFromGoogleLogin", "Account"); //"/Account/RedirectFromGooleLogin";
            var properties =
                signInManager.ConfigureExternalAuthenticationProperties("Google", redirectAction);
            return new ChallengeResult("Google", properties);
        }
        public async Task<IActionResult> RedirectFromGoogleLogin()
        {
            var info = await signInManager.GetExternalLoginInfoAsync();

            var Email = info.Principal
                .FindFirst(System.Security.Claims.ClaimTypes.Email).Value;
            var name = info.Principal
                .FindFirst(System.Security.Claims.ClaimTypes.GivenName).Value;
            var family = info.Principal
                .FindFirst(System.Security.Claims.ClaimTypes.Surname).Value;

            var founduser = await usermanager.FindByNameAsync(Email);
            if (founduser == null) //User doesn't exist must be registerd 
            {
                return RedirectToAction("SignUp", new SignUpViewModel() {
                    Email=Email,
                    Name=name,
                    Family=family
                });
            }
            else
            {
                //login with google credentials
                
            }

            return RedirectToAction("Index");
        }

        //--------------SignUp------------------//
        public IActionResult SignUp()
        {
                return View();  
            //if (obj==null)
            //{
            //}
            //return View(obj);
        } 

        public async Task<IActionResult> SignUpConfirm(SignUpViewModel obj,string returnUrl=null) 
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            var user = await usermanager.FindByNameAsync(obj.Email); 
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = obj.Email,
                    UserName = obj.Email,
                    PhoneNumber = obj.PhoneNum,
                    FirstName = obj.Name,
                    LastName = obj.Family
                };

                var status = await usermanager.CreateAsync(user,obj.Password);
                if (status.Succeeded == true)
                {
                    await usermanager.AddToRoleAsync(user, "customers");
                }
                return Redirect(returnUrl);
            }
            else
            {
                return View("Account already exists");
            }
        }

        //----------Logout-----------------//
        public async Task<IActionResult> LogOut(string returnUrl = null)
        {
            await signInManager.SignOutAsync();
            logger.LogInformation("User logged out.");
            //if (returnUrl != null)
            //{
            //    return LocalRedirect(returnUrl);
            //}

            return Redirect("/Home/Index");
        }

    }
}