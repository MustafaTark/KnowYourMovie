using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IMDB2.Models
{
    public class Cast
    {
        public int Id { get; set; }
        public int? MovieId { get; set; }
       // [ForeignKey("MovieId")]
        public Movie Movie { get; set; }

        public int? ActorId { get; set; }
       // [ForeignKey("ActorId")]
        public Actor Actor { get; set; }
        public int? DirectorId { get; set; }
       // [ForeignKey("DirectorId")]
        public Director Director { get; set; }
    }
}