using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// The view model used to display reservation details
    /// </summary>
    public class Reservation
    {
        /// <summary>
        /// The reference string of the reservation
        /// venueCode + yyyyMMdd
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// The date of the event and reservation
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        /// <summary>
        /// The identifying code of the venue being booked
        /// </summary>
        public string VenueCode { get; set; }

        /// <summary>
        /// The date the reservation was made on
        /// </summary>
        public DateTime WhenMade { get; set; }

        /// <summary>
        /// I don't actually know what this should represent
        /// </summary>
        public string StaffId { get; set; }
    }
}
