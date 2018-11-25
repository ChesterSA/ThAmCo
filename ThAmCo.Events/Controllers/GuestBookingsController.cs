using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Controllers
{
    public class GuestBookingsController : Controller
    {
        private readonly EventsDbContext _context;

        public GuestBookingsController(EventsDbContext context)
        {
            _context = context;
        }


        // GET: GuestBookings/CreateFromEvent
        public IActionResult CreateFromEvent([FromQuery] int? eventId)
        {
            if (eventId == null)
            {
                return BadRequest();
            }

            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Title", eventId);

            var customers = _context.Customers.ToList();
            var currentGuests = _context.Guests
                                        .Where(g => g.EventId == eventId)
                                        .ToList();
            customers.RemoveAll(c => currentGuests.Any(g => g.CustomerId == c.Id));

            ViewData["CustomerId"] = new SelectList(customers, "Id", "Email");

            var @event = _context.Events.Find(eventId);
            if (@event == null)
            {
                return BadRequest();
            }

            ViewData["EventTitle"] = @event.Title;

            return View();
        }

        // GET: GuestBookings/CreateFromCustomer
        public IActionResult CreateFromCustomer([FromQuery] int? customerId)
        {
            if (customerId == null)
            {
                return BadRequest();
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email", customerId);

            var events = _context.Events.ToList();
            var currentGuests = _context.Guests
                                        .Where(g => g.CustomerId == customerId)
                                        .ToList();
            events.RemoveAll(c => currentGuests.Any(g => g.EventId == c.Id));

            ViewData["EventId"] = new SelectList(events, "Id", "Title");

            var customer = _context.Customers.Find(customerId);
            if (customer == null)
            {
                return BadRequest();
            }

            ViewData["CustomerName"] = customer.FirstName + " " + customer.Surname;

            return View();
        }

        // POST: GuestBookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffId,EventId,Attended")] GuestBooking guestBooking)
        {
            if (ModelState.IsValid)
            {
                //Check if guest booking already exists
                if (_context.Guests.Any(g => g.CustomerId == guestBooking.CustomerId && g.EventId == guestBooking.EventId))
                {
                    ModelState.AddModelError(string.Empty, "Booking already exists");
                }
                else
                {
                    _context.Add(guestBooking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Events");
                }
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email", guestBooking.CustomerId);
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Title", guestBooking.EventId);

            var @event = await _context.Events.FindAsync(guestBooking.EventId);
            if (@event == null)
            {
                return BadRequest();
            }

            ViewData["EventTitle"] = @event.Title;

            return View(guestBooking);
        }

        // GET: GuestBookings/Delete/5
        public IActionResult Delete([FromQuery] int? eventId)
        {
            //TODO Make customer list only contain people on movie

            if (eventId == null)
            {
                return BadRequest();
            }

            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Title", eventId);

            var customers = _context.Customers.ToList();

            var currentGuests = _context.Guests
                                        .Where(g => g.EventId == eventId)
                                        .ToList();

            customers.RemoveAll(c => currentGuests.Any(g => g.CustomerId == c.Id));

            ViewData["CustomerId"] = new SelectList(customers, "Id", "Email");

            var @event = _context.Events.Find(eventId);
            if (@event == null)
            {
                return BadRequest();
            }

            ViewData["EventTitle"] = @event.Title;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("CustomerId,EventId,Attended")] GuestBooking guestBooking)
        {
            if (ModelState.IsValid)
            {
                //Check if guest booking already exists
                if (_context.Guests.Any(g => g.CustomerId == guestBooking.CustomerId && g.EventId == guestBooking.EventId))
                {
                    ModelState.AddModelError(string.Empty, "Booking already exists");
                }
                else
                {
                    _context.Remove(guestBooking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Events");
                }
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email", guestBooking.CustomerId);
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Title", guestBooking.EventId);

            var @event = await _context.Events.FindAsync(guestBooking.EventId);
            if (@event == null)
            {
                return BadRequest();
            }

            ViewData["EventTitle"] = @event.Title;

            return View(guestBooking);
        }
    }
}
