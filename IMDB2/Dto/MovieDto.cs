using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMDB2.Dto
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        
        public virtual ICollection<ActorDto> Actors { get; set; }
        public int? DirectorId { get; set; }
      
        public DirectorDto Director { get; set; }

        public bool IsFavorite { get; set; }
    }
}