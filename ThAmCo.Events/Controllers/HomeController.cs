using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ThAmCo.Events.Models;

namespace ThAmCo.Events.Controllers
{
    /// <summary>
    /// The Home controller, default webpage for ThAmCo.Events
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// The index view
        /// </summary>
        /// <returns>The homepage of the site</returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}
