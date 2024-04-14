using EventsReminderApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using EventsReminderApp;
using Microsoft.AspNetCore.Authorization;

namespace EventsReminderApp.MVC.Controllers
{
        [Authorize]
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

