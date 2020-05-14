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
using System.Collections;

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ManageUserViewModel model)
        {
            string selectedRoleCheckboxes = Request.Form["idList"];
            string[] checkboxesRoleValuesTable = selectedRoleCheckboxes.Split(',');

            List<string> rolesToRemoveList = new List<string>();
            List<string> rolesToAddList = new List<string>();
            ApplicationUser user = await _userManager.FindByIdAsync(model.User.Id);


            foreach (var role in checkboxesRoleValuesTable)
            {
                if(role.Contains("unchecked-"))
                    rolesToRemoveList.Add(role.Substring(role.IndexOf('-') + 1));
                else
                    rolesToAddList.Add(role);
            }
            foreach(string role in rolesToAddList)
            {
                if (!_userManager.IsInRoleAsync(user, role).Result)
                {
                    var userResult = await _userManager.AddToRoleAsync(user, role);
                }
            }
            foreach(string removeRole in rolesToRemoveList)
            {
                if(_userManager.IsInRoleAsync(user, removeRole).Result)
                {
                    var userResult = await _userManager.RemoveFromRoleAsync(user, removeRole);
                }
            }

            return RedirectToAction("UserList");
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
            var selectedUser = _userManager.FindByIdAsync(id).Result;
            ManageUserViewModel viewModel = new ManageUserViewModel
            {
                User = selectedUser,
                RolesList = _roleManager.Roles.ToList(),
                CurrentUserRoles = _userManager.GetRolesAsync(selectedUser).Result.ToList()
            };
            return View(viewModel);
        }

        public IActionResult UserList()
        {
            var users = _context.Users.ToList();
            return View(users);
        }
    }
}