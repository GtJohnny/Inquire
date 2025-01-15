using Inquire.Data;
using Inquire.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Data;

namespace Inquire.Controllers
{
    [Authorize(Roles ="User,Admin")]
    public class UsersController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(
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
        [Authorize(Roles ="Admin")]
        public IActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            var users = db.Users.ToList();
            ViewBag.Users = users;
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "User,Admin")]

        public IActionResult Delete(string id)
        {
            var user = db.Users.Find(id);
            if(id == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                db.Users.Remove(user);
                db.SaveChanges();
                TempData["message"] = "Utilizatorul a fost sters";
                TempData["messageType"] = "alert-success";
                if (User.IsInRole("Admin"))
                {

                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["message"] = "Nu poti sterge alt utilizator";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index", "Home");
            }



        }
        [Authorize(Roles = "User,Admin")]

        public IActionResult Edit(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            setRights(id);
            if (id == _userManager.GetUserId(User) || User.IsInRole("Admin") && ModelState.IsValid)
            {
                if (User.IsInRole("User")) ViewBag.CurrentRole = "User";
                if (User.IsInRole("Admin")) ViewBag.CurrentRole = "Admin";

                var oldRoleId = db.UserRoles.Where(r => r.UserId == id).First().RoleId;
                var oldRoleName=db.Roles.Find(oldRoleId).Name;
                ViewBag.EditUserRole =oldRoleName;


                return View(user);
            }
            else
            {
                TempData["message"] = "Nu poti edita alt utilizator";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index", "Home");
            }
        }



        private void setRights(string id)
        {
            if (id == _userManager.GetUserId(User)) ViewBag.IsOwner = true;
            if (User.IsInRole("Admin")) ViewBag.IsAdmin=true;
        }




        [HttpPost]
        [Authorize(Roles = "User,Admin")]

        public async Task<IActionResult> Edit(string id, ApplicationUser requestUser,string? NewRole, string? OldRole)
        {
            var user = db.Users.Find(id);
            if (id == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                user.FirstName = requestUser.FirstName;
                user.LastName = requestUser.LastName;
                user.PhoneNumber = requestUser.PhoneNumber;
                user.Email = requestUser.Email;
                user.Bio = requestUser.Bio;
                user.UserName = requestUser.UserName;

                bool ok = false;
                if (NewRole == null) ok = true;
                if (NewRole != null && User.IsInRole("Admin") && OldRole!=NewRole) //mai verificam odata ca userul sa fie admin
                {

                    var res1 = await _userManager.RemoveFromRoleAsync(requestUser, OldRole);
                    var res2 = await _userManager.AddToRoleAsync(requestUser, NewRole);

                    if(res1.Succeeded && res2.Succeeded)
                    {
                        ok = true;
                    }

                    
                }
                

                db.SaveChanges();

                TempData["message"] = "Utilizatorul a fost editat";
                TempData["messageType"] = "alert-success";

                //return RedirectToAction("Show", "Users",id);

                if (User.IsInRole("Admin") && ok)
                {
                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["message"] = "Nu poti edita alt utilizator";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index", "Home");
            }

        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult Show(string id)
        {
            var user = db.Users.Find(id);
            return View(user);
        }
    }
}
