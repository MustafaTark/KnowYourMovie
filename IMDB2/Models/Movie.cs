using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IMDB2.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public byte[] Image { get; set; }

  
        //public int? ActorId { get; set; }
        //[ForeignKey("ActorId")]
        //public  Actor Actor { get; set; }
        //[InverseProperty("Movie")] // <- Navigation property name in EntityA
        public virtual ICollection<Actor> Actors { get; set; }
        public virtual ICollection<Person> Users { get; set; }
        public int? DirectorId { get; set; }
        [ForeignKey("DirectorId")]
        public Director Director { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }

        //public bool IsFavorite { get; set; }
    }
}