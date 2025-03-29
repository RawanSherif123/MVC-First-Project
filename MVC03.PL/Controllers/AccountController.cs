using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC03.DAL.Models;
using MVC03.PL.Dtos;
using MVC03.PL.Helpers;

namespace MVC03.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        #region SignUp

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto model)
        {
            if (ModelState.IsValid)
            {

              var User =await  _userManager.FindByNameAsync(model.UserName);
                if (User == null) 
                {
                    User = await _userManager.FindByEmailAsync(model.Email);
                    if (User == null)
                    {
                         User = new AppUser()
                        {
                            UserName = model.UserName,
                            FristName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            IsAgree = model.IsAgree,

                        };
                        var result = await _userManager.CreateAsync(User, model.Password);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("SignIn");
                        }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }

                ModelState.AddModelError("", "Invalid SignUp !!");
            }
            return View(model);
        }

        #endregion

        #region SignIn

        [HttpGet]

        public IActionResult SignIn() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDto model)
        {
            if (ModelState.IsValid) 
            {
              var user = await  _userManager.FindByEmailAsync(model.Email);
                if (user is not null )
                {
                    var flage = await  _userManager.CheckPasswordAsync(user, model.Password);
                    if (flage) 
                    {
                      var result = await  _signInManager.PasswordSignInAsync(user,model.Password,model.RememberMe,false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");

                        }
                    }
                }

                ModelState.AddModelError("", "Invalid Login");
            }
            return View(model);
        }

        #endregion

        #region SignOut


        [HttpGet]
        public new async Task<IActionResult> SignOut() 
        {
           await  _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }

        #endregion

        #region Forget Password

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl( ForgetPasswordDto model)
        {
            if (ModelState.IsValid) 
            {
               var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {

                  var token = await   _userManager.GeneratePasswordResetTokenAsync(user);

                 var url =   Url.Action("ResetPassword","Account", new { email = model.Email , token},Request.Scheme );

                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = url
                    };


                     var flag =  EmailSettings.SendEmail(email);
                    if (flag)
                    {
                        return RedirectToAction("CheckYourInput");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid Reset Password Operations !! ");
            return View("ForgetPassword" , model);
        }

        [HttpGet]
        public IActionResult CheckYourInput()
        {
            return View();
        }
        #endregion

        #region Reset Password

        [HttpGet]
        public IActionResult ResetPassword(string email , string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (ModelState.IsValid) 
            {
              var email = TempData["email"] as string;
             var token = TempData["token"] as string;

                if (email == null || token == null) return BadRequest("Invalid Operations");
               var user = await  _userManager.FindByEmailAsync(email);
                if (user is not null) 
                {
                  var result = await  _userManager.ResetPasswordAsync(user,token,model.NewPassword);
                    if (result.Succeeded) 
                    {
                        return RedirectToAction("SignIn");
                    }
                }
                ModelState.AddModelError("", "Invalid Reset Password Operations");
                
            }
            return View();
        }


        #endregion


    }
}
