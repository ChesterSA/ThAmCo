using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThAmCo.Events.Data
{
    /// <summary>
    /// The data type used for guest bookings
    /// </summary>
    public class GuestBooking
    {
        /// <summary>
        /// The Id of the customer in the guest booking
        /// </summary>
        [Key, Column(Order = 0)]
        public int CustomerId { get; set; }

        /// <summary>
        /// The Customer in the GuestBooking
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// The Id of the Event
        /// </summary>
        [Key, Column(Order = 1)]
        public int EventId { get; set; }

        /// <summary>
        /// The Event used in the booking
        /// </summary>
        public Event Event { get; set; }

        /// <summary>
        /// A boolean indicating Guest attendance
        /// </summary>
        public bool Attended { get; set; }
    }
}
