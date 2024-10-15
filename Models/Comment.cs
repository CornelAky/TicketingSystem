using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystem.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedDate { get; set; }
        [NotMapped]
        public User User { get; set; }
        [NotMapped]
        public Ticket Ticket { get; set; }
    }
}
