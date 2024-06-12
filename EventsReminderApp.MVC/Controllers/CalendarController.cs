using EventsReminderApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using EventsReminderApp;
using Microsoft.AspNetCore.Authorization;

namespace EventsReminderApp.MVC.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly EventsReminderAppContext _context;

        public CalendarController(EventsReminderAppContext context)
        {
            _context = context;
        }

        public ActionResult Calendar()
        {
            var calendar = new Calendar(DateTime.Now);

            // Pobierz wydarzenia z bazy danych
            var events = _context.Events.ToList();

            ViewBag.Events = events;

            return View(calendar);
        }

        public ActionResult PreviousMonth(int year, int month)
        {
            DateTime currentDate = new DateTime(year, month, 1).AddMonths(-1);
            var calendar = new Calendar(currentDate);

            // Pobierz wydarzenia z bazy danych
            var events = _context.Events.ToList();

            ViewBag.Events = events;

            return View("Calendar", calendar);
        }

        public ActionResult NextMonth(int year, int month)
        {
            DateTime currentDate = new DateTime(year, month, 1).AddMonths(1);
            var calendar = new Calendar(currentDate);

            // Pobierz wydarzenia z bazy danych
            var events = _context.Events.ToList();

            ViewBag.Events = events;

            return View("Calendar", calendar);
        }

        public ActionResult Edit(int id)
        {
            var eventToEdit = _context.Events.Find(id);
            if (eventToEdit == null)
            {
                return NotFound();
            }

            return View(eventToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Events eventToEdit)
        {
            if (ModelState.IsValid)
            {
                _context.Update(eventToEdit);
                _context.SaveChanges();
                return RedirectToAction("Calendar");
            }
            return View(eventToEdit);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var eventToDelete = _context.Events.Find(id);
            if (eventToDelete != null)
            {
                _context.Events.Remove(eventToDelete);
                _context.SaveChanges();
            }
            return RedirectToAction("Calendar");
        }

    }
}
