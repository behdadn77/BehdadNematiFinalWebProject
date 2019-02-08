using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BehdadNematiFinalWebProject.Areas.Identity.Data;
using BehdadNematiFinalWebProject.Models.viewModels;

namespace BehdadNematiFinalWebProject.Controllers
{
    public class AccountController : Controller
    {
        RoleManager<IdentityRole> roleManager;
        UserManager<ApplicationUser> userManager;
        SignInManager<ApplicationUser> signInManager;

        public AccountController(RoleManager<IdentityRole> _roleManager,
                              UserManager<ApplicationUser> _userManager,
                              SignInManager<ApplicationUser> _signInManager)
        {
            roleManager = _roleManager;
            userManager = _userManager;
            signInManager = _signInManager;
        }

        public async Task<IActionResult> googleLogin()
        {
            string redirectUrl = "Account/googleLoginCallBack";
            var rediUrl = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", rediUrl);
        }

        public async Task<IActionResult> googleLoginCallBack(string remoteError)
        {
            switch (remoteError)
            {
                case null:

                    var info = await signInManager.GetExternalLoginInfoAsync();
                    var user = await userManager.FindByEmailAsync(info.Principal.FindFirst(System.Security.Claims.ClaimTypes.Email).Value);
                    switch (user)
                    {
                        case null:
                            regiserUserWithGoogleViewModels objuser = new regiserUserWithGoogleViewModels()
                            {
                                name = info.Principal.FindFirst(System.Security.Claims.ClaimTypes.GivenName).Value,
                                lastname = info.Principal.FindFirst(System.Security.Claims.ClaimTypes.Surname).Value,
                                username = info.Principal.FindFirst(System.Security.Claims.ClaimTypes.Email).Value
                            };
                            return View(objuser);

                        default: return RedirectToAction("Index", "Home");

                    }

                default: return View("we have some issue");
            }
        }

        public async Task<IActionResult> regiserUserWithGoogle(regiserUserWithGoogleViewModels model)
        {
            var user = await userManager.FindByNameAsync(model.username);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = model.username,
                    firstname = model.name,
                    lastname = model.lastname,
                    Email = model.username
                };

                var status = await userManager.CreateAsync(user, model.password);
                if (status.Succeeded == true)
                {
                    await userManager.AddToRoleAsync(user, "customers");

                    //var userid = user.Id;
                    //var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    //string actionAddres = Url.Action("confirmEmail", "Account", new { Id = userid, token = token }, "https");

                    //string link = $"<div style='border:2px solid red;border-border-radius:10px;padding:10px'>HI, <b>{user.firstname + " " + user.lastname}</b><br/>"
                    //   + "To confirm your account, Please press below link" +
                    //   $"<a href='{actionAddres}'>Confirm my account</a>"
                    //   + "<br/><b>Asp.Net Core</b><br/>Reza Mohammadpour</div>";

                    //System.Net.Mail.MailMessage mailmsg = new System.Net.Mail.MailMessage("nick.rzmmzr@gmail.com", user.Email);
                    //mailmsg.IsBodyHtml = true;
                    //mailmsg.Subject = "cnfirm email";
                    //mailmsg.Body = link;

                    //System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
                    //smtpClient.Host = "smtp.gmail.com";
                    //smtpClient.Port = 465;
                    //smtpClient.EnableSsl = true;
                    //smtpClient.Credentials = new System.Net.NetworkCredential("nick.rzmmzr@gmail.com", "vahidrzmmzr");
                    //smtpClient.Send(mailmsg);

                    return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction("SingUp");
        }

        public async Task<IActionResult> regiserUserWithoutGoogleComfirm(regiserUserWithoutGoogleViewModels model)
        {
            var user = await userManager.FindByNameAsync(model.username);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = model.username,
                    firstname = model.name,
                    lastname = model.lastname,
                    PhoneNumber = model.phonenumber,
                    Email = model.email
                };

                var status = await userManager.CreateAsync(user, model.password);
                if (status.Succeeded == true)
                {
                    await userManager.AddToRoleAsync(user, "customers");

                    //var userid = user.Id;
                    //var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    //string actionAddres = Url.Action("confirmEmail", "Account", new { Id = userid, token = token }, "https");

                    //string link = $"<div style='border:2px solid red;border-border-radius:10px;padding:10px'>HI, <b>{user.firstname + " " + user.lastname}</b><br/>"
                    //   + "To confirm your account, Please press below link" +
                    //   $"<a href='{actionAddres}'>Confirm my account</a>"
                    //   + "<br/><b>Asp.Net Core</b><br/>Reza Mohammadpour</div>";

                    //System.Net.Mail.MailMessage mailmsg = new System.Net.Mail.MailMessage("nick.rzmmzr@gmail.com", user.Email);
                    //mailmsg.IsBodyHtml = true;
                    //mailmsg.Subject = "cnfirm email";
                    //mailmsg.Body = link;

                    //System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
                    //smtpClient.Host = "smtp.gmail.com";
                    //smtpClient.Port = 465;
                    //smtpClient.EnableSsl = true;
                    //smtpClient.Credentials = new System.Net.NetworkCredential("nick.rzmmzr@gmail.com", "vahidrzmmzr");
                    //smtpClient.Send(mailmsg);
                    var userr = await userManager.FindByNameAsync(model.username);
                    await signInManager.SignInAsync(userr,false);
                    return RedirectToAction("Index", "Home");


                }
            }

            return RedirectToAction("regiserUserWithoutGoogle");
        }
        public IActionResult regiserUserWithoutGoogle()
        {
            return View();
        }

        public async Task<IActionResult> confirmEmail(string Id, string token)
        {
            var user = await userManager.FindByIdAsync(Id);
            await userManager.ConfirmEmailAsync(user, token);

            return RedirectToAction("SingUp");
        }

        public IActionResult login(string returnUrl = "")
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        public async Task<IActionResult> loginConfirm(loginUserViewModel model, string returnUrl ="/Home/Index")
        {
            var user = await userManager.FindByNameAsync(model.username);
            if (user != null)
            {
                var status = await signInManager.PasswordSignInAsync(user, model.password, model.rememberme, false);
                var s = User.Identity.IsAuthenticated;
                if (status.Succeeded == true)
                {
                    return Redirect(returnUrl);
                }
            }
            return RedirectToAction("login");
        }
        public async Task<IActionResult> logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}