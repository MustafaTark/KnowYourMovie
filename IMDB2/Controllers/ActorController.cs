using IMDB2.Data;
using IMDB2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace IMDB2.Controllers
{
    public class ActorController : Controller
    {
        private ApplicationDbContext _context;

        public ActorController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AddActors()
        {
            List<SelectListItem> actorList = new List<SelectListItem>();
            foreach (var actor in _context.Actors)
            {
                actorList.Add(new SelectListItem { Value = actor.Id.ToString(), Text = actor.FirstName + "" + actor.LastName });
            }
            ViewBag.ActorList = actorList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public ActionResult AddActors(FormCollection form)
        {


            if (ModelState.IsValid)
            {
                var movie = _context.Movies.ToList().Last();
                string selectedActorIds = form["SelectedActorId"];
                //string[] selectedActorIdsArray = selectedActorIds.Split(',');
                //foreach (string id in selectedActorIdsArray)
                //{
                var actor = _context.Actors.ToList().FirstOrDefault(a => a.Id.ToString().Contains(selectedActorIds));
                movie.Actors.Add(actor);


                _context.SaveChanges();


                return RedirectToAction("AddActors","Actor");
            }


            return RedirectToAction("Index","Movies");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult NewActor()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Route("Movies/AddActors")]
        public ActionResult NewActor(Actor actor)
        {



            if (ModelState.IsValid)
            {

                var movie = _context.Movies.ToList().Last();



                HttpPostedFileBase file = Request.Files["ImageActor"];
                if (file != null)
                {
                    byte[] image = Upload.UploadImageInDataBase(file);
                    actor.Image = image;
                    string ImageName = Path.GetFileName(file.FileName);
                    actor.Img = ImageName;
                    string physicalPath = Server.MapPath(Url.Content("~/Images/") + ImageName);
                    file.SaveAs(physicalPath);
                }


                movie.Actors.Add(actor);

                _context.Actors.ToList().Add(actor);

                _context.SaveChanges();


                return RedirectToAction("NewActor");
            }


            return RedirectToAction(nameof(AddActors));
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
                actorInDb.FirstName = actor.FirstName;
                actorInDb.LastName = actor.LastName;
                actorInDb.Age = actor.Age;
                HttpPostedFileBase file = Request.Files["ImageActor"];
                if (file != null)
                {
                    byte[] image = Upload.UploadImageInDataBase(file);
                    actorInDb.Image = image;
                    string ImageName = Path.GetFileName(file.FileName);
                    actorInDb.Img = ImageName;
                    string physicalPath = Server.MapPath("~/Images" + ImageName);
                    file.SaveAs(physicalPath);
                }
                _context.SaveChanges();
                return RedirectToAction("Details");
            }
            return View(actor);
        }
    }
}