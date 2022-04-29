using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IMDB2.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public  string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Img { get; set; }
        public byte[] Image { get; set; }
        //public int? MovieId { get; set; }
        //[ForeignKey("MovieId")]
        //public Movie Movie { get; set; }
        //[InverseProperty("Actor")] // <- Navigation property name in EntityA
        public ICollection<Movie> Movies { get; set; }
        public ICollection<UpdateMovieViewModel> NewMovies { get; set; }
        //public bool IsFavorite { get; set; }
       
        
    }
}