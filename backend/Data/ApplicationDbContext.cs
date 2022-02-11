using backend.Models;
using backend.Models.Filters;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Decade> Decades { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Movie_Director> Movie_Directors { get; set; }
        public DbSet<Movie_Genre> Movie_Genres { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Movie_Director>()
                .HasOne(m => m.Movie)
                .WithMany(md => md.Movie_Directors)
                .HasForeignKey(mi => mi.MovieId);

            builder.Entity<Movie_Director>()
                .HasOne(d => d.Director)
                .WithMany(md => md.Movie_Directors)
                .HasForeignKey(di => di.DirectorId);

            builder.Entity<Movie_Genre>()
                .HasOne(m => m.Movie)
                .WithMany(mg => mg.Movie_Genres)
                .HasForeignKey(mi => mi.MovieId);

            builder.Entity<Movie_Genre>()
                .HasOne(g => g.Genre)
                .WithMany(mg => mg.Movie_Genres)
                .HasForeignKey(gi => gi.GenreId);
        }
    }
}
