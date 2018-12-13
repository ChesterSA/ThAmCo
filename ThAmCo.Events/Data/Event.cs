using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ThAmCo.Venues.Data;

namespace ThAmCo.Events.Data
{
    /// <summary>
    /// The data type for each event
    /// </summary>
    public class Event
    {
        /// <summary>
        /// The unique Id of each event
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Checks whether an event is active, used as a soft delete
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The Title of the event
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// The Date of the event
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        /// <summary>
        /// How long the event will take
        /// </summary>
        public TimeSpan? Duration { get; set; }

        /// <summary>
        /// The TypeId of the event
        /// </summary>
        /// //, MaxLength(3), MinLength(3)
        [Required]
        public string TypeId { get; set; }

        /// <summary>
        /// The code of the venue the event is booked at
        /// </summary>
        public string Venue { get; set; }

        /// <summary>
        /// The list of booking on the event
        /// </summary>
        public List<GuestBooking> Bookings { get; set; }

        /// <summary>
        /// The list of staff for the event
        /// </summary>
        public List<Staffing> Staff { get; set; }

        /// <summary>
        /// The Food Menu booked to the event
        /// </summary>
        public string Menu { get; set; }

        /// <summary>
        /// The cost of the menu
        /// </summary>
        public decimal FoodCost { get; set; }

        /// <summary>
        /// The cost of the venue
        /// </summary>
        public decimal VenueCost { get; set; }

    }
}
