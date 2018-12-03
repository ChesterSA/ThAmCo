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
            var availableVenues = new List<AvailableVenuesDto>().AsEnumerable();

            HttpClient client = getClient("23652");

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

            ViewData["EventType"] = eventType;

            return View(availableVenues);
        }

        public IActionResult CreateEvent(string venueCode, string eventType, DateTime date)
        {
            if (venueCode == null || eventType == null)
            {
                return NotFound();
            }

            Event @event = new Event
            {
                Venue = venueCode,
                TypeId = eventType,
                Date = date
            };

            return View(@event);
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent([Bind("Id,Title,Date,Duration,TypeId,Venue")] Event @event)
        {
            if (ModelState.IsValid)
            {
                @event.IsActive = true;
                _context.Add(@event);
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
