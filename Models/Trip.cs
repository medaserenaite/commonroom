using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace commonroom.Models
{
    public class Trip
    {
        
        [Key]
        public int TripID { get; set; }
        public string TripName { get; set; }
        public string Start { get; set; }
        public string Middle { get; set; }
        public string End { get; set; }
        public int Length { get; set; }
        public string About { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public int CountryID { get; set; }
        public Country Country { get; set; }

        public List<Comment> Comments { get; set; }
        List<SavedTrip> SavedTrips { get; set; }
    }
}