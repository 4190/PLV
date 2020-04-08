using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using plv.Models;
using plv.Data;
using plv.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace plv.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(UserManager<ApplicationUser> userManager, 
            ApplicationDbContext _context, 
            ILogger<HomeController> logger)
        {
            _userManager = userManager;
            this._context = _context;
            _logger = logger;
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public IActionResult Index()
        {
            // Get the roles for the current user
            if (this.User.Identity.Name != null)
            {
                var roles = _userManager.GetRolesAsync(_userManager.FindByNameAsync(this.User.Identity.Name).Result).Result.ToList();
                CurrentUserViewModel currentUser = new CurrentUserViewModel
                {
                    UserName = this.User.Identity.Name,
                    Roles = roles
                };
                return View(currentUser);
            }
            else
            {
                CurrentUserViewModel currentUser = new CurrentUserViewModel { UserName = "Anonymous" };
                return View(currentUser);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
