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
    /// The controller for the food menus
    /// Uses Data/FoodMenu
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FoodMenusController : ControllerBase
    {
        /// <summary>
        /// The Database used by the controller
        /// </summary>
        private readonly CateringDbContext _context;

        /// <summary>
        /// Creates a new instance of the controller, initialises database
        /// </summary>
        /// <param name="context"></param>
        public FoodMenusController(CateringDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// gets a list of all the menus in the database
        /// </summary>
        /// <returns>All food menus in the database</returns>
        // GET: api/FoodMenus
        [HttpGet]
        public IEnumerable<FoodMenu> GetMenus()
        {
            return _context.Menus;
        }

        /// <summary>
        /// Gets a specific food menu
        /// </summary>
        /// <param name="id">The id of the menu to find</param>
        /// <returns>The menu with a matching id</returns>
        // GET: api/FoodMenus/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFoodMenu([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var foodMenu = await _context.Menus.FindAsync(id);

            if (foodMenu == null)
            {
                return NotFound();
            }

            return Ok(foodMenu);
        }

        /// <summary>
        /// Updates a FoodMenu in the database
        /// </summary>
        /// <param name="id">The id of the menu to be updated</param>
        /// <param name="foodMenu">The menu object to be put into the database</param>
        /// <returns>An error if the command fails</returns>
        // PUT: api/FoodMenus/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFoodMenu([FromRoute] int id, [FromBody] FoodMenu foodMenu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != foodMenu.Id)
            {
                return BadRequest();
            }

            _context.Entry(foodMenu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodMenuExists(id))
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
        /// Adds a new menu to the database
        /// </summary>
        /// <param name="foodMenu">The menu to be added</param>
        /// <returns>An HTML string detailing the command</returns>
        // POST: api/FoodMenus
        [HttpPost]
        public async Task<IActionResult> PostFoodMenu([FromBody] FoodMenu foodMenu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Menus.Add(foodMenu);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFoodMenu", new { id = foodMenu.Id }, foodMenu);
        }

        /// <summary>
        /// Deletes a food menu from the database
        /// </summary>
        /// <param name="id">The id of the menu to delete</param>
        /// <returns>An html string detailing the success</returns>
        // DELETE: api/FoodMenus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodMenu([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var foodMenu = await _context.Menus.FindAsync(id);
            if (foodMenu == null)
            {
                return NotFound();
            }

            _context.Menus.Remove(foodMenu);
            await _context.SaveChangesAsync();

            return Ok(foodMenu);
        }

        /// <summary>
        /// Checks if a menu already exists
        /// </summary>
        /// <param name="id">The id of the menu to check</param>
        /// <returns>True if the menu exists, false if not</returns>
        private bool FoodMenuExists(int id)
        {
            return _context.Menus.Any(e => e.Id == id);
        }
    }
}