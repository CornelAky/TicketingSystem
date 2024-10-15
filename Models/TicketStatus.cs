using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystem.Models
{
    public class TicketStatus
    {
        public long StatusId { get; set; }
        public string StatusName { get; set; }

        [NotMapped]
        public List<Ticket> Tickets { get; set; }
    }
}
