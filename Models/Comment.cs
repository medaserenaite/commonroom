using System.ComponentModel.DataAnnotations;
namespace commonroom.Models
{
    public class Comment
    {
        
        [Key]
        public int CommentID { get; set; }
        public string CommentTheme { get; set; }
        public string CommentText { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public int TripID { get; set; }
        public Trip Trip { get; set; }

    }
}