using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC03.DAL.Models;
using MVC03.PL.Dtos;

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
    }
}
