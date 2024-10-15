using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystem.Models
{
    public class Document
    {
        public double DocumentId { get; set; }
        public double TicketId { get; set; }
        public string DocumentName { get; set; }
        public string Base64 { get; set; }
        public string ContentType { get; set; }
        [NotMapped]
        public Ticket Ticket { get; set; }
    }
}
