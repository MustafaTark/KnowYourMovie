using IMDB2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMDB2.ViewModel
{
    public class DirectorDetailsViewModel
    {
        public Director Director { get; set; }
        public List<Movie> Movies { get; set; }
    }
}