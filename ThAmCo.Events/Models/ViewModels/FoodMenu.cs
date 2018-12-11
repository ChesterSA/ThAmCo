using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// The view model used to show menus in their index
    /// </summary>
    public class FoodMenu
    {
        /// <summary>
        /// The unique id of the event
        /// </summary>
        [Required, Key]
        public int Id { get; set; }

        /// <summary>
        /// The first course of the meal
        /// </summary>
        public string Starter { get; set; }

        /// <summary>
        /// The main course of the meal
        /// </summary>
        [Required]
        public string Main { get; set; }

        /// <summary>
        /// The dessert of the meal 
        /// </summary>
        public string Dessert { get; set; }

        /// <summary>
        /// The cost per head of the meal
        /// </summary>
        public decimal Cost { get; set; }
    }
}
