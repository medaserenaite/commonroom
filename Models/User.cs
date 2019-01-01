using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace commonroom.Models
{
    public class User
    {
        
        [Key]
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Country> Countries { get; set; }
        public List<Trip> Trips { get; set; }
        public List<Comment> Comments { get; set; }
        List<SavedTrip> SavedTrips { get; set; }
    }
}