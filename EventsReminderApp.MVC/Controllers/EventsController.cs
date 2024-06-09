using EventsReminderApp.MVC.Models;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsReminderApp.MVC.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly EventsReminderAppContext _context;

        public EventsController(EventsReminderAppContext context)
        {
            _context = context;
        }

        public IActionResult AddEvent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEvent(Events events)
        {
            // Walidacja modelu za pomocą FluentValidation
            EventsValidator validator = new EventsValidator();
            ValidationResult results = validator.Validate(events);

            if (results.IsValid)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Events.Add(events);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Events", "Events");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", $"An error occurred while saving the event: {ex.Message}");
                    }
                }
            }
            else
            {
                foreach (var error in results.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }

            // Jeśli walidacja nie powiedzie się, lub istnieją błędy w ModelState, zwróć widok z błędami
            return View(events);
        }

        [HttpGet]
        public async Task<IActionResult> Events()
        {
            List<Events> events = await _context.Events.ToListAsync();
            return View(events);
        }

        [HttpGet]
        public IActionResult AddEventForm()
        {
            return PartialView("_AddEventForm");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.Events.FindAsync(id);

            if (events == null)
            {
                return NotFound();
            }

            return View(events);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Events events)
        {
            if (id != events.Id)
            {
                return NotFound();
            }

            EventsValidator validator = new EventsValidator();
            ValidationResult results = validator.Validate(events);

            if (ModelState.IsValid && results.IsValid)
            {
                try
                {
                    _context.Update(events);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Events));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(events.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                foreach (var error in results.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }

            return View(events);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var events = await _context.Events.FindAsync(id);
            if (events == null)
            {
                return NotFound();
            }

            _context.Events.Remove(events);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Events));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
