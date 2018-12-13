using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;
using ThAmCo.Events.Models.ViewModels;

namespace ThAmCo.Events.Controllers
{
    /// <summary>
    /// The controller for the GuestBookings data type
    /// </summary>
    public class GuestBookingsController : Controller
    {
        /// <summary>
        /// The dbContext to be used
        /// </summary>
        private readonly EventsDbContext _context;

        /// <summary>
        /// Initialises a new controller
        /// </summary>
        /// <param name="context">The context to be used by the controller</param>
        public GuestBookingsController(EventsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new guest booking to add to a specified event
        /// </summary>
        /// <param name="eventId">the event id to add to the booking</param>
        /// <returns>A view containing a list of valid customers to add</returns>
        // GET: GuestBookings/CreateFromEvent
        public IActionResult Create([FromQuery] int? eventId)
        {
            if (eventId == null)
            {
                return BadRequest();
            }

            var customers = _context.Customers.ToList();
            var currentGuests = _context.Guests
                                        .Where(g => g.EventId == eventId)
                                        .ToList();
            customers.RemoveAll(s => currentGuests.Any(g => g.CustomerId == s.Id));

            var guestList = customers.Select(s => new GuestList
            {
                Id = s.Id,
                FullName = s.FirstName + " " + s.Surname,
            });

            var @event = _context.Events.Find(eventId);
            if (@event == null)
            {
                return BadRequest();
            }

            ViewData["EventId"] = eventId;
            ViewData["EventTitle"] = @event.Title;
            ViewData["CustomerId"] = new SelectList(guestList, "Id", "FullName");

            return View();
        }

        

        /// <summary>
        /// Adds a new GuestBooking to the dbContext
        /// </summary>
        /// <param name="guestBooking">The GuestBooking object to be added</param>
        /// <returns>The index view if successful, otherwise the Create view</returns>
        // POST: GuestBookings/Create
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
                    return RedirectToAction("Details", "Events", new { id = guestBooking.EventId });
                }
            }

            var @event = await _context.Events.FindAsync(guestBooking.EventId);
            if (@event == null)
            {
                return BadRequest();
            }

            guestBooking.Customer.Bookings.Add(guestBooking);
            guestBooking.Event.Bookings.Add(guestBooking);

            ViewData["EventId"] = guestBooking.EventId;
            return View(guestBooking);
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
               return RedirectToAction("Details", "Events", new { id = eventid });
            }
            return View(GuestBooking);
        }

        /// <summary>
        /// Gets a delete view for a booking
        /// </summary>
        /// <param name="id">The id of the booking to delete</param>
        /// <returns>The details view of the booking to delete</returns>
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

            var guestList = currentGuests.Select(s => new GuestList
            {
                Id = s.Customer.Id,
                FullName = s.Customer.FirstName + " " + s.Customer.Surname,
            });

            ViewData["CustomerId"] = new SelectList(guestList, "Id", "FullName");

            var @event = _context.Events.Find(eventId);
            if (@event == null)
            {
                return BadRequest();
            }

            ViewData["EventTitle"] = @event.Title;

            return View();
        }

        /// <summary>
        /// Deletes a booking from the db Context
        /// </summary>
        /// <param name="id">The id of the booking to delete</param>
        /// <returns>The index view</returns>
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
            return RedirectToAction("Details", "Events", new { id = guestBooking.EventId });
        }

        /// <summary>
        /// Checks if a Booking exists
        /// </summary>
        /// <param name="id">The id of the booking to check</param>
        /// <returns>True if the booking exists, false if not</returns>
        private bool GuestExists(int eventid, int customerid)
        {
            return _context.Guests.Where(e => e.EventId == eventid)
                                  .Where(e => e.CustomerId == customerid)
                                  .Count() > 0;
        }
    }
    
}
