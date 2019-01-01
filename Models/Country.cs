using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace commonroom.Models
{
    public class Country
    {
        
        [Key]
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public List<Trip> Trips { get; set; }

    }
}