using IMDB2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMDB2.ViewModel
{
    public class CreateMovieViewModel
    {
        public Movie Movie { get; set; }
        public HttpPostedFileBase MovieImage { get; set; }
        public Director Director { get; set; }
        public HttpPostedFileBase DirectorImage { get; set; }
    }
}