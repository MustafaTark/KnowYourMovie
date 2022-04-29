using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMDB2.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public Person User { get; set; }
        public string Content { get; set; }
    }
}