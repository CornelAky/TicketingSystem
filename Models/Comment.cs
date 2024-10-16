using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystem.Models
{
    public class Comment
    {
        public long CommentId { get; set; }
        public long TicketId { get; set; }
        public long UserId { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedDate { get; set; }
        public User User { get; set; }
        public Ticket Ticket { get; set; }
    }
}
