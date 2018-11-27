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
    public class StaffingsController : Controller
    {
        private readonly EventsDbContext _context;

        public StaffingsController(EventsDbContext context)
        {
            _context = context;
        }

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

        // GET: Staffings/Create
        public IActionResult CreateFromStaff([FromQuery] int? staffId)
        {
            if (staffId == null)
            {
                return BadRequest();
            }

            ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "StaffCode", staffId);

            var events = _context.Events.ToList();
            var currentGuests = _context.Workers
                                        .Where(g => g.StaffId == staffId)
                                        .ToList();
            events.RemoveAll(c => currentGuests.Any(g => g.EventId == c.Id));

            ViewData["EventId"] = new SelectList(events, "Id", "Title");

            var staffMember = _context.Staff.Find(staffId);
            if (staffMember == null)
            {
                return BadRequest();
            }

            ViewData["StaffName"] = staffMember.FirstName + " " + staffMember.Surname;

            return View();
        }

        // POST: Staffings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffId,EventId")] Staffing staffing)
        {
            if (ModelState.IsValid)
            {
                //Check if guest booking already exists
                if (_context.Guests.Any(g => g.CustomerId == staffing.StaffId && g.EventId == staffing.EventId))
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

        private bool StaffingExists(int id)
        {
            return _context.Workers.Any(e => e.StaffId == id);
        }
    }
}
