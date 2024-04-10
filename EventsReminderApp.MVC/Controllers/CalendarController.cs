using EventsReminderApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using EventsReminderApp;

namespace EventsReminderApp.MVC.Controllers
{
        public class CalendarController : Controller
        {
            public ActionResult Calendar()
            {
                Calendar calendar = new Calendar(DateTime.Now);
                return View(calendar);
            }

            public ActionResult PreviousMonth(int year, int month)
            {
                DateTime currentDate = new DateTime(year, month, 1).AddMonths(-1);
                Calendar calendar = new Calendar(currentDate);
                return View("Calendar", calendar);
            }

            public ActionResult NextMonth(int year, int month)
            {
                DateTime currentDate = new DateTime(year, month, 1).AddMonths(1);
                Calendar calendar = new Calendar(currentDate);
                return View("Calendar", calendar);
            }
        }
 }

