namespace TicketingSystem.Models
{
    public class Ticket
    {

        public long TicketId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long CreatedById { get; set; }
        public User CreatorBy { get; set; }
        public long? AssignedToId { get; set; }
        public User AssignedTo { get; set; }
        public long StatusId { get; set; }
        public TicketStatus Status { get; set; }
        public ePriority Priority { get; set; } // Enum
        public DateTime CreatedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public List<Document> Documents { get; set; }

        public List<Comment> Comments;
        public enum ePriority
        {
            Low=0,
            Medium=1,
            High=2,
        }
    }
}
