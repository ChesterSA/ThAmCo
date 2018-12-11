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
using ThAmCo.Venues.Models;

namespace ThAmCo.Events.Controllers
{
    /// <summary>
    /// The controller for the events data type
    /// </summary>
    public class EventsController : Controller
    {
        /// <summary>
        /// The dbContext to be used
        /// </summary>
        private readonly EventsDbContext _context;

        /// <summary>
        /// Initialises a new controller
        /// </summary>
        /// <param name="context">The context to be used by the controller</param>
        public EventsController(EventsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a view containing the list of all events
        /// </summary>
        /// <returns>The index view of all events</returns>
        // GET: Events
        public async Task<IActionResult> Index()
        {
            var eventsDbContext = _context.Events
                                          .Where(e => e.IsActive);

            var events = await eventsDbContext.ToListAsync();

            List<EventList> eventlists = new List<EventList>();

            foreach (Event e in events)
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

        /// <summary>
        /// Gets the details of one specific Event
        /// Joins it to the GuestBookings list and Staff List for that Event
        /// </summary>
        /// <param name="id">The id of the event whose details are needed</param>
        /// <returns>A view containing the details of the event</returns>
        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                                       .Select(e => new EventDetails
                                       {
                                           Id = e.Id,
                                           IsActive = e.IsActive,
                                           Title = e.Title,
                                           Date = e.Date,
                                           Duration = e.Duration,
                                           TypeId = e.TypeId,
                                           Venue = e.Venue,
                                           GuestCount = _context.Guests.Where(v => v.EventId == e.Id).Count(),
                                           Menu = e.Menu,
                                           FoodCost = e.FoodCost.ToString("C2"),
                                           VenueCost = e.VenueCost.ToString("C2"),
                                           
                                           Guests = _context.Guests
                                                            .Where(g => g.EventId == e.Id)
                                                            .Select(g => new EventGuest
                                                            {
                                                                Id = g.Customer.Id,
                                                                Name = g.Customer.FirstName + " " + g.Customer.Surname,
                                                                Email = g.Customer.Email,
                                                                Attended = g.Attended
                                                            }),
                                           Staff = _context.Workers
                                                           .Where(w => w.EventId == e.Id)
                                                           .Select(w => new EventStaff
                                                           {
                                                               Id = w.Staff.Id,
                                                               StaffCode = w.Staff.StaffCode,
                                                               Name = w.Staff.FirstName + " " + w.Staff.Surname
                                                           })
                                       })
                                       .FirstOrDefaultAsync(m => m.Id == id);

            var guestList = _context.Guests.Where(v => v.EventId == @event.Id);
            var staffList = _context.Workers.Where(v => v.EventId == @event.Id);

            int guestCount = guestList.Count();
            int staffCount = staffList.Count();

            @event.CorrectStaff = (staffCount > 0 && staffCount >= (guestCount / 10));
            @event.FirstAider = (staffList.Where(s => s.Staff.FirstAider).Count() > 0);

            decimal FoodCost = Convert.ToDecimal(@event.FoodCost.Substring(1));
            decimal VenueCost = Convert.ToDecimal(@event.VenueCost.Substring(1));

            @event.TotalFoodCost = (FoodCost * @event.GuestCount).ToString("C2");
            @event.TotalCost = ((FoodCost * @event.GuestCount) * VenueCost).ToString("C2");

            if (@event == null)
            {
                return NotFound();
            }

            ViewData["EventTitle"] = @event.Title;

            return View(@event);
        }

        /// <summary>
        /// Gets the create view for an event
        /// </summary>
        /// <returns>The Create view for the event</returns>
        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Adds a new Event to the dbContext
        /// </summary>
        /// <param name="@event">The Customer object to be added</param>
        /// <returns>The index view if successful, otherwise the Create view</returns>
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

        /// <summary>
        /// Returns the edit view for an event data object
        /// Data fields are initialised to match data
        /// </summary>
        /// <param name="id">The id of the event to edit</param>
        /// <returns>The edit view, fields populated</returns>
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

        /// <summary>
        /// Edits an event in the dbContext
        /// </summary>
        /// <param name="eventEdit">The Event object to be edited</param>
        /// <returns>The index view if successful, otherwise the Edit view</returns>
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

        /// <summary>
        /// Gets a delete view for an event
        /// </summary>
        /// <param name="id">The id of the event to delete</param>
        /// <returns>The details view of the event to delete</returns>
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

        /// <summary>
        /// Deletes an event from the db Context
        /// </summary>
        /// <param name="id">The id of the event to delete</param>
        /// <returns>The index view</returns>
        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            @event.IsActive = false;

            _context.Guests.RemoveRange(_context.Guests
                                        .Where(g => g.EventId == id));
            _context.Workers.RemoveRange(_context.Workers
                                        .Where(g => g.EventId == id));

            HttpClient client = getClient("23652");

            HttpResponseMessage delete = await client.DeleteAsync("api/reservations/" + @event.Venue + @event.Date.ToString("yyyyMMdd"));


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Gets the list of available venues from the api
        /// </summary>
        /// <param name="eventid">The eventId to check availability for</param>
        /// <returns>A list of AvailabeVenues</returns>
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

            HttpClient client = getClient("23652");

            HttpResponseMessage response = await client.GetAsync("api/Availability?eventType=" + eventType
                + "&beginDate=" + beginDate.ToString("yyyy/MM/dd")
                + "&endDate=" + endDate.ToString("yyyy/MM/dd"));

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


            var staff = await _context.Staff.Select(s => new StaffList
            {
                Id = s.Id,
                FullName = s.FirstName + " " + s.Surname,
                StaffCode = s.StaffCode
            }).ToListAsync();

            ViewData["VenueList"] = new SelectList(availableVenues, "Code", "Name");
            ViewData["StaffList"] = new SelectList(staff, "StaffCode", "FullName");
            ViewData["EventTitle"] = curEvent.Title;
            ViewData["EventId"] = curEvent.Id;

            return View(availableVenues);
        }

