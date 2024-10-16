using Microsoft.EntityFrameworkCore;
using TicketingSystem.Models;

namespace TicketingSystem.DataAccess
{
    public class AppContextDB : DbContext
    {
        public AppContextDB(DbContextOptions<AppContextDB> option) : base(option)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Data Source=(localdb)\AcademyLocalDB;Initial Catalog=TicketSystemDB;persist security info=True;Integrated Security=SSPI;");
        //}
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
            #region PrimaryKey

            modelBuilder.Entity<User>()
                .HasKey(x => x.UserID);

            modelBuilder.Entity<Ticket>()
                .HasKey(x => x.TicketId);

            modelBuilder.Entity<TicketStatus>()
                .HasKey(x => x.StatusId);

            modelBuilder.Entity<Comment>()
                .HasKey(x => x.CommentId);

            modelBuilder.Entity<Document>()
                .HasKey(x => x.DocumentId);

            modelBuilder.Entity<Notification>()
                .HasKey(x => x.NotificationId);

            modelBuilder.Entity<Plan>()
                .HasKey(x => x.PlanId);

            modelBuilder.Entity<Subscription>()
                .HasKey(x => x.SubscriptionId);

            modelBuilder.Entity<Payment>()
                .HasKey(x => x.PaymentId);



            #endregion
            // User-Ticket (CreatedBy relation)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.CreatorBy)
                .WithMany(u => u.CreatedTickets)
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            // User-Ticket (AssignedTo relation)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.AssignedTo)
                .WithMany(u => u.AssignedTickets)
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ticket-TicketStatus
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Status)
                .WithMany(ts => ts.Tickets)
                .HasForeignKey(t => t.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ticket-Comment
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Ticket)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            // User-Comment
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ticket-Document
            modelBuilder.Entity<Document>()
                .HasOne(d => d.Ticket)
                .WithMany(t => t.Documents)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            // User-Notification
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Subscription-User
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithMany(u => u.Subscriptions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Subscription-Plan
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Plan)
                .WithMany(p => p.Subscriptions)
                .HasForeignKey(s => s.PlanId)
                .OnDelete(DeleteBehavior.Cascade);

            // Payment-Subscription
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Subscription)
                .WithMany(s => s.Payments)
                .HasForeignKey(p => p.SubscriptionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
