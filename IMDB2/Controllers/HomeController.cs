using IMDB2.Models;
using IMDB2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMDB2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public ActionResult Index()
        {
            var movies = _context.Movies.ToList();
            var actors= _context.Actors.ToList();
            var TopMovies = _context.Movies.Where(t => t.Likes > 3).ToList();
            var person=_context.Persons.ToList().FirstOrDefault(p=>p.UserName.Contains(User.Identity.Name));
            var HomeVM = new HomeViewModel
            {
                Movies = movies,
                Actors = actors,
                TopMovies = TopMovies,
                Person = person,
            };
            return View(HomeVM);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult Search(FormCollection form)
        {
            var searchKey = form["searchKey"];
            var movie = _context.Movies.ToList().FirstOrDefault(m => m.Name.ToLower().Contains(searchKey.ToLower()));
            var actor = _context.Actors.ToList().FirstOrDefault(a => a.FirstName.ToLower().Contains(searchKey.ToLower()) || a.LastName.ToLower().Contains(searchKey.ToLower()));
            var director = _context.Directors.ToList().FirstOrDefault(d => d.FirstName.ToLower().Contains(searchKey.ToLower()) || d.LastName.ToLower().Contains(searchKey.ToLower()));
            if (movie != null)
            {
                return RedirectToAction("Details", "Movies", new { id = movie.Id });
            }
            else if (director != null)
            {
                return RedirectToAction("Details", "Director", new { id = director.Id });
            }
            else if (actor != null)
            {
                return RedirectToAction("Details", "Actor", new { id = actor.Id });
            }
            return View("NOT FOUND");
        }
    }
}