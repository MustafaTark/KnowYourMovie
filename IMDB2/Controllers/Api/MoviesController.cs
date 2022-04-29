using IMDB2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using IMDB2.Dto;
using AutoMapper;
using IMDB2.App_Start;

namespace IMDB2.Controllers.Api
{
    public class MoviesController : ApiController
    {
        private ApplicationDbContext _context;
      
        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }
        //GET /api/customers
        public IEnumerable<MovieDto> GetMovies()
        {
            return _context.Movies
                .Include(m => m.Actors)
                .Include(d=>d.Director)
                .ToList()
                .Select(Mapper.Map<Movie, MovieDto>);

        }
        //GET /api/customers/1
        public IHttpActionResult GetMovie(int id)
        {
            var movie = _context.Movies.SingleOrDefault(x => x.Id == id);
            if (movie == null)
                return NotFound();
            return Ok(Mapper.Map<Movie, MovieDto>(movie));
        }
        //POST /api/cutomers
        [HttpPost]
        public IHttpActionResult CreateMovie(MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var movie = Mapper.Map<MovieDto, Movie>(movieDto);
            _context.Movies.Add(movie);
            _context.SaveChanges();
            movieDto.Id = movie.Id;
            return Created(new Uri(Request.RequestUri + "/" + movie.Id), movieDto);
        }
        //PUT /api/customers/1
        [HttpPut,Route("{id}")]
        public void UpdateMovie(int id, MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            var movieInDb = _context.Movies.SingleOrDefault(c => c.Id == id);
            if (movieDto == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            Mapper.Map<MovieDto, Movie>(movieDto, movieInDb);

            _context.SaveChanges();

        }
        //DELETE /api/customers/1
        [HttpDelete, Route("{id}")]
        public void DeleteMovie(int id)
        {
            var movieInDb = _context.Movies.SingleOrDefault(c => c.Id == id);
            if (movieInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            _context.Movies.Remove(movieInDb);
            _context.SaveChanges();

        }
    }
}