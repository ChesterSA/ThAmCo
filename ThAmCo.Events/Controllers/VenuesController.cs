using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;
using ThAmCo.Events.Models;
using ThAmCo.Venues.Data;

namespace ThAmCo.Events.Controllers
{
    public class VenuesController : Controller
    {
        private readonly EventsDbContext _context;

        public VenuesController(EventsDbContext context)
        {
            _context = context;
        }

        // GET: VenuesViewModels
        public async Task<IActionResult> Index(string eventType, DateTime beginDate, DateTime endDate)
        {
            DateTime empty = new DateTime();
            eventType = eventType ?? "WED";
            beginDate = (beginDate == empty) ? DateTime.MinValue : beginDate;
            endDate = (endDate == empty) ? DateTime.MaxValue : endDate;

            var availableVenues = new List<AvailableVenuesDto>().AsEnumerable();

            HttpClient client = getClient("23652");

            Debug.WriteLine(eventType);
            Debug.WriteLine(beginDate);
            Debug.WriteLine(endDate);

            HttpResponseMessage response = await client.GetAsync("api/Availability?eventType=" + eventType
                + "&beginDate=" + beginDate.ToString("yyyy/MM/dd")
                + "&endDate=" + endDate.ToString("yyyy/MM/dd"));

            if (response.IsSuccessStatusCode)
            {
                availableVenues = await response.Content.ReadAsAsync<IEnumerable<AvailableVenuesDto>>();

                if (availableVenues.Count() == 0)
                {
                    Debug.WriteLine("No available menus");
                }
            }
            else
            {
                Debug.WriteLine("Recieved a bad response from service");
            }

            var eventTypesResponse = await client.GetAsync("api/EventTypes");
            var eventTypes = await eventTypesResponse.Content.ReadAsAsync<IEnumerable<EventType>>();

            ViewData["EventTypes"] = new SelectList(eventTypes, "Id", "Title");
            ViewData["EventType"] = eventType;

            return View(availableVenues);
        }

        public IActionResult CreateEvent(string venueCode, string eventType, DateTime date)
        {
            if (venueCode == null || eventType == null)
            {
                return NotFound();
            }

            Debug.WriteLine(date);

            var @event = new VenueEventViewModel
            {
                Venue = venueCode,
                TypeId = eventType,
                Date = date.ToString("yyyy-MM-dd")
            };

            Debug.WriteLine(@event.Date);

            return View(@event);
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent([Bind("Id,Title,Date,Duration,TypeId,Venue")] VenueEventViewModel @event)
        {

            if (ModelState.IsValid)
            {
                var newEvent = new Event
                {
                    Id = @event.Id,
                    Title = @event.Title,
                    Date = DateTime.Parse(@event.Date),
                    Duration = @event.Duration,
                    TypeId = @event.TypeId,
                    Venue = @event.Venue
                };
                newEvent.IsActive = true;
                _context.Add(newEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "Events");
            }
            return View(@event);
        }

        private HttpClient getClient(string port)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri("http://localhost:" + port);
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

            return client;
        }

        private bool VenuesViewModelExists(string id)
        {
            return _context.VenuesViewModel.Any(e => e.Code == id);
        }
    }
}
