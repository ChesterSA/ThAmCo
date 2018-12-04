using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    public class VenueEventViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Date { get; set; }

        public TimeSpan? Duration { get; set; }

        [MaxLength(3), MinLength(3)]
        public string TypeId { get; set; }

        public string Venue { get; set; }
    }
}
