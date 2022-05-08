using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMDB2.Models
{
    public class PersonDislikes
    {
        public int id { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}