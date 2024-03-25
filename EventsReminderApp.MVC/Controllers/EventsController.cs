using EventsReminderApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

public class EventsController : Controller
{
    private readonly EventsReminderAppContext _context;

    public EventsController(EventsReminderAppContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult AddEventForm()
    {
        return PartialView("_AddEventForm");
    }

    [HttpPost]
    public async Task<IActionResult> AddEvent(Events events)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.Events.Add(events);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while saving the event: {ex.Message}");
            }
        }
        return View("Error");
    }
}
