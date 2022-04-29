using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMDB2.Models
{
    public class UpdateMovieViewModel
    {
        public int Id { get; set; }
        public Movie Movie { get; set; }
        public Actor Actor { get; set; }
        public Director Director { get; set; }
        public UpdateMovieViewModel()
        {
            Actor = new Actor();
            
        }
    }
}