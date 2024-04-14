using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EventsReminderApp.MVC.Models
{
    public class EventsReminderAppContext : IdentityDbContext<UserModel>
    {
        public DbSet<Events> Events { get; set; }

        public EventsReminderAppContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
