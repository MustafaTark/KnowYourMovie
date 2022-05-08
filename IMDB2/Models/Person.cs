using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMDB2.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Img { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        
        public ApplicationUser User { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
        public virtual ICollection<Actor> Actors { get; set; }
        public virtual ICollection<Director> Directors { get; set; }
        public bool LikeMovie { get; set; }
        public bool DislikeMovie { get; set; }
    }
}