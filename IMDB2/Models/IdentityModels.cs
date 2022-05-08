using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IMDB2.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

       
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Cast> Casts { get; set; }
        public DbSet<PersonLikes> PersonLikes { get; set; }
        public DbSet<PersonDislikes> PersonDislikes { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
   
        public static ApplicationDbContext Create()
        {
         DbModelBuilder modelBuilder = new DbModelBuilder();
            modelBuilder.Entity<Movie>()
                .HasMany<Actor>(s => s.Actors)
                .WithMany(c => c.Movies)
                .Map(cs =>
                {
                    cs.MapLeftKey("MovieRefId");
                    cs.MapRightKey("ActorRefId");
                    cs.ToTable("ActorMovie");
                });
            //modelBuilder.Entity<UpdateMovieViewModel>()
            //    .HasMany<Actor>(s => s.Actors)
            //    .WithMany(c => (ICollection<UpdateMovieViewModel>)c.Movies)
            //    .Map(cs =>
            //    {
            //        cs.MapLeftKey("MovieFormRefId");
            //        cs.MapRightKey("ActorFormRefId");
            //        cs.ToTable("ActorMovieForm");
            //    });

            modelBuilder.Entity<Person>()
                .HasMany(s => s.Movies)
                .WithMany(c => c.Users)
                .Map(cs =>
                {
                    cs.MapLeftKey("PersonRefId");
                    cs.MapRightKey("MovieRefId");
                    cs.ToTable("PersonMovie");
                });
            modelBuilder.Entity<Person>()
                .HasMany(s => s.Actors)
                .WithMany(c => c.Users)
                .Map(cs =>
                {
                    cs.MapLeftKey("PersonRefId");
                    cs.MapRightKey("ActorRefId");
                    cs.ToTable("PersonActor");
                });
            modelBuilder.Entity<Person>()
                .HasMany(s => s.Directors)
                .WithMany(c => c.Users)
                .Map(cs =>
                {
                    cs.MapLeftKey("PersonRefId");
                    cs.MapRightKey("DirectorRefId");
                    cs.ToTable("PersonDirector");
                });

            return new ApplicationDbContext();
        }
    }
}