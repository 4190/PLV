using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using plv.Data;
using plv.Models;
using plv.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace plv.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext _context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            this._context = _context;
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        
        public IActionResult UserList()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        [Route("admin/ManageUser/{id}")]
        public IActionResult ManageUser(string id)
        {
            ManageUserViewModel user = new ManageUserViewModel
            {
                User = _userManager.FindByIdAsync(id).Result,
                Role = _roleManager.Roles.ToList()
            };        
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ManageUserViewModel user)
        {
            var User = _userManager.
            return Content(user.User.UserName);
            /*
            if(!ModelState.IsValid)
            {
                ManageUserViewModel model = user;
                return View("ManageUser", user);
            }

            if (user.User.Id == "" || user.User.Id == null) { return NotFound(); }

            else
            {
                _userManager.AddToRoleAsync(user.User, user.SelectedRole.Name);
            }
            return RedirectToAction("Admin", "UserList");
            */
        }
        
    }
}