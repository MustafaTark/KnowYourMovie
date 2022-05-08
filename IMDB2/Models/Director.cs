using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IMDB2.Models
{
    public class Director
    {
        public int Id { get; set; }
       
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public virtual ICollection<Person> Users { get; set; }


    }
}