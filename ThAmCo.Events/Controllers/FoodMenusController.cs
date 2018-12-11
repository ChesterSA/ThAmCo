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
    /// <summary>
    /// The controller for the FoodMenus data type
    /// </summary>
    public class FoodMenusController : Controller
    {
        /// <summary>
        /// The dbContext to be used
        /// </summary>
        private readonly EventsDbContext _context;

        /// <summary>
        /// Initialises the controller and the dbContext
        /// </summary>
        public FoodMenusController(EventsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a view containing the list of all menus
        /// Pushes a get statement to the Menus api
        /// </summary>
        /// <returns>The index view of all menus</returns>
        // GET: FoodMenus
        public async Task<IActionResult> Index()
        {
            var availableMenus = new List<FoodMenu>().AsEnumerable();

            HttpClient client = getClient("32824");

            HttpResponseMessage response = await client.GetAsync("api/FoodMenus");

            if (response.IsSuccessStatusCode)
            {
                availableMenus = await response.Content.ReadAsAsync<IEnumerable<FoodMenu>>();

                if (availableMenus.Count() == 0)
                {
                    Debug.WriteLine("No available menus");
                }
            }
            else
            {
                Debug.WriteLine("Recieved a bad response from service");
            }

            return View(availableMenus);
        }

        /// <summary>
        /// Gets the details of one specific Menu
        /// Pushes a request to the Menus api
        /// </summary>
        /// <param name="id">The id of the menu whose details are needed</param>
        /// <returns>A view containing the details of the menu</returns>
        // GET: FoodMenus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HttpClient client = getClient("32824");

            HttpResponseMessage response = await client.GetAsync("api/foodmenus/" + id);
            var menu = await response.Content.ReadAsAsync<FoodMenu>();
            return View(menu);
        }

        /// <summary>
        /// Gets the create view for a menu
        /// </summary>
        /// <returns>The Create view for the menu</returns>
        // GET: FoodMenus/Create
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Adds a new menu to the API details
        /// </summary>
        /// <param name="foodMenu">The Customer object to be added</param>
        /// <returns>The index view if successful, otherwise the Create view</returns>
        // POST: FoodMenus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Starter,Main,Dessert,Cost")] FoodMenu foodMenu)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = getClient("32824");

                HttpResponseMessage post = await client.PostAsJsonAsync("api/foodmenus", foodMenu);

                return RedirectToAction(nameof(Index));
            }
            return View(foodMenu);
        }

        /// <summary>
        /// Returns the edit view for a menu data object
        /// Data fields are initialised to match data
        /// </summary>
        /// <param name="id">The id of the menu to edit</param>
        /// <returns>The edit view, fields populated</returns>
        // GET: FoodMens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HttpClient client = getClient("32824");
            HttpResponseMessage response = await client.GetAsync("api/foodmenus/" + id);

            var foodMenu = await response.Content.ReadAsAsync<FoodMenu>();
            if (foodMenu == null)
            {
                return NotFound();
            }
            return View(foodMenu);
        }

        /// <summary>
        /// Edits a Menu in the dbContext
        /// </summary>
        /// /// <param name="id">The id of the Menu object to be edited</param>
        /// <param name="foodMenu">The Menu object to be edited</param>
        /// <returns>The index view if successful, otherwise the Edit view</returns>
        // POST: FoodMenus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Starter,Main,Dessert,Cost")] FoodMenu foodMenu)
        {
            if (id != foodMenu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    HttpClient client = getClient("32824");

                    HttpResponseMessage response = await client.PutAsJsonAsync("api/foodmenus/" + id, foodMenu);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodMenuExists(foodMenu.Id))
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
            return View(foodMenu);
        }

        /// <summary>
        /// Returns the delete view for a menu data object
        /// Data fields are initialised to match data
        /// </summary>
        /// <param name="id">The id of the menu to delete</param>
        /// <returns>The delete view, fields populated</returns>
        // GET: FoodMenus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HttpClient client = getClient("32824");
            HttpResponseMessage response = await client.GetAsync("api/foodmenus/" + id);
            var foodMenu = await response.Content.ReadAsAsync<FoodMenu>();

            if (foodMenu == null)
            {
                return NotFound();
            }

            return View(foodMenu);
        }

        /// <summary>
        /// Deletes a menu from the db Context
        /// </summary>
        /// <param name="id">The id of the menu to delete</param>
        /// <returns>The index view</returns>
        // POST: FoodMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpClient client = getClient("32824");
            HttpResponseMessage response = await client.DeleteAsync("api/foodmenus/" + id);

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Initialises the client connection to the port
        /// </summary>
        /// <param name="port">The port to connect to</param>
        /// <returns>The Client connected to the requested port</returns>
        private HttpClient getClient(string port)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri("http://localhost:" + port);
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

            return client;
        }

        /// <summary>
        /// Checks if a customer exists
        /// </summary>
        /// <param name="id">The id of the customer to check</param>
        /// <returns>True if the customer exists, false if not</returns>
        private bool FoodMenuExists(int id)
        {
            return _context.FoodMenus.Any(e => e.Id == id);
        }
    }
}
