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
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Route("Movies/Create")]
        public ActionResult Create(Movie movie)
        {
            
            if (ModelState.IsValid)
            {

                //movieVm.Movie.Actors.Add(movieVm.Actors);
                //movieVm.Movie.Director.Add(movieVm.Director);
                movie.DirectorId = movie.Director.Id;
                HttpPostedFileBase Moviefile = Request.Files["ImageMovie"];
                HttpPostedFileBase Directorfile = Request.Files["ImageDirector"];
                if (Moviefile != null)
                {
                    byte[] image = UploadImageInDataBase(Moviefile);
                    movie.Image = image;
                }
               
                //if (file != null)
                //{

                //    string ImageName = Path.GetFileName(file.FileName);
                //    string physicalPath = Server.MapPath("~/Images" + ImageName);

                //    // save image in folder
                //    file.SaveAs(physicalPath);

                //    //save new record in database

                //    movie.Image = ImageName;


                //}


                _context.Movies.Add(movie);
                
                _context.Directors.Add(movie.Director);
                
                
                _context.SaveChanges();
                
                
                return RedirectToAction(nameof(AddDirector));
            }
           
            return View(movie);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AddDirector()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDirector(Director director)
        {
            if (ModelState.IsValid)
            {
                var movie=_context.Movies.Last();
                movie.DirectorId = director.Id;
                _context.Directors.ToList().Add(director);
                return RedirectToAction(nameof(AddActors));
            }
            return View(director);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult UpdateDirector(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ;
            Director director = _context.Directors.ToList().FirstOrDefault(m => m.Id == id);

            if (director == null)
            {
                return HttpNotFound();
            }
            return View(director);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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
                return RedirectToAction(nameof(Index));
            }
            return View(director);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AddActors()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
       // [Route("Movies/AddActors")]
        public ActionResult AddActors(Actor actor)
        {

            
            if (ModelState.IsValid)
            {
               var movie= _context.Movies.ToList().Last();

                HttpPostedFileBase file = Request.Files["ImageActor"];
                byte[] image = UploadImageInDataBase(file);
                actor.Image = image;
                movie.Actors.Add(actor);
               
                _context.Actors.ToList().Add(actor);
               
                _context.SaveChanges();


                return RedirectToAction(nameof(AddActors));
            }
           

            return View(actor);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = _context.Movies.ToList().FirstOrDefault(m=>m.Id==id);
            Director director=_context.Directors.ToList().FirstOrDefault(m=>m.Id==movie.DirectorId);

            if (movie == null)
            {
                return HttpNotFound();
            }
            var updateMovie = new UpdateMovieViewModel
            {
                Movie = movie,
                Actor=movie.Actors.FirstOrDefault(),
                Director = director,
            };
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                Movie movieInDb=_context.Movies.ToList().First(m=>m.Id==movie.Id);
                movieInDb.Name=movie.Name;
                movieInDb.Img=movie.Img;
                HttpPostedFileBase Moviefile = Request.Files["ImageMovie"];
                if (Moviefile != null)
                {
                    byte[] image = UploadImageInDataBase(Moviefile);
                    movieInDb.Image = image;
                }
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult UpdateActor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var actor = _context.Actors.FirstOrDefault(a => a.Id == id);

            if (actor == null)
            {
                return HttpNotFound();
            }

            return View(actor);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateActor(Actor actor)
        {
            if (ModelState.IsValid)
            {
                var actorInDb = _context.Actors.ToList().First(a => a.Id == actor.Id);
                actorInDb.FirstName=actor.FirstName;
                actorInDb.LastName=actor.LastName;
                actorInDb.Age=actor.Age;
                actorInDb.Img = actor.Img;
                HttpPostedFileBase actorfile = Request.Files["ImageActor"];
                if (actorfile != null)
                {
                    byte[] image = UploadImageInDataBase(actorfile);
                    actorInDb.Image = image;
                }
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }
        [NonAction]
        private byte[] UploadImageInDataBase(HttpPostedFileBase file)
        {
            //file.InputStream.Read(movieInDb.Image, 0, file.ContentLength);
            var stream = file.InputStream;
            var reader = new BinaryReader(stream);
            byte[] imageBytes = reader.ReadBytes(file.ContentLength);
            return imageBytes;
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