using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// Model used in the event index view
    /// </summary>
    public class EventList
    {
        /// <summary>
        /// The id of the event
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// A boolean indicating whether the event is active, used for soft delete functionality
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The Title of the event
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// The date the event will occur on
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// The duration of the event
        /// </summary>
        public TimeSpan? Duration { get; set; }

        public int GuestCount { get; set; }

        public string Venue { get; set; }

        ///// <summary>
        ///// An identifier describing the event type
        ///// </summary>
        //[Required, MaxLength(3), MinLength(3)]
        //public string TypeId { get; set; }

        ///// <summary>
        ///// The bookings linked to this event
        ///// </summary>
        //public List<GuestBooking> Bookings { get; set; }

        ///// <summary>
        ///// The staffings linked to this event
        ///// </summary>
        //public List<Staffing> Staff { get; set; }
    }
}
