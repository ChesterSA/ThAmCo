using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Catering.Data
{
    /// <summary>
    /// The data value to store which menus have been booked to which event
    /// </summary>
    public class Booking
    {
        /// <summary>
        /// The Id of the menu
        /// </summary>
        [Key, Column(Order = 0)]
        public int MenuId { get; set; }

        /// <summary>
        /// The Menu object that has been booked
        /// </summary>
        public FoodMenu Menu { get; set; }

        /// <summary>
        /// The Id of the event that the menu is booked to
        /// </summary>
        [Key, Column(Order = 1)]
        public int EventId { get; set; }
    }
}
