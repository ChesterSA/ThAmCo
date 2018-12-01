using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ThAmCo.Events.Data;
using ThAmCo.Events.Models;
using ThAmCo.Venues.Data;
using ThAmCo.Venues.Models;

namespace ThAmCo.Events.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventsDbContext _context;

        public EventsController(EventsDbContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var eventsDbContext = _context.Events
                                          .Where(e => e.IsActive);


            var events = await eventsDbContext.ToListAsync();

            List<EventList> eventlists = new List<EventList>();

            foreach(Event e in events)
            {
                EventList el = new EventList();
                el.Bookings = e.Bookings;
                el.Date = e.Date;
                el.Duration = e.Duration;
                el.Id = e.Id;
                el.IsActive = e.IsActive;
                el.Title = e.Title;
                el.TypeId = e.TypeId;

                eventlists.Add(el);
            }

            return View(eventlists);
        }



        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                                       .Select(e => new EventDetailsViewModel
                                       {
                                           Id = e.Id,
                                           IsActive = e.IsActive,
                                           Title = e.Title,
                                           Date = e.Date,
                                           Duration = e.Duration,
                                           TypeId = e.TypeId,
                                           Guests = _context.Guests
                                                            .Where(g => g.EventId == e.Id)
                                                            .Select(g => new EventGuestViewModel
                                                            {
                                                                Id = g.Customer.Id,
                                                                FirstName = g.Customer.FirstName,
                                                                Surname = g.Customer.Surname,
                                                                Email = g.Customer.Email
                                                            }),
                                           Staff = _context.Workers
                                                           .Where(w => w.EventId == e.Id)
                                                           .Select(w => new EventStaffViewModel
                                                           {
                                                               Id = w.Staff.Id,
                                                               StaffCode = w.Staff.StaffCode,
                                                               FirstName = w.Staff.FirstName,
                                                               Surname = w.Staff.Surname
                                                           })
                                       })
                                       .FirstOrDefaultAsync(m => m.Id == id);

            var guestList = _context.Guests.Where(v => v.EventId == @event.Id);
            var staffList = _context.Workers.Where(v => v.EventId == @event.Id);

            int guestCount = guestList.Count();
            int staffCount = staffList.Count();
            Debug.WriteLine("guestCount: " + guestCount + "| staffCount: " + staffCount + "| guestCount/10: " + guestCount / 10 + "| CorrectStaff: " + (staffCount > 1 && staffCount >= guestCount / 10));

            @event.CorrectStaff = (staffCount > 0 && staffCount >= (guestCount / 10));

            @event.FirstAider = (staffList.Where(s => s.Staff.FirstAider).Count() > 0);

            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Date,Duration,TypeId")] Event @event)
        {
            if (ModelState.IsValid)
            {
                @event.IsActive = true;
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Event @event = await _context.Events.FindAsync(id);
            if (@event == null || !@event.IsActive)
            {
                return NotFound();
            }
            
            EventEdit eventEdit = new EventEdit
            {
                Id = @event.Id,
                Title = @event.Title,
                Date = @event.Date,
                Duration = @event.Duration,
                TypeId = @event.TypeId
            };
            
            return View(@eventEdit);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Duration")] EventEdit eventEdit)
        {
            if (id != @eventEdit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Event @event = _context.Events.Find(id);

                @event.Title = eventEdit.Title;
                @event.Duration = eventEdit.Duration;

                try
                {
                    @event.IsActive = true;
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(eventEdit);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                                       .Where(e => e.IsActive)
                                       .FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            //@event.IsActive = false;

            _context.Guests.RemoveRange(_context.Guests
                                        .Where(g => g.EventId == id));
            _context.Workers.RemoveRange(_context.Workers
                                        .Where(g => g.EventId == id));

            //DELETE RESERVATION
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AvailableVenues(int? eventid)
        {
            if (eventid == null)
            {
                return NotFound();
            }

            var curEvent = await _context.Events.FindAsync(eventid);

            String eventType = curEvent.TypeId;
            DateTime beginDate = curEvent.Date;
            DateTime endDate = curEvent.Date.Add(curEvent.Duration.Value);

            var availableVenues = new List<AvailableVenuesDto>().AsEnumerable();

            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri("http://localhost:23652");

            String s = "api/Availability?eventType=" + eventType
                       + "&beginDate=" + beginDate.ToString("yyyy/MM/dd")
                       + "&endDate=" + endDate.ToString("yyyy/MM/dd");
            Debug.WriteLine(s);

            HttpResponseMessage response = await client.GetAsync("api/Availability?eventType=" + eventType
                + "&beginDate=" + beginDate.ToString("yyyy/MM/dd")
                + "&endDate=" + endDate.ToString("yyyy/MM/dd"));

            Debug.WriteLine(response.RequestMessage);

            //handle empty venues

            if (response.IsSuccessStatusCode)
            {
                availableVenues = await response.Content.ReadAsAsync<IEnumerable<AvailableVenuesDto>>();

                if(availableVenues.Count() == 0)
                {
                    Debug.WriteLine("No available venues");
                }
            }
            else
            {
                Debug.WriteLine("Recieved a bad response from service");
            }

            ViewData["EventTitle"] = curEvent.Title;
            ViewData["EventId"] = curEvent.Id;

            return View(availableVenues);
        }

        public async Task<IActionResult> ReserveVenue(int? eventid, string venueCode, string staffid)
        {
            if (eventid == null || venueCode == null || staffid == null)
            {
                return BadRequest();
            }
            var @event = await _context.Events.FindAsync(eventid);
            DateTime eventDate = @event.Date;

            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri("http://localhost:23652");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            
            ReservationPostDto reservation = new ReservationPostDto();
            reservation.EventDate = eventDate;
            reservation.StaffId = staffid;
            reservation.VenueCode = venueCode;

            string reference = venueCode + eventDate.ToString("yyyyMMdd");
            HttpResponseMessage delete = await client.DeleteAsync("api/reservations/" + reference);

            HttpResponseMessage post = await client.PostAsJsonAsync("api/reservations", reservation);

            if (post.IsSuccessStatusCode)
            {
                
                HttpResponseMessage getReservation = await client.GetAsync("api/reservations/" + reference);
                var x = await getReservation.Content.ReadAsAsync<ReservationViewModel>();
                return View("Reservation", x);
            }
            else
            {
                return RedirectToAction(nameof(AvailableVenues));
            }

        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
