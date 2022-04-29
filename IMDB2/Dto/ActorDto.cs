using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMDB2.Dto
{
    public class ActorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Img { get; set; }
        //public int? MovieId { get; set; }
        //[ForeignKey("MovieId")]
        //public Movie Movie { get; set; }
        //[InverseProperty("Actor")] // <- Navigation property name in EntityA
        public virtual ICollection<MovieDto> Movies { get; set; }
        public bool IsFavorite { get; set; }
    }
}