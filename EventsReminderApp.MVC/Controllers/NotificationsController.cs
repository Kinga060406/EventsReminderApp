using EventsReminderApp.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

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
            var tenMinutesFromNow = now.AddMinutes(10);

            // Pobierz wszystkie wydarzenia z bazy danych
            var allEvents = _context.Events.ToList();

            // Przefiltruj wydarzenia w pamięci
            var upcomingEvents = allEvents
                .Where(e => e.Date.ToDateTime(e.Time) > now && e.Date.ToDateTime(e.Time) <= tenMinutesFromNow)
                .Select(e => new {
                    e.Name,
                    e.Description,
                    Date = e.Date.ToDateTime(e.Time)
                })
                .ToList();

            Console.WriteLine($"Liczba nadchodzących wydarzeń w ciągu 10 minut: {upcomingEvents.Count}");

            return Json(upcomingEvents);
        }
    }
}

