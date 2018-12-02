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
    [Route("api/[controller]")]
    [ApiController]
    public class FoodMenusController : ControllerBase
    {
        private readonly CateringDbContext _context;

        public FoodMenusController(CateringDbContext context)
        {
            _context = context;
        }

        // GET: api/FoodMenus
        [HttpGet]
        public IEnumerable<FoodMenu> GetMenus()
        {
            return _context.Menus;
        }

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

        private bool FoodMenuExists(int id)
        {
            return _context.Menus.Any(e => e.Id == id);
        }
    }
}