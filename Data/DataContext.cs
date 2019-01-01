using Microsoft.EntityFrameworkCore;
using commonroom.Models;

namespace commonroom.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<SavedTrip> SavedTrips { get; set; }
    }
}
