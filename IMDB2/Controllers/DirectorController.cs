using IMDB2.Models;
using IMDB2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace IMDB2.Controllers
{
    public class DirectorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DirectorController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public ActionResult Details(int id)
        {
            var director = _context.Directors.FirstOrDefault(d => d.Id == id);
            var movies = _context.Movies.Where(m => m.DirectorId== id).ToList();
            var directorVM = new DirectorDetailsViewModel
            {
                Director = director,
                Movies = movies,
            };
            return View(directorVM);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult NewDirector()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewDirector(Director director)
        {
           
            if (ModelState.IsValid)
            {

                var movie = _context.Movies.ToList().Last();
               
                _context.Directors.ToList().Add(director);
                movie.Director=director;

                _context.SaveChanges();
                return RedirectToAction("AddActors","Actor");
            }
            return View(director);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AddDirector()
        {
            List<SelectListItem> directorList = new List<SelectListItem>();
            foreach (var director in _context.Directors)
            {
                directorList.Add(new SelectListItem { Value = director.Id.ToString(), Text = director.FirstName + "" + director.LastName });
            }
            ViewBag.DirectorList = directorList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDirector(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                var movie = _context.Movies.ToList().Last();
                string selectedActorIds = form["SelectedActorId"];

                var director = _context.Directors.ToList().FirstOrDefault(d => d.Id.ToString().Contains(selectedActorIds));


                movie.DirectorId = director.Id;
                _context.SaveChanges();
                return RedirectToAction("AddActors", "Actor");
            }
            return RedirectToAction(nameof(AddDirector));
        }
        [Authorize(Roles = "Admin")]
        public ActionResult UpdateDirector(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            };
            Director director = _context.Directors.ToList().FirstOrDefault(m => m.Id == id);

            if (director == null)
            {
                return HttpNotFound();
            }
            return View(director);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateDirector(Director director)
        {
            if (ModelState.IsValid)
            {

                Director directorInDb = _context.Directors.ToList().First(d => d.Id == director.Id);
                directorInDb.FirstName = director.FirstName;

                directorInDb.LastName = director.LastName;
                _context.SaveChanges();
                return RedirectToAction("Details", "Director", new {id=director.Id});
            }
            return View(director);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id, int movieId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var director = _context.Directors.ToList().FirstOrDefault(d => d.Id == id);

            var movie = _context.Movies.ToList().FirstOrDefault(m => m.Id == movieId);
            if (director == null)
            {
                return HttpNotFound();
            }
            movie.DirectorId=0;
            _context.SaveChanges();
            return RedirectToAction("Details", "Movies", new { id = movieId });
        }
    }
}