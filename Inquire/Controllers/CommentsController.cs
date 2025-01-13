using Inquire.Data;
using Inquire.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;

namespace Inquire.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public CommentsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public IActionResult Delete(int id)
        {
            Comment comm = db.Commentaries.Find(id);

            if (comm.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                db.Commentaries.Remove(comm);
                db.SaveChanges();
                return Redirect("/Posts/Show/" + comm.PostId);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti acest";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("IndexByCateg", "Posts",comm.Post.CategoryId);
            }
        }

  

        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int id)
        {
            Comment comm = db.Commentaries.Find(id);

            if (comm.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                return View(comm);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa editati acest comentariul";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index", "Articles");
            }
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int id, Comment requestComment)
        {
            Comment comm = db.Commentaries.Find(id);

            if (comm.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                if (ModelState.IsValid)
                {
                    comm.Content = requestComment.Content;

                    db.SaveChanges();

                    return Redirect("/Posts/Show/" + comm.PostId);
                }
                else
                {
                    return View(requestComment);
                }
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa editati acest comentariu";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("IndexByCateg", "Posts",comm.Post.CategoryId);
            }
        }
    }





}