        /// <summary>
        /// Posts a new Reservation to ThAmCo.Venues
        /// </summary>
        /// <param name="eventId">the event id to be added</param>
        /// <param name="venueCode">The code of the venue to be reserved</param>
        /// <param name="staffid">the id of the staff member linked to the reservation</param>
        /// <returns>A view displaying the reservation details</returns>
        public async Task<IActionResult> ReserveVenue(int? eventId, string venueCode, string staffId)
        {
            if (eventId == null || venueCode == null || staffId == null)
            {
                return BadRequest();
            }

            HttpClient client = getClient("23652");

            var @event = await _context.Events.FindAsync(eventId);

            HttpResponseMessage getVenueName = await client.GetAsync("api/Venues/Details/" + venueCode);
            var a = await getVenueName.Content.ReadAsAsync<VenuesDto>();
            @event.Venue = a.Name;

            HttpResponseMessage getAvailability = await client.GetAsync("api/Availability?eventType=" + @event.TypeId
                + "&beginDate=" + @event.Date.ToString("yyyy/MM/dd")
                + "&endDate=" + @event.Date.ToString("yyyy/MM/dd"));

            var availability = await getAvailability.Content.ReadAsAsync<IEnumerable<AvailableVenuesDto>>();
            decimal venueCost = (decimal)availability.FirstOrDefault().CostPerHour;
            @event.VenueCost = venueCost * @event.Duration.Value.Hours;

            _context.Update(@event);
            await _context.SaveChangesAsync();

            DateTime eventDate = @event.Date;

            ReservationPostDto reservation = new ReservationPostDto();
            reservation.EventDate = eventDate;
            reservation.StaffId = staffId;
            reservation.VenueCode = venueCode;

            
            string reference = venueCode + eventDate.ToString("yyyyMMdd");
            HttpResponseMessage delete = await client.DeleteAsync("api/reservations/" + reference);
            HttpResponseMessage post = await client.PostAsJsonAsync("api/reservations", reservation);

            if (post.IsSuccessStatusCode)
            {
                
                HttpResponseMessage getReservation = await client.GetAsync("api/reservations/" + reference);
                var x = await getReservation.Content.ReadAsAsync<Reservation>();
                return View("Reservation", x);
            }
            else
            {
                return RedirectToAction(nameof(AvailableVenues), eventId);
            }

        }

        /// <summary>
        /// Gets the list of food menus for an event
        /// </summary>
        /// <param name="eventid">the event id to use for reserving a venue</param>
        /// <returns></returns>
        public async Task<IActionResult> AvailableMenus(int? eventid)
        {
            if (eventid == null)
            {
                return BadRequest();
            }

            var availableVenues = new List<FoodMenu>().AsEnumerable();

            HttpClient client = getClient("32824");

            HttpResponseMessage response = await client.GetAsync("api/FoodMenus");

            if (response.IsSuccessStatusCode)
            {
                availableVenues = await response.Content.ReadAsAsync<IEnumerable<FoodMenu>>();

                if (availableVenues.Count() == 0)
                {
                    Debug.WriteLine("No available venues");
                }
            }
            else
            {
                Debug.WriteLine("Recieved a bad response from service");
            }

            ViewData["EventId"] = eventid;

            return View(availableVenues);
        }

        /// <summary>
        /// Creates a new Booking in ThAmCo.Catering
        /// </summary>
        /// <param name="eventid">the id of the event</param>
        /// <param name="menuid">the id of the menu</param>
        /// <returns></returns>
        public async Task<IActionResult> BookMenu(int? eventid, int? menuid)
        {
            if (eventid == null || menuid == null)
            {
                return BadRequest();
            }
            var @event = await _context.Events.FindAsync(eventid);

            HttpClient client = getClient("32824");

            HttpResponseMessage response = await client.GetAsync("api/FoodMenus/" + menuid);
            FoodMenu menu = await response.Content.ReadAsAsync<FoodMenu>();

            @event.Menu = menu.Starter + " | " + menu.Main + " | " + menu.Dessert;
            @event.FoodCost = menu.Cost;

            _context.Update(@event);
            await _context.SaveChangesAsync();

            FoodBookingDto booking = new FoodBookingDto();
            booking.EventId = (int)eventid;
            booking.MenuId = (int)menuid;

            HttpResponseMessage delete = await client.DeleteAsync("api/bookings/" + eventid);

            HttpResponseMessage post = await client.PostAsJsonAsync("api/bookings", booking);

            if (post.IsSuccessStatusCode)
            {
                HttpResponseMessage getBooking = await client.GetAsync("api/foodmenus/" + menuid);
                var x = await getBooking.Content.ReadAsAsync<FoodMenu>();
                return View("BookMenu", x);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

        }

        /// <summary>
        /// Deletes a booking from ThAmCo.Catering
        /// </summary>
        /// <param name="eventid">the id of the menu to delete</param>
        /// <returns></returns>
        public async Task<IActionResult> CancelMenu(int? eventid)
        {
            if (eventid == null)
            {
                return BadRequest();
            }

            var @event =  await _context.Events.FindAsync(eventid);
            @event.FoodCost = 0;
            @event.Menu = null;

            _context.Update(@event);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = eventid });
        }

        private HttpClient getClient(string port)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri("http://localhost:" + port);
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

            return client;
        }

        /// <summary>
        /// Checks if an event exists
        /// </summary>
        /// <param name="id">The id of the event to check</param>
        /// <returns>True if the customer event exists, false if not</returns>
        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
