using IMDB2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMDB2.ViewModel
{
    public class MovieFormViewModel
    {
        public Movie Movie { get; set; }
        public Actor Actors { get; set; }
        public Director Director { get; set; }
    }
}