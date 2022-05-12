using IMDB2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMDB2.ViewModel
{
    public class HomeViewModel
    {
        public List<Movie> Movies { get; set; }
        public List<Actor> Actors { get; set; }
        public List<Movie> TopMovies { get; set; }
        public Person Person { get; set; }

    }
}