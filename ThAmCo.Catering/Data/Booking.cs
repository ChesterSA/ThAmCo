using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Catering.Data
{
    public class Booking
    {
        [Key, Column(Order = 0)]
        public int MenuId { get; set; }

        public FoodMenu Menu { get; set; }

        [Key, Column(Order = 1)]
        public int EventId { get; set; }

        public bool Attended { get; set; }
    }
}
