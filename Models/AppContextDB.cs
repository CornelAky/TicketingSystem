using Microsoft.EntityFrameworkCore;

namespace TicketingSystem.Models
{
    public class AppContextDB : DbContext
    {
        public AppContextDB(DbContextOptions<AppContextDB> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\AcademyLocalDB;Database=TicketSystemDB;Trusted_Connection=True;");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //// User-Ticket (CreatedBy relation)
            //modelBuilder.Entity<Ticket>()
            //    .HasOne(t => t.CreatorBy)
            //    .WithMany(u => u.CreatedTickets)
            //    .HasForeignKey(t => t.CreatedById)
            //    .OnDelete(DeleteBehavior.Restrict);

            //// User-Ticket (AssignedTo relation)
            //modelBuilder.Entity<Ticket>()
            //    .HasOne(t => t.AssignedTo)
            //    .WithMany(u => u.AssignedTickets)
            //    .HasForeignKey(t => t.AssignedToId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //// Ticket-TicketStatus
            //modelBuilder.Entity<Ticket>()
            //    .HasOne(t => t.Status)
            //    .WithMany(ts => ts.Tickets)
            //    .HasForeignKey(t => t.StatusId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //// Ticket-Comment
            //modelBuilder.Entity<Comment>()
            //    .HasOne(c => c.Ticket)
            //    .WithMany(t => t.Comments)
            //    .HasForeignKey(c => c.TicketId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //// User-Comment
            //modelBuilder.Entity<Comment>()
            //    .HasOne(c => c.User)
            //    .WithMany(u => u.Comments)
            //    .HasForeignKey(c => c.UserId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //// Ticket-Document
            //modelBuilder.Entity<Document>()
            //    .HasOne(d => d.Ticket)
            //    .WithMany(t => t.Documents)
            //    .HasForeignKey(d => d.TicketId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //// User-Notification
            //modelBuilder.Entity<Notification>()
            //    .HasOne(n => n.User)
            //    .WithMany(u => u.Notifications)
            //    .HasForeignKey(n => n.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //// Subscription-User
            //modelBuilder.Entity<Subscription>()
            //    .HasOne(s => s.User)
            //    .WithMany(u => u.Subscriptions)
            //    .HasForeignKey(s => s.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //// Subscription-Plan
            //modelBuilder.Entity<Subscription>()
            //    .HasOne(s => s.Plan)
            //    .WithMany(p => p.Subscriptions)
            //    .HasForeignKey(s => s.PlanId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //// Payment-Subscription
            //modelBuilder.Entity<Payment>()
            //    .HasOne(p => p.Subscription)
            //    .WithMany(s => s.Payments)
            //    .HasForeignKey(p => p.SubscriptionId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
