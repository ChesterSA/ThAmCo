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
    /// The controller for the customers data type
    /// </summary>
    public class CustomersController : Controller
    {
        /// <summary>
        /// The dbContext to be used
        /// </summary>
        private readonly EventsDbContext _context;

        /// <summary>
        /// Initialises a new controller
        /// </summary>
        /// <param name="context">The context to be used by the controller</param>
        public CustomersController(EventsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a view containing the list of all customers
        /// </summary>
        /// <returns>The index view of all customers</returns>
        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }

        /// <summary>
        /// Gets the details of one specific Customer
        /// Joins it to the GuestBookings list for that Customer
        /// </summary>
        /// <param name="id">The id of the customer whose details are needed</param>
        /// <returns>A view containing the details of the Customer</returns>
        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                                         .Select(c => new CustomerDetailsViewModel
                                         {
                                            Id = c.Id,
                                            FirstName = c.FirstName,
                                            LastName = c.Surname,
                                            Email = c.Email,
                                            Events = _context.Guests
                                                             .Where(g => g.CustomerId == c.Id)
                                                             .Select(e => new _EventViewModel
                                                             {
                                                                Id = e.Event.Id,
                                                                Date = e.Event.Date,
                                                                Title = e.Event.Title,
                                                                TypeId = e.Event.TypeId
                                                             })
                                         })
                                         .FirstOrDefaultAsync(m => m.Id == id);


            if (customer == null)
            {
                return NotFound();
            }

            ViewData["CustomerName"] = customer.FirstName + " " + customer.LastName;
            return View(customer);
        }

        /// <summary>
        /// Gets the create view for a customer
        /// </summary>
        /// <returns>The Create view for the customer</returns>
        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Adds a new Customer to the dbContext
        /// </summary>
        /// <param name="customer">The Customer object to be added</param>
        /// <returns>The index view if successful, otherwise the Create view</returns>
        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Surname,FirstName,Email")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        /// <summary>
        /// Returns the edit view for a customer data object
        /// Data fields are initialised to match data
        /// </summary>
        /// <param name="id">The id of the customer to edit</param>
        /// <returns>The edit view, fields populated</returns>
        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        /// <summary>
        /// Edits a Customer in the dbContext
        /// </summary>
        /// /// <param name="id">The id of the Customer object to be edited</param>
        /// <param name="customer">The Customer object to be edited</param>
        /// <returns>The index view if successful, otherwise the Edit view</returns>
        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Surname,FirstName,Email")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        /// <summary>
        /// Gets a delete view for a customer
        /// </summary>
        /// <param name="id">The id of the customer to delete</param>
        /// <returns>The details view of the customer to delete</returns>
        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        /// <summary>
        /// Deletes a customer from the db Context
        /// </summary>
        /// <param name="id">The id of the customer to delete</param>
        /// <returns>The index view</returns>
        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Checks if a customer exists
        /// </summary>
        /// <param name="id">The id of the customer to check</param>
        /// <returns>True if the customer exists, false if not</returns>
        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
