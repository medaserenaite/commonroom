using System.ComponentModel.DataAnnotations;

namespace commonroom.ViewModels {
    public class TripViewModel {

        [Required]
        public string TripName { get; set; }
        [Required]
        public string Start { get; set; }
        [Required]
        public string Middle { get; set; }
        [Required]
        public string End { get; set; }
        [Required]
        public int Length { get; set; }
        [Required]
        public string Comment { get; set; }

    }
}

