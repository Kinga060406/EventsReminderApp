using Microsoft.EntityFrameworkCore;

namespace EventsReminderApp.MVC.Models
{
    public class EventsReminderAppContext : DbContext
    {
        public DbSet<Events> Events { get; set; }

        public EventsReminderAppContext(DbContextOptions options) : base(options)
        {

        }
    }
}
