using Inquire.Data;
using Inquire.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Inquire.Controllers
{
    public class TestController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public TestController(
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
            var currentUser = _userManager.GetUserAsync(User);   

            var id = currentUser.Id; 




            return View();
        }
    }
}
