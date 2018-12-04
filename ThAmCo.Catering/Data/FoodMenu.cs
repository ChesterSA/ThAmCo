using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Catering.Data
{
    /// <summary>
    /// The FoodMenus that can be booked for events
    /// </summary>
    public class FoodMenu
    {
        /// <summary>
        /// The unique id of the menu
        /// </summary>
        [Required, Key]
        public int Id { get; set; }

        /// <summary>
        /// The Starter of the menu, can be null
        /// </summary>
        public string Starter { get; set; }

        /// <summary>
        /// The Main course of the menu, can't be null
        /// </summary>
        [Required]
        public string Main { get; set; }

        /// <summary>
        /// The Dessert of the menu, can be null
        /// </summary>
        public string Dessert { get; set; }

        /// <summary>
        /// The total cost of the menu per person
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// The list of bookings that the menu is used on
        /// </summary>
        public IEnumerable<Booking> Bookings { get; set;}
    }
}
