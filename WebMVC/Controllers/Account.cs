using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //public IActionResult Register()
        //{
        //    ViewData["Title"] = "注册";
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Register(LoginViewModel model)
        //{
        //    //先创建一个user，不包括密码
        //    var user = new AppUser { UserName = model.UserName };
        //    //将user和密码绑定入库
        //    var result = await _userManager.CreateAsync(user, model.Password);

        //    return Json(result);
        //}

        public IActionResult Login(string returnUrl)
        {
            ViewData["Title"] = "登录";
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            //var returnUrl = Request.QueryString;

            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);

            if (user == null)
            {
                ModelState.AddModelError(nameof(loginViewModel.UserName), $"Email {loginViewModel.UserName} not exists");
            }
            else
            {
                if (await _userManager.CheckPasswordAsync(user, loginViewModel.Password))
                {
                    AuthenticationProperties props = null;
                    if (loginViewModel.RememberMe)
                    {
                        props = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(30))
                        };
                    }
                    await _signInManager.SignInAsync(user, props);

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return Redirect("~/");
                }

                ModelState.AddModelError(nameof(loginViewModel.Password), "密码错误");
            }

            return View(loginViewModel);
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/home/index");
        }
    }
}
