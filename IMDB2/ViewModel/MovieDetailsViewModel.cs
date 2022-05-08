using IMDB2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMDB2.ViewModel
{
    public class MovieDetailsViewModel
    {
        public Movie Movie { get; set; }
        public ICollection<Actor> Actors { get; set; }
        public Director Director { get; set; }
        public LinkedList<Comment> Comments { get; set; }
        public MovieDetailsViewModel()
        {
            Actors = new List<Actor>();
            Movie = new Movie();
            Director = new Director();
            Comments = new LinkedList<Comment>();
        }
        public bool Like { get; set; }
        public bool Dislike { get; set; }
    }
}