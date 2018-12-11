using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;
using ThAmCo.Events.Models;

namespace ThAmCo.Events.Controllers
{
    /// <summary>
    /// The controller for the staffs data type
    /// </summary>
    public class StaffsController : Controller
    {
        /// <summary>
        /// The dbContext to be used
        /// </summary>
        private readonly EventsDbContext _context;

        /// <summary>
        /// Initialises a new controller
        /// </summary>
        /// <param name="context">The context to be used by the controller</param>
        public StaffsController(EventsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a view containing the list of all staffs
        /// </summary>
        /// <returns>The index view of all staffs</returns>
        // GET: Staffs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Staff.ToListAsync());
        }

        /// <summary>
        /// Gets the details of one specific Staff
        /// Joins it to the GuestBookings list for that Staff
        /// </summary>
        /// <param name="id">The id of the staff whose details are needed</param>
        /// <returns>A view containing the details of the Staff</returns>
        // GET: Staffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                                      .Select( s => new StaffDetails
                                      {
                                          Id = s.Id,
                                          FirstName = s.FirstName,
                                          Surname = s.Surname,
                                          StaffCode = s.StaffCode,
                                          Events = _context.Events
                                                           .Where(e => e.Id == s.Id)
                                                           .Select(e => new _Event
                                                           {
                                                               Id = e.Id,
                                                               Date = e.Date,
                                                               Title = e.Title,
                                                               TypeId = e.TypeId
                                                           })
                                      })
                                      .FirstOrDefaultAsync(m => m.Id == id);

            if (staff == null)
            {
                return NotFound();
            }

            ViewData["StaffName"] = staff.FirstName + " " + staff.Surname;
            return View(staff);
        }

        /// <summary>
        /// Gets the create view for a staff
        /// </summary>
        /// <returns>The Create view for the staff</returns>
        // GET: Staffs/Create
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Adds a new Staff to the dbContext
        /// </summary>
        /// <param name="staff">The Staff object to be added</param>
        /// <returns>The index view if successful, otherwise the Create view</returns>
        // POST: Staffs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StaffCode,Surname,FirstName")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                _context.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(staff);
        }

        /// <summary>
        /// Returns the edit view for a staff data object
        /// Data fields are initialised to match data
        /// </summary>
        /// <param name="id">The id of the staff to edit</param>
        /// <returns>The edit view, fields populated</returns>
        // GET: Staffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            return View(staff);
        }

        /// <summary>
        /// Edits a Staff in the dbContext
        /// </summary>
        /// /// <param name="id">The id of the Staff object to be edited</param>
        /// <param name="staff">The Staff object to be edited</param>
        // POST: Staffs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StaffCode,Surname,FirstName")] Staff staff)
        {
            if (id != staff.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staff.Id))
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
            return View(staff);
        }

        /// <summary>
        /// Gets a delete view for a staff
        /// </summary>
        /// <param name="id">The id of the staff to delete</param>
        /// <returns>The details view of the staff to delete</returns>
        // GET: Staffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        /// <summary>
        /// Deletes a staff from the db Context
        /// </summary>
        /// <param name="id">The id of the staff to delete</param>
        /// <returns>The index view</returns>
        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staff = await _context.Staff.FindAsync(id);
            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // <summary>
        /// Checks if a staff exists
        /// </summary>
        /// <param name="id">The id of the staff to check</param>
        /// <returns>True if the staff exists, false if not</returns>
        private bool StaffExists(int id)
        {
            return _context.Staff.Any(e => e.Id == id);
        }
    }
}
