using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Models
{
    public class ReservationViewModel
    {
        public string Reference { get; set; }

        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        public string VenueCode { get; set; }

        public DateTime WhenMade { get; set; }

        public string StaffId { get; set; }
    }
}
