using IMDB2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMDB2.Controllers
{
    public class ProfileController : Controller
    {
        private ApplicationDbContext _context;

        public ProfileController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit()
        {
            return RedirectToAction("Index", "Manage");
        }
        public ActionResult FavoriteList(string id)
        {
            var favMovies = _context.Users.FirstOrDefault(u => u.Id.Contains(id));
            return View(favMovies);
        }


    }
}