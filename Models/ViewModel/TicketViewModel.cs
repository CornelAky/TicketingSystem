namespace TicketingSystem.Models.ViewModel
{
    public class TicketViewModel
    {
        public List<TicketingSystem.Models.Ticket> tickets { get; set; } = new List<Ticket>();
        public bool hasCredit { get { return ticketsAvailable > 0 ? true : false;  } }
        public int ticketsAvailable { get; set; } = 0; 
    }
}
