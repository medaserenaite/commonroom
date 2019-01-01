using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace commonroom.ViewModels {
    public class RegistrationViewModel {
        [Key]
        public int userID { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Required Field")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Required Field")]
        [EmailAddress]
        public string Email { get; set; }


        [Required(ErrorMessage = "Required Field")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [NotMapped]
        [Required(ErrorMessage = "Required Field")]
        [Compare("Password")]        
        [DataType(DataType.Password)]
        public string Confirm { get; set; }
    }
}