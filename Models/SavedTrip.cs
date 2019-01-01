using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace commonroom.Models
{
    public class SavedTrip
    {
        
        [Key]
        public int SavedTripID { get; set; }
        public string SavedTripName { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public int TripID { get; set; }
        public Trip Trip { get; set; }
        

    }
}