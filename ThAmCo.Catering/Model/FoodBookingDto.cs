using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// The datatype used for transferring bookings between projects
    /// </summary>
    public class FoodBookingDto
    {
        /// <summary>
        ///  The Id of the menu
        /// </summary>
        [Key, Column(Order = 0)]
        public int MenuId { get; set; }

        /// <summary>
        /// The id of the event
        /// </summary>
        [Key, Column(Order = 1)]
        public int EventId { get; set; }
    }
}
