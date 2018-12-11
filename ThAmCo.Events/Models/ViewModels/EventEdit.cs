using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// Model used for the Event Edit view
    /// </summary>
    public class EventEdit
    {
        /// <summary>
        /// The Id of the event
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The title of the event
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The Date the event will occur on
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The duration of the event
        /// </summary>
        public TimeSpan? Duration { get; set; }

        /// <summary>
        /// The identifier of the event Type
        /// </summary>
        [MaxLength(3), MinLength(3)]
        public string TypeId { get; set; }

        /// <summary>
        /// A list of bookings for the event
        /// </summary>
        public List<GuestBooking> Bookings { get; set; }
    }
}
