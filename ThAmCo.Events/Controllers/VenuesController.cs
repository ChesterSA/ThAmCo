
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
    /// <summary>
    /// The controller for the FoodMenus data type
    /// </summary>
    public class VenuesController : Controller
    {
        /// <summary>
        /// The dbContext to be used
        /// </summary>
        private readonly EventsDbContext _context;

        /// <summary>
        /// Initialises the controller and the dbContext
        /// </summary>
        public VenuesController(EventsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a view containing the list of all menus
        /// Pushes a get statement to the Menus api
        /// </summary>
        /// <returns>The index view of all menus</returns>
        // GET: VenuesViewModels
        public async Task<IActionResult> Index(string eventType, DateTime beginDate, DateTime endDate)
        {
            eventType = eventType ?? "WED";

            Debug.WriteLine(endDate);

            DateTime empty = new DateTime();
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

        /// <summary>
        /// Creates a new event for the selected Availalibity
        /// </summary>
        /// <param name="venueCode">the code of the venue to be selected</param>
        /// <param name="eventType">the event type</param>
        /// <param name="date">The date of the event</param>
        /// <returns>The create view initialised with values</returns>
        public IActionResult CreateEvent(string venueCode, string eventType, DateTime date)
        {
            if (venueCode == null || eventType == null)
            {
                return NotFound();
            }

            Debug.WriteLine(date);

            var @event = new VenueEvent
            {
                Venue = venueCode,
                TypeId = eventType,
                Date = date.ToString("yyyy-MM-dd")
            };

            Debug.WriteLine(@event.Date);

            return View(@event);
        }

        /// <summary>
        /// Adds an event to the db context
        /// </summary>
        /// <param name="event">the event to added</param>
        /// <returns></returns>
        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent([Bind("Id,Title,Date,Duration,TypeId,Venue")] VenueEvent @event)
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

        /// <summary>
        /// Connects an HttpClient to a selected port
        /// </summary>
        /// <param name="port">The port number to connect to</param>
        /// <returns>An HttpClient connected to the port</returns>
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
