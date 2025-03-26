using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC03.DAL.Models;
using MVC03.PL.Dtos;

namespace MVC03.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
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

        #endregion

        #region SignOut

        #endregion
    }
}
