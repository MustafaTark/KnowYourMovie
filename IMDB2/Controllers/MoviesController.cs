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

namespace IMDB2.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;

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
                    string physicalPath = Server.MapPath("~/Images" + ImageName);
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
                if (Moviefile != null)
                {
                    byte[] image = Upload.UploadImageInDataBase(Moviefile);
                    movieInDb.Image = image;
                    string ImageName = Path.GetFileName(Moviefile.FileName);
                    movieInDb.Img = ImageName;
                    string physicalPath = Server.MapPath("~/Images" + ImageName);
                    Moviefile.SaveAs(physicalPath);
                }
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }
      




        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.




        //    public ActionResult New()
        //    {
        //        //var actors = _context.Actors;
        //        var director = _context.Directors;
        //        var viewModel = new MovieFormViewModel
        //        {
        //            Actors= new List<Actor>(),
        //            Director=director.FirstOrDefault(),
        //        };

        //        return View("MoviesForm", viewModel);
        //    }
        //    //public ActionResult NewActor(int id)
        //    //{
        //    //    var movie = _context.Movies.FirstOrDefault(m => m.Id == id);
        //    //  // var actors = _context.Actors;
        //    //    //var director = _context.Directors;
        //    //    var viewModel = new MovieFormViewModel
        //    //    {
        //    //       Movie=movie,
        //    //       Actors=new List<Actor>(),
        //    //       //Director=movie.Director,

        //    //    };

        //    //    return View("ActorForm", viewModel);
        //    //}
        //    //public ActionResult EditActor(int id)
        //    //{
        //    //    var movie = _context.Movies.FirstOrDefault(c => c.Id == id);


        //    //    if (movie == null)
        //    //        return HttpNotFound();

        //    //    var viewModel = new MovieFormViewModel
        //    //    {
        //    //        Movie = movie,
        //    //        Actors = _context.Actors.Where(x => x.Movies.FirstOrDefault(m => m.Id == id).Id == id).ToList(),
        //    //        Director = _context.Directors.FirstOrDefault(m => m.Id == movie.DirectorId),

        //    //    };

        //    //    return View("ActorForm", viewModel);
        //    //}
        //    [Authorize(Roles = "Admin")]
        //    public ActionResult Edit(int id)
        //    {
        //        var movie = _context.Movies.FirstOrDefault(c => c.Id == id);


        //        if (movie == null)
        //            return HttpNotFound();

        //        var viewModel = new MovieFormViewModel
        //        {
        //            Movie = movie,
        //            Actors = movie.Actors.ToList(),
        //            Director = movie.Director,

        //        };

        //        return View("MoviesForm", movie);
        //    }
        //    [HttpPost]
        ////   [ValidateAntiForgeryToken]
        //    public ActionResult Save(Movie movie, Director director,ICollection<Actor> actors)
        //    {
        //        //if (!ModelState.IsValid)
        //        //{
        //        //    //var viewModel = new MovieFormViewModel
        //        //    //{
        //        //    //    Movie = movie,
        //        //    //    Actors = movie.Actors.ToList(),
        //        //    //    Director = movie.Director,
        //        //    //};

        //        //    return View("MovieForm", movie);
        //        //}
        //        if (movie.Id == 0)
        //        {
        //            _context.Movies.Add(movie);
        //        }
        //        else
        //        {
        //            var movieInDb = _context.Movies.First(m => m.Id == movie.Id);
        //            //var DirectorInDb=_context.Directors.First(m => m.Id == director.Id);

        //            movieInDb.Name = movie.Name;
        //            //DirectorInDb.Name = movie.Director.Name;
        //            //movieInDb.Director.Name= movie.Director.Name;

        //            //movieInDb.Actors = movie.Actors;
        //            for (int i = 0; i < actors.Count(); i++)
        //            {
        //                var value = actors.ToList()[i].Id;
        //                var actorInDb = _context.Actors.FirstOrDefault(a => a.Id ==value );
        //                actorInDb.FirstName = actors.ToList()[i].FirstName;
        //                actorInDb.LastName = actors.ToList()[i].LastName;
        //                actorInDb.Img = actors.ToList()[i].Img;
        //                // movieInDb.Actors.ToList()[i].Image = movie.Actors.ToList()[i].Image;
        //                actorInDb.Age = actors.ToList()[i].Age;
        //            }

        //            //movieInDb.Image = new byte[file.ContentLength];
        //            //file.InputStream.Read(movieInDb.Image, 0, file.ContentLength);
        //            //movieInDb.GenreId = movie.GenreId;
        //            //movieInDb.NumberInStock = movie.NumberInStock;
        //            //movieInDb.ReleaseDate = movie.ReleaseDate;
        //        }

        //        _context.SaveChanges();
        //        return RedirectToAction("Index", "Movies");
        //    }
        public ActionResult Details(int id)
        {
            var movie = _context.Movies.Include(m => m.Actors).Include(m=>m.Director).SingleOrDefault(m => m.Id == id);

            if (movie == null)
                return HttpNotFound();

            return View(movie);

        }
    }
}