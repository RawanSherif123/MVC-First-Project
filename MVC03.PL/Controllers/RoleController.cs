using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using MVC03.DAL.Models;
using MVC03.PL.Dtos;
using MVC03.PL.Helpers;

namespace MVC03.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager , UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<RoleToReturnDto> roles;

            if (string.IsNullOrEmpty(SearchInput))
            {
                roles = _roleManager.Roles.Select(U => new RoleToReturnDto()
                {
                    Id = U.Id,
                    Name = U.Name,

                });
            }
            else
            {
                roles = _roleManager.Roles.Select(U => new RoleToReturnDto()
                {
                    Id = U.Id,
                    Name = U.Name,

                }).Where(R => R.Name.ToLower().Contains(SearchInput));
            }



            return View(roles);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {

                var role = await _roleManager.FindByNameAsync(model.Name);
                if (role is null)
                {
                    role = new IdentityRole()
                    {
                        Name = model.Name,
                    };

                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                }

            }
                return View(model);
        }



        public async Task<IActionResult> Details(string? id, string viewname = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null) return NotFound(new { statusCode = 404, messege = $"Role With Id:{id} is Not Found" });
            var dto = new RoleToReturnDto()
            {
                Id = role.Id,
                Name= role.Name
            };

            return View(viewname, dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {


            return await Details(id, "Edit");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {

                if (id != model.Id) return BadRequest("Invalid Operations !");
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) return BadRequest("Invalid Operations !");

                var  roleResult = await _roleManager.FindByNameAsync(model.Name);
                if (roleResult is  null)
                {
                    role.Name = model.Name;
                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                ModelState.AddModelError(" ", "Invalid Operations !");
               
            }
            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Operations !");
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) return BadRequest("Invalid Operations !");

                 
                    var result = await _roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                
                ModelState.AddModelError(" ", "Invalid Operations !");


            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role =  await _roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound ();
            var usersInRole = new List<UsersInRoleDto> ();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userInRole = new UsersInRoleDto()
                {
                    UsersId = user.Id,
                    UsersName = user.UserName,
                };

                if ( await _userManager.IsInRoleAsync(user , role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected = false;
                }

                usersInRole.Add(userInRole);
            }
            return View(usersInRole);
            
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId , List<UsersInRoleDto> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound();

          //  ViewData["RoleId"] = roleId;

            if (ModelState.IsValid) 
            { 
                foreach (var user in users)
                {
                    var Appuser = await _userManager.FindByIdAsync(user.UsersId);
                    if (Appuser is  not null)
                    {
                        if (user.IsSelected && ! await _userManager.IsInRoleAsync(Appuser, role.Name))
                        {
                           await _userManager.AddToRoleAsync(Appuser , role.Name);
                        }
                        else if (! user.IsSelected && await _userManager.IsInRoleAsync(Appuser, role.Name))
                        {
                           await _userManager.RemoveFromRoleAsync(Appuser, role.Name);

                        }
                    }
                }

                return RedirectToAction(nameof(Edit) , new {id = roleId});
            }

            return View(users);
        }
    }
}
