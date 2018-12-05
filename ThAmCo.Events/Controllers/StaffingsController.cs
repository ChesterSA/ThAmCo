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
    /// <summary>
    /// The controller for the Staffings data type
    /// </summary>
    public class StaffingsController : Controller
    {
        /// <summary>
        /// The dbContext to be used
        /// </summary>
        private readonly EventsDbContext _context;

        /// <summary>
        /// Initialises a new controller
        /// </summary>
        /// <param name="context">The context to be used by the controller</param>
        public StaffingsController(EventsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new staffing to add to a specified event
        /// </summary>
        /// <param name="eventId">the event id to add to the staffing</param>
        /// <returns>A view containing a list of valid staff to add</returns>
        // GET: Staffings/CreateFromEvent
        public IActionResult CreateFromEvent([FromQuery] int? eventId)
        {
            if (eventId == null)
            {
                return BadRequest();
            }

            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Title", eventId);

            var staff = _context.Staff.ToList();
            var currentStaff = _context.Workers
                                       .Where(g => g.EventId == eventId)
                                       .ToList();
            staff.RemoveAll(s => currentStaff.Any(g => g.StaffId == s.Id));

            ViewData["StaffId"] = new SelectList(staff, "Id", "StaffCode");

            var @event = _context.Events.Find(eventId);
            if (@event == null)
            {
                return BadRequest();
            }

            ViewData["EventTitle"] = @event.Title;

            return View();
        }

        /// <summary>
        /// Creates a staffing assigning an event to a staff
        /// </summary>
        /// <param name="customerId">the id of the staff to create an event</param>
        /// <returns>A view containing a list of valid events to add</returns>
        // GET: Staffings/Create
        public IActionResult CreateFromStaff([FromQuery] int? staffId)
        {
            if (staffId == null)
            {
                return BadRequest();
            }

            ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "StaffCode", staffId);

            var events = _context.Events.ToList();
            var currentStaff = _context.Workers
                                       .Where(w => w.StaffId == staffId)
                                       .ToList();
            events.RemoveAll(e => currentStaff.Any(w => w.EventId == e.Id));

            ViewData["EventId"] = new SelectList(events, "Id", "Title");

            var staffMember = _context.Staff.Find(staffId);
            if (staffMember == null)
            {
                return BadRequest();
            }

            ViewData["StaffName"] = staffMember.FirstName + " " + staffMember.Surname;

            return View();
        }

        /// <summary>
        /// Adds a new Staffing to the dbContext
        /// </summary>
        /// <param name="staffing">The Staffing object to be added</param>
        /// <returns>The index view if successful, otherwise the Create view</returns>
        // POST: Staffings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffId,EventId")] Staffing staffing)
        {
            if (ModelState.IsValid)
            {
                //Check if staffing already exists
                if (_context.Workers.Any(w => w.StaffId == staffing.StaffId && w.EventId == staffing.EventId))
                {
                    ModelState.AddModelError(string.Empty, "Booking already exists");
                }
                else
                {
                    _context.Add(staffing);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Events");
                }
            }
            ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "StaffCode", staffing.StaffId);
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Title", staffing.EventId);

            var @event = await _context.Events.FindAsync(staffing.EventId);
            if (@event == null)
            {
                return BadRequest();
            }

            ViewData["EventTitle"] = @event.Title;

            return View(staffing);
        }

        /// <summary>
        /// Gets a delete view for a staffing
        /// </summary>
        /// <param name="id">The id of the staffing to delete</param>
        /// <returns>The details view of the staffing to delete</returns>
        // GET: Staffings/Delete/5
        public IActionResult Delete([FromQuery] int? eventId)
        {
            if (eventId == null)
            {
                return BadRequest();
            }

            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Title", eventId);

            var currentWorkers = _context.Workers
                                         .Where(g => g.EventId == eventId)
                                         .Include(gb => gb.Staff)
                                         .ToList();

            ViewData["StaffId"] = new SelectList(currentWorkers, "StaffId", "Staff.StaffCode");

            var @event = _context.Events.Find(eventId);
            if (@event == null)
            {
                return BadRequest();
            }

            ViewData["EventTitle"] = @event.Title;

            return View();
        }

        /// <summary>
        /// Deletes a staffing from the db Context
        /// </summary>
        /// <param name="id">The id of the staffing to delete</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("StaffId,EventId")] Staffing staffing)
        {
            var worker = await _context.Workers
                                       .Where(w => w.EventId == staffing.EventId)
                                       .Where(w => w.StaffId == staffing.StaffId)
                                       .FirstOrDefaultAsync();

            _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Events");
        }

        /// <summary>
        /// Checks if a staffing exists
        /// </summary>
        /// <param name="id">The id of the staffing to check</param>
        /// <returns>True if the staffing exists, false if not</returns>
        private bool StaffingExists(int id)
        {
            return _context.Workers.Any(e => e.StaffId == id);
        }
    }
}
