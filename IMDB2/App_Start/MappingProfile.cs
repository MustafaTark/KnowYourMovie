using AutoMapper;
using AutoMapper.Configuration;
using IMDB2.Dto;
using IMDB2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMDB2.App_Start
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Actor, ActorDto>(); ;
            Mapper.CreateMap<ActorDto, Actor>(); ;
            Mapper.CreateMap<Director, DirectorDto>(); ;
            Mapper.CreateMap<DirectorDto, Director>();
            Mapper.CreateMap<Movie, MovieDto>();
            Mapper.CreateMap<MovieDto, Movie>();

        }
        
        


        

    }
}