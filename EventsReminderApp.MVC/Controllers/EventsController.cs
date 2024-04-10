using EventsReminderApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using EventsReminderApp;
using Microsoft.EntityFrameworkCore;


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

    [HttpPost]
    public async Task<IActionResult> AddEvent(Events events)
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
        return View("Error");
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

        if (ModelState.IsValid)
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
        return View(events);
    }

    private bool EventExists(int id)
    {
        return _context.Events.Any(e => e.Id == id);
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


}
