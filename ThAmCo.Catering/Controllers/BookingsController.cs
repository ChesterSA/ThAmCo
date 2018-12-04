using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Catering.Data;


namespace ThAmCo.Catering.Controllers
{
    /// <summary>
    /// The controller for booking FoodMenus onto events, 
    /// controls the Booking data type
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        /// <summary>
        /// The reference to the database used by the project
        /// </summary>
        private readonly CateringDbContext _context;

        /// <summary>
        /// Creates a new instance of the controller and initialises the DbContext
        /// </summary>
        /// <param name="context"></param>
        public BookingsController(CateringDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a booking to a service asking
        /// </summary>
        /// <returns></returns>
        // GET: api/Bookings
        [HttpGet]
        public IEnumerable<Booking> GetBooking()
        {
            return _context.Booking;
        }

        /// <summary>
        /// Returns a booking filtered by the id
        /// </summary>
        /// <param name="id">The event id of the booking to find</param>
        /// <returns></returns>
        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var booking = await _context.Booking.Where(b => b.EventId == id)
                                                .FirstOrDefaultAsync();


            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        /// <summary>
        /// Updates a booking field currently in the database
        /// </summary>
        /// <param name="id">The id of the booking to be updated</param>
        /// <param name="booking">the booking to be added instead</param>
        /// <returns></returns>
        // PUT: api/Bookings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking([FromRoute] int id, [FromBody] Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != booking.MenuId)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Adds a new booking to the database
        /// </summary>
        /// <param name="booking">the booking to be added</param>
        /// <returns></returns>
        // POST: api/Bookings
        [HttpPost]
        public async Task<IActionResult> PostBooking([FromBody] Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Booking.Add(booking);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BookingExists(booking.MenuId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBooking", new { id = booking.MenuId }, booking);
        }

        /// <summary>
        /// Deletes a booking from the database
        /// </summary>
        /// <param name="id">the id of the event booking to be deleted</param>
        /// <returns></returns>
        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();

            return Ok(booking);
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.MenuId == id);
        }
    }
}