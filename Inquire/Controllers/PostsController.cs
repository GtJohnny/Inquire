using Ganss.Xss;
using Inquire.Data;
using Inquire.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Versioning;
using System.Collections;

namespace Inquire.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public PostsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }



        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int id)
        {

            Post post = db.Posts.Include("Category").Where(p=>p.Id==id).First();
                                         

            post.Categ = GetAllCategories();

            if ((post.UserId == _userManager.GetUserId(User)) ||  User.IsInRole("Admin"))
            {
                return View(post);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unei postari care nu va apartine";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Show", "Post", id);
            }
        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult New()
        {
            Post post = new Post();
            post.Categ = GetAllCategories();
            return View(post);
        }

        [HttpPost]
        [Authorize(Roles ="User,Admin")]
        public IActionResult New(Post post)
        {
            var sanitizer = new HtmlSanitizer();

            post.CreatedDate = DateTime.Now;
            post.UpdatedDate = DateTime.Now;
            post.Likes = 0;
            post.UserId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                post.Description = sanitizer.Sanitize(post.Description);
                db.Posts.Add(post);
                db.SaveChanges();
                TempData["message"] = "Articolul a fost adaugat";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                post.Categ = GetAllCategories();
                return View(post);
            }
            
        }

        public IActionResult IndexByCateg(int? categoryId)
        {

            var posts = db.Posts.Where(a => a.CategoryId == categoryId).Include("Category").Include("User")
;


            ViewBag.CategoryName = db.Categories.Find(categoryId).CategoryName;

            ViewBag.CategoryId = categoryId;


            var search = "";

            if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
            {
                search = Convert.ToString(HttpContext.Request.Query["search"]).Trim();  


                List<int> postIds = db.Posts.Where
                                        (
                                         p => p.Title.Contains(search)
                                         || p.Description.Contains(search)
                                        ).Select(p => p.Id).ToList();

                List<int> postsCommIds = db.Commentaries
                                        .Where
                                        (
                                         c => c.Content.Contains(search)
                                        ).Select(c => (int)c.PostId).ToList();

                List<int> mergedIds = postIds.Union(postsCommIds).ToList();


                posts = db.Posts.Where(post => mergedIds.Contains(post.Id))
                                      .Include("Category")
                                      .Include("User");

            }

            ViewBag.SearchString = search;

            var sort = "Created";


            if (Convert.ToString(HttpContext.Request.Query["sort"]) != null)
            {
                sort = Convert.ToString(HttpContext.Request.Query["sort"]).Trim();
            }
            switch (sort)
            {
                case "Likes":
                    posts = posts.OrderByDescending(p => p.Likes);
                    ViewBag.SortString = "Aprecieri";
                    break;
                case "Comments":
                    posts = posts.OrderByDescending(p => p.Comments.Count);
                    ViewBag.SortString = "Comentarii";
                    break;
                case "Alfabetic":
                    posts = posts.OrderBy(p => p.Title);
                    ViewBag.SortString = "Alfabetic";
                    break;
                case "Updated":
                    posts = posts.OrderByDescending(p => p.UpdatedDate);
                    ViewBag.SortString = "Data Update";
                    break;
                default:
                    posts = posts.OrderByDescending(p => p.CreatedDate);
                    ViewBag.SortString = "Data Postare";
                    break;
            }
            ViewBag.Sort= sort;




                //paginare

            int _perPage = 3;
            int totalItems = posts.Count(); 
            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);
            var offset = 0;

            if(!currentPage.Equals(0))
            {
                offset = (currentPage -1) * _perPage;
            }

            var paginatedPosts = posts.Skip(offset).Take(_perPage);

            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)_perPage);

            ViewBag.Posts = paginatedPosts;


            if (search !="")
            {
                ViewBag.PaginationBaseUrl = "/Posts/IndexByCateg?sort="+sort+"&search="+search+"&categoryId";
            }
            else
            {
                ViewBag.PaginationBaseUrl = "/Posts/IndexByCateg?sort="+sort+"&categoryId";
            }

            return View();
        }


      
        // [HttpGet] 
        //[Authorize(Roles = "User,Admin")]
        public IActionResult Show(int id)
        {
            Post article = db.Posts.Include("Category")
                                    .Include("Comments")
                                    .Include("User")
                                    .Include("Comments.User")
                                    .Where(post => post.Id == id).First();

            SetAccessRights();
            return View(article);
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public IActionResult Show([FromForm] Comment comment)
        {
            comment.Date = DateTime.Now;
            comment.UserId = _userManager.GetUserId(User);


            if (ModelState.IsValid)
            {
                db.Commentaries.Add(comment);
                db.SaveChanges();
                return Redirect("/Posts/Show/" + comment.PostId);
            }
            else
            {
                Post post = db.Posts.Include("Category")
                                         .Include("User")
                                         .Include("Comments")
                                         .Include("Comments.User")
                                         .Where(p => p.Id == comment.PostId)
                                         .First();

                //return Redirect("/Articles/Show/" + comm.ArticleId);

                // Adaugam bookmark-urile utilizatorului pentru dropdown
              

                SetAccessRights();

                return View(post);
            }
        }





        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            var selectList = new List<SelectListItem>();

            var categories = from categ in db.Categories
                             select categ;


            foreach (var category in categories)
            {
                var listItem = new SelectListItem();
                listItem.Value = category.Id.ToString();
                listItem.Text = category.CategoryName;

                selectList.Add(listItem);

            }
            return selectList;
        }
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Delete(int id)
        {
            Post post = db.Posts.Include("Comments").Where(p => p.Id == id).First();


            if ((post.UserId == _userManager.GetUserId(User))
                    || User.IsInRole("Admin"))
            {
  
                db.Posts.Remove(post);
                db.SaveChanges();
                TempData["message"] = "Postarea a fost ste";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index","Home");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti un articol care nu va apartine";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index", "Home");
            }
        }

        // Se adauga articolul modificat in baza de date
        // Se verifica rolul utilizatorilor care au dreptul sa editeze (Editor si Admin)
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int id, Post requestArticle)
        {
            var sanitizer = new HtmlSanitizer();

            Post post = db.Posts.Find(id);

            if (ModelState.IsValid)
            {
                if ((post.UserId == _userManager.GetUserId(User))
                    || User.IsInRole("Admin"))
                {
                    post.Title = requestArticle.Title;

                    requestArticle.Description = sanitizer.Sanitize(requestArticle.Description);

                    post.Description = requestArticle.Description;

                    post.UpdatedDate = DateTime.Now;
                    post.CategoryId = requestArticle.CategoryId;
                    TempData["message"] = "Postarea a fost modificata";
                    TempData["messageType"] = "alert-success";
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unei postari care nu va apartine";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                requestArticle.Categ = GetAllCategories();
                return View(requestArticle);
            }
        }
        private void SetAccessRights()
        {
            ViewBag.AfisareButoane = false;
            ViewBag.UserCurent = _userManager.GetUserId(User);
            ViewBag.EsteAdmin = User.IsInRole("Admin");
        }
    }
}
