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

        [HttpPost]
        public async Task<IActionResult> Edit(ManageUserViewModel model)
        {
            string list = Request.Form["idList"];
            ApplicationUser user = await _userManager.FindByIdAsync(model.User.Id);
/*
            // Select the user, and then add the role to the user
            if (!_userManager.IsInRoleAsync(user, roleName).Result)
            {
                var userResult = await _userManager.AddToRoleAsync(user, roleName);
            }
            */
            return Content($"{list}\n");
        }

        public IActionResult ManageRoles()
        {
            var viewModel = new ManageRoleViewModel
            {
                Role = _roleManager.Roles.ToList()
            };

            return View(viewModel);
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

        public IActionResult UserList()
        {
            var users = _context.Users.ToList();
            return View(users);
        }
    }
}