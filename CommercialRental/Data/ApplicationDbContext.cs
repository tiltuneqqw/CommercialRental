using CommercialRental.Data.Models;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CommercialRental.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Advertisment> Advertisments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RequestRent> RequestsRent { get; set; }
        public DbSet<AppFile> Files { get; set; }
        public int Collapsed { get; internal set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Advertisment>().ToTable("Advertisments");
            modelBuilder.Entity<RequestRent>().ToTable("RequestsRent");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<AppFile>().ToTable("Files");
        }
    }
}