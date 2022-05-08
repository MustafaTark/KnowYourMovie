using IMDB2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        [Route("Profile/Index")]
        public ActionResult Index()
        {
           
            var person = _context.Persons.FirstOrDefault(p => p.UserName.Contains(User.Identity.Name));
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var person = _context.Persons.FirstOrDefault(p => p.Id == id);
            if (person == null)
            {
                return HttpNotFound();
            }
           
            return View(person);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Person person)
        {
            if (ModelState.IsValid)
            {
                var personInDb = _context.Persons.FirstOrDefault(p => p.Id == person.Id);
                personInDb.FirstName = person.FirstName;
                personInDb.LastName = person.LastName;
                HttpPostedFileBase UserImage = Request.Files["ProfileImage"];
                if (UserImage != null)
                {


                    string ImageName = System.IO.Path.GetFileName(UserImage.FileName);
                    personInDb.Img = ImageName;
                    string physicalPath = Server.MapPath(Url.Content("~/Images/") + ImageName);
                    UserImage.SaveAs(physicalPath);
                }
                _context.SaveChanges();
                return View(person);
            }
            return View();
        }

        public ActionResult AddFavoriteMovies()
        {
            var person = _context.Persons.FirstOrDefault(p => p.UserName.Contains(User.Identity.Name));
            List<SelectListItem> moviesList = new List<SelectListItem>();
            foreach (var movie in _context.Movies)
            {
                moviesList.Add(new SelectListItem { Value = movie.Id.ToString(), Text = movie.Name });
            }
            ViewBag.MovieList = moviesList;
            return View(person);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFavoriteMovies(FormCollection form)
        {
            var person = _context.Persons.FirstOrDefault(p => p.UserName.Contains(User.Identity.Name));
            string selectedMovieIds = form["SelectedMovieId"];
            var movie =_context.Movies.ToList().FirstOrDefault(m=>m.Id.ToString().Contains(selectedMovieIds));
            person.Movies.Add(movie);
            _context.SaveChanges();
            return RedirectToAction("AddFavoriteMovies","Profile");
        } 
        public ActionResult AddFavoriteActors()
        {
            var person = _context.Persons.FirstOrDefault(p => p.UserName.Contains(User.Identity.Name));
            List<SelectListItem> actorsList = new List<SelectListItem>();
            foreach (var actor in _context.Actors)
            {
                actorsList.Add(new SelectListItem { Value = actor.Id.ToString(), Text = actor.FirstName + " " + actor.LastName });
            }
            ViewBag.ActorList = actorsList;
            return View(person);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFavoriteActors(FormCollection form)
        {
            var person = _context.Persons.FirstOrDefault(p => p.UserName.Contains(User.Identity.Name));
            string selectedActorIds = form["SelectedActorId"];
            var actor =_context.Actors.ToList().FirstOrDefault(a=>a.Id.ToString().Contains(selectedActorIds));
            person.Actors.Add(actor);
            _context.SaveChanges();
            return RedirectToAction("AddFavoriteActors","Profile");
        }
        public ActionResult AddFavoriteDirectors()
        {
            var person = _context.Persons.FirstOrDefault(p => p.UserName.Contains(User.Identity.Name));
            List<SelectListItem> directorList = new List<SelectListItem>();
            foreach (var director in _context.Directors)
            {
                directorList.Add(new SelectListItem { Value = director.Id.ToString(), Text = director.FirstName + " " + director.LastName });
            }
            ViewBag.DirectorList = directorList;
            return View(person);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFavoriteDirectors(FormCollection form)
        {
            var person = _context.Persons.FirstOrDefault(p => p.UserName.Contains(User.Identity.Name));
            string selectedDirectorIds = form["SelectedDirectorId"];
            var director =_context.Directors.ToList().FirstOrDefault(a=>a.Id.ToString().Contains(selectedDirectorIds));
            person.Directors.Add(director);
            _context.SaveChanges();
            return RedirectToAction("AddFavoriteDirectors","Profile");
        }
        public ActionResult DeleteMovie(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var movie = _context.Movies.ToList().FirstOrDefault(a => a.Id == id);
            var person = _context.Persons.FirstOrDefault(p => p.UserName.Contains(User.Identity.Name));
            //var movie=_context.Movies.ToList().FirstOrDefault(m=>m.Actors.FirstOrDefault(a=>a.Id==id).Id==id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            person.Movies.Remove(movie);
            _context.SaveChanges();
            return RedirectToAction(nameof(AddFavoriteMovies));
        }
        public ActionResult DeleteActor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var actor = _context.Actors.ToList().FirstOrDefault(a => a.Id == id);
            var person = _context.Persons.FirstOrDefault(p => p.UserName.Contains(User.Identity.Name));
            //var movie=_context.Movies.ToList().FirstOrDefault(m=>m.Actors.FirstOrDefault(a=>a.Id==id).Id==id);
            if (actor == null)
            {
                return HttpNotFound();
            }

            person.Actors.Remove(actor);
            _context.SaveChanges();
            return RedirectToAction(nameof(AddFavoriteActors));
        }
        public ActionResult DeleteDirector(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var director = _context.Directors.ToList().FirstOrDefault(a => a.Id == id);
            var person = _context.Persons.FirstOrDefault(p => p.UserName.Contains(User.Identity.Name));
            //var movie=_context.Movies.ToList().FirstOrDefault(m=>m.Actors.FirstOrDefault(a=>a.Id==id).Id==id);
            if (director == null)
            {
                return HttpNotFound();
            }

            person.Directors.Remove(director);
            _context.SaveChanges();
            return RedirectToAction(nameof(AddFavoriteDirectors));
        }
    }
}