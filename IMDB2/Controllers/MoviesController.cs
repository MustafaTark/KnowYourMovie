using IMDB2.Models;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using IMDB2.ViewModel;
//using System.Web.Script.Serialization;
//using System.Web.Script.Services;
//using System.Web.Services;
using System.Net;
using System.IO;
using IMDB2.Data;
using IMDB2.ViewModel;

namespace IMDB2.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Movies
        [Route("Movies/Index")]
        public ActionResult Index()
        {
            var movie =  _context.Movies.Include(x=>x.Actors).ToList() ;
            if(User.IsInRole("Admin"))
                return View("IndexToAdmin",movie);
            return View("IndexUser",movie);
        }
        [Authorize(Roles ="Admin")]
        
        public ActionResult Create()
        {
            int[] SelectedActorId = new int[] { 1 };
            ViewBag.SelectedActorId = SelectedActorId; 
            List<SelectListItem> actorList = new List<SelectListItem>();
            foreach (var actor in _context.Actors)
            {
                actorList.Add(new SelectListItem { Value = actor.Id.ToString(), Text = actor.FirstName+""+actor.LastName });
            }
            ViewBag.ActorList = actorList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create(Movie movie)
        {
            
            if (ModelState.IsValid)
            {
                HttpPostedFileBase Moviefile = Request.Files["ImageMovie"];
                if (Moviefile != null)
                {
                    byte[] image = Upload.UploadImageInDataBase(Moviefile);
                    movie.Image = image;
                    string ImageName = Path.GetFileName(Moviefile.FileName);
                    movie.Img = ImageName;
                    string physicalPath = Server.MapPath(Url.Content("~/Images/") + ImageName);
                    Moviefile.SaveAs(physicalPath);
                }
               
                _context.Movies.Add(movie);
                _context.SaveChanges();
                return RedirectToAction("AddDirector","Director");
            }
           
            return View(movie);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = _context.Movies.ToList().FirstOrDefault(m => m.Id == id);
            Director director = _context.Directors.ToList().FirstOrDefault(m => m.Id == movie.DirectorId);

            if (movie == null)
            {
                return HttpNotFound();
            }
            var updateMovie = new UpdateMovieViewModel
            {
                Movie = movie,
                Actor = movie.Actors.FirstOrDefault(),
                Director = director,
            };
            return View(movie);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                Movie movieInDb = _context.Movies.ToList().First(m => m.Id == movie.Id);
                movieInDb.Name = movie.Name;
                //movieInDb.Img=movie.Img;
                HttpPostedFileBase Moviefile = Request.Files["ImageMovie"];
                if (Moviefile != null||Moviefile.ContentLength==0)
                {
                    byte[] image = Upload.UploadImageInDataBase(Moviefile);
                    movieInDb.Image = image;
                    
                    try
                    {
                        string ImageName = Path.GetFileName(Moviefile.FileName);
                        
                        string physicalPath = Server.MapPath(Url.Content("~/Images/") + ImageName);
                        Moviefile.SaveAs(physicalPath);
                        movieInDb.Img = ImageName;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);

                    }
                    
                }
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }
        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            var movie = _context.Movies.ToList().FirstOrDefault(m => m.Id == id);
          

            if (movie == null)
            {
                return HttpNotFound();
            }
            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Details(int id)
        {
            var movie = _context.Movies.Include(m => m.Actors).Include(m=>m.Director).FirstOrDefault(m => m.Id == id);
            
            //var actors = _context.Actors.ToList().Where(a => a.Movies.FirstOrDefault(m => m.Id == id).Id == id);
            var director=_context.Directors.ToList().FirstOrDefault(d=>d.Id==movie.DirectorId);
            var comments= new LinkedList<Comment>(_context.Comments.Where(m => m.Movie.Id == id)) ;
            MovieDetailsViewModel movieVM = new MovieDetailsViewModel
            {
                Movie = movie,
                Director = director,
                Actors = movie.Actors,
                Comments = comments,
            };
            if (movie == null)
                return HttpNotFound();
            if(User.IsInRole("Admin"))
                return View("DetailsToAdmin",movieVM);
            return View("DetailsToUser",movieVM);

        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Comment(FormCollection form)
        {
            
            int movieId = int.Parse(form["Id"]);
            var newComment = new Comment
            {
                Content = form["Comment"],
                MovieId = movieId,
                UserName = User.Identity.Name
            };

            _context.Comments.Add(newComment);
            _context.SaveChanges();
            return RedirectToAction("Details", "Movies", new {id=movieId});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Like(FormCollection form)
        {
            int movieId = int.Parse(form["Id"]);
            var movie=_context.Movies.ToList().FirstOrDefault(m=>m.Id==movieId);
            var person=_context.Persons.ToList().FirstOrDefault(p=>p.UserName.Contains(User.Identity.Name));
            var personCheckLike = _context.PersonLikes.FirstOrDefault(p => p.PersonId == person.Id && p.MovieId == movieId);
            var personCheckDislike = _context.PersonDislikes.FirstOrDefault(p => p.PersonId == person.Id && p.MovieId == movieId);
            if (personCheckLike == null)
            {
                var personLikes = new PersonLikes
                {
                    MovieId = movieId,
                    PersonId = person.Id,
                };
                movie.Likes++;
                _context.PersonLikes.Add(personLikes);


                if (personCheckDislike != null)
                {
                    _context.PersonDislikes.Remove(personCheckDislike);
                    movie.Dislikes--;
                }


            }
            else
            {
                var personLikes = _context.PersonLikes.FirstOrDefault(p => p.PersonId == person.Id && p.MovieId == movieId);
                movie.Likes--;
                _context.PersonLikes.Remove(personLikes);
                

            }

            _context.SaveChanges();
            return RedirectToAction("Details", "Movies", new { id = movieId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dislike(FormCollection form)
        {
            int movieId = int.Parse(form["Id"]);
            var movie = _context.Movies.ToList().FirstOrDefault(m => m.Id == movieId);
            var person = _context.Persons.ToList().FirstOrDefault(p => p.UserName.Contains(User.Identity.Name));
            var personCheckDislike = _context.PersonDislikes.FirstOrDefault(p => p.PersonId == person.Id && p.MovieId == movieId);
            var personCheckLike = _context.PersonLikes.FirstOrDefault(p => p.PersonId == person.Id && p.MovieId == movieId);
            if (personCheckDislike == null)
            {
                var personDislikes = new PersonDislikes
                {
                    MovieId = movieId,
                    PersonId = person.Id,
                };
                movie.Dislikes++;
                _context.PersonDislikes.Add(personDislikes);
                if (personCheckLike != null)
                {
                    _context.PersonLikes.Remove(personCheckLike);
                    movie.Likes--;
                }

            }
            else
            {
                var personDislikes = _context.PersonDislikes.FirstOrDefault(p => p.PersonId == person.Id && p.MovieId == movieId);
                movie.Dislikes--;
                _context.PersonDislikes.Remove(personDislikes);
              
            }

            _context.SaveChanges();
            return RedirectToAction("Details", "Movies", new { id = movieId });
        }
    }
}