using EventsReminderApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EventsReminderApp.MVC.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly EventsReminderAppContext _context;
        private readonly UserManager<UserModel> userManager;

        public CalendarController(EventsReminderAppContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public async Task<ActionResult> Calendar()
        {
            var user = await userManager.GetUserAsync(User);

            var calendar = new Calendar(DateTime.Now);

            // Pobierz wydarzenia z bazy danych dla aktualnie zalogowanego użytkownika
            var events = _context.Events.Where(e => e.Author == user).ToList();

            ViewBag.Events = events;

            return View(calendar);
        }

        public async Task<ActionResult> PreviousMonth(int year, int month)
        {
            var user = await userManager.GetUserAsync(User);

            DateTime currentDate = new DateTime(year, month, 1).AddMonths(-1);
            var calendar = new Calendar(currentDate);

            // Pobierz wydarzenia z bazy danych dla aktualnie zalogowanego użytkownika
            var events = _context.Events.Where(e => e.Author == user).ToList();

            ViewBag.Events = events;

            return View("Calendar", calendar);
        }

        public async Task<ActionResult> NextMonth(int year, int month)
        {
            var user = await userManager.GetUserAsync(User);

            DateTime currentDate = new DateTime(year, month, 1).AddMonths(1);
            var calendar = new Calendar(currentDate);

            // Pobierz wydarzenia z bazy danych dla aktualnie zalogowanego użytkownika
            var events = _context.Events.Where(e => e.Author == user).ToList();

            ViewBag.Events = events;

            return View("Calendar", calendar);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var user = await userManager.GetUserAsync(User);

            var eventToEdit = _context.Events.FirstOrDefault(e => e.Id == id && e.Author == user);
            if (eventToEdit == null)
            {
                return NotFound();
            }

            return View(eventToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Events eventToEdit)
        {
            var user = await userManager.GetUserAsync(User);

            if (eventToEdit.Author != user)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                _context.Update(eventToEdit);
                _context.SaveChanges();
                return RedirectToAction("Calendar");
            }
            return View(eventToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await userManager.GetUserAsync(User);

            var eventToDelete = _context.Events.FirstOrDefault(e => e.Id == id && e.Author == user);
            if (eventToDelete != null)
            {
                _context.Events.Remove(eventToDelete);
                _context.SaveChanges();
            }
            return RedirectToAction("Calendar");
        }
    }
}

