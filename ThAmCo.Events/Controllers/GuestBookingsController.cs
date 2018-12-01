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
        public async Task<IActionResult> Create([Bind("CustomerId,EventId,Attended")] GuestBooking guestBooking)
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
            if (eventId == null)
            {
                return BadRequest();
            }

            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Title", eventId);

            var currentGuests = _context.Guests
                                        .Where(g => g.EventId == eventId)
                                        .Include(gb => gb.Customer)
                                        .ToList();

            ViewData["CustomerId"] = new SelectList(currentGuests, "CustomerId", "Customer.Email");

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
            var booking = await _context.Guests
                                        .Where(g => g.EventId == guestBooking.EventId)
                                        .Where(g => g.CustomerId == guestBooking.CustomerId)
                                        .FirstOrDefaultAsync();

            _context.Guests.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Events");
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? eventid, int? customerid)
        {
            if (eventid == null || customerid == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests.Where(e => e.EventId == eventid)
                                             .Where(e => e.CustomerId == customerid)
                                             .FirstOrDefaultAsync();

            if (guest == null)
            {
                return NotFound();
            }
            return View(guest);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int eventid, int customerid, [Bind("CustomerId, EventId, Attended")] GuestBooking GuestBooking)
        {
            if (eventid != GuestBooking.EventId || customerid != GuestBooking.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(GuestBooking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GuestExists(GuestBooking.EventId, GuestBooking.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Events");
            }
            return View(GuestBooking);
        }
        private bool GuestExists(int eventid, int customerid)
        {
            return _context.Guests.Where(e => e.EventId == eventid)
                                  .Where(e => e.CustomerId == customerid)
                                  .Count() > 0;
        }
    }
    
}
