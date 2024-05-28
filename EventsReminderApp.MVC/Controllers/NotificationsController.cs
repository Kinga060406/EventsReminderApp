using EventsReminderApp.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EventsReminderApp.MVC.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly EventsReminderAppContext _context;

        public NotificationsController(EventsReminderAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetReminders()
        {
            var now = DateTime.Now;
            var upcomingEvents = _context.Events
                .AsEnumerable()
                .Where(e => e.Date.ToDateTime(e.Time) > now)
                .Select(e => new {
                    e.Name,
                    e.Description,
                    Date = e.Date.ToDateTime(e.Time)
                })
                .ToList();

            Console.WriteLine($"Liczba nadchodzących wydarzeń: {upcomingEvents.Count}");

            return Json(upcomingEvents);
        }
    }
}
