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
    public class FoodMenusController : Controller
    {
        private readonly EventsDbContext _context;

        public FoodMenusController(EventsDbContext context)
        {
            _context = context;
        }

        // GET: FoodMenuViewModels
        public async Task<IActionResult> Index()
        {
            var availableVenues = new List<FoodMenuViewModel>().AsEnumerable();

            HttpClient client = getClient("32824");

            HttpResponseMessage response = await client.GetAsync("api/FoodMenus");

            if (response.IsSuccessStatusCode)
            {
                availableVenues = await response.Content.ReadAsAsync<IEnumerable<FoodMenuViewModel>>();

                if (availableVenues.Count() == 0)
                {
                    Debug.WriteLine("No available venues");
                }
            }
            else
            {
                Debug.WriteLine("Recieved a bad response from service");
            }

            return View(availableVenues);
        }

        // GET: FoodMenuViewModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HttpClient client = getClient("32824");

            HttpResponseMessage response = await client.GetAsync("api/foodmenus/" + id);
            var menu = await response.Content.ReadAsAsync<FoodMenuViewModel>();
            return View(menu);
        }

        // GET: FoodMenuViewModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FoodMenuViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Starter,Main,Dessert,Cost")] FoodMenuViewModel foodMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = getClient("32824");

                HttpResponseMessage post = await client.PostAsJsonAsync("api/foodmenus", foodMenuViewModel);

                return RedirectToAction(nameof(Index));
            }
            return View(foodMenuViewModel);
        }

        // GET: FoodMenuViewModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HttpClient client = getClient("32824");
            HttpResponseMessage response = await client.GetAsync("api/foodmenus/" + id);

            var foodMenuViewModel = await response.Content.ReadAsAsync<FoodMenuViewModel>();
            if (foodMenuViewModel == null)
            {
                return NotFound();
            }
            return View(foodMenuViewModel);
        }

        // POST: FoodMenuViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Starter,Main,Dessert,Cost")] FoodMenuViewModel foodMenuViewModel)
        {
            if (id != foodMenuViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    HttpClient client = getClient("32824");

                    HttpResponseMessage response = await client.PutAsJsonAsync("api/foodmenus/" + id, foodMenuViewModel);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodMenuViewModelExists(foodMenuViewModel.Id))
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
            return View(foodMenuViewModel);
        }

        // GET: FoodMenuViewModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HttpClient client = getClient("32824");
            HttpResponseMessage response = await client.GetAsync("api/foodmenus/" + id);
            var foodMenuViewModel = await response.Content.ReadAsAsync<FoodMenuViewModel>();

            if (foodMenuViewModel == null)
            {
                return NotFound();
            }

            return View(foodMenuViewModel);
        }

        // POST: FoodMenuViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpClient client = getClient("32824");
            HttpResponseMessage response = await client.DeleteAsync("api/foodmenus/" + id);

            return RedirectToAction(nameof(Index));
        }

        private HttpClient getClient(string port)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri("http://localhost:" + port);
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

            return client;
        }

        private bool FoodMenuViewModelExists(int id)
        {
            return _context.FoodMenuViewModel.Any(e => e.Id == id);
        }
    }
}
