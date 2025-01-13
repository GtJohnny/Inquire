using System.Collections;
using System.Diagnostics;
using Inquire.Data;
using Inquire.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Inquire.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<HomeController> logger
            )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        public IActionResult Index()
        {
            var categories = db.Categories.OrderBy(a => a.CategoryName);
            ViewBag.Categories = categories;
            if (User.IsInRole("User"))
            {
                ViewBag.IsUser = true;
            }
            if (User.IsInRole("Admin"))
            {
                ViewBag.IsAdmin = true;
            }
            return View(categories);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }
}
