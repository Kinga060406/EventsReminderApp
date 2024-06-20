using EventsReminderApp.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace EventsReminderApp.MVC.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly EventsReminderAppContext _context;
        private readonly UserManager<UserModel> userManager;

        public NotificationsController(EventsReminderAppContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetReminders()
        {
            var now = DateTime.Now;
            var tenMinutesFromNow = now.AddMinutes(10);
            var user = await userManager.GetUserAsync(User);

            // Pobierz wszystkie wydarzenia z bazy danych
            var allEvents = _context.Events.ToList();

            // Przefiltruj wydarzenia w pamięci
            var upcomingEvents = allEvents
                .Where(e => e.Date.ToDateTime(e.Time) > now && e.Date.ToDateTime(e.Time) <= tenMinutesFromNow && e.Author == user)
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

