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
     
        public ICollection<Movie> Movies { get; set; }
   
        public virtual ICollection<Person> Users { get; set; }
        //public bool IsFavorite { get; set; }


    }
}