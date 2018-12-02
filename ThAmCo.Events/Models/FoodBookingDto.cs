using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    public class FoodBookingDto
    {
        [Key, Column(Order = 0)]
        public int MenuId { get; set; }

        [Key, Column(Order = 1)]
        public int EventId { get; set; }
    }
}
