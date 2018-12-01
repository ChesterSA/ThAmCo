using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    public class EventDetailsViewModel
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public TimeSpan? Duration { get; set; }

        [Required, MaxLength(3), MinLength(3)]
        public string TypeId { get; set; }

        public string Venue { get; set; }

        public bool CorrectStaff { get; set; }

        public bool FirstAider { get; set; }

        //public string Venue { get; set; }

        public IEnumerable<EventGuestViewModel> Guests { get; set; } 

        public IEnumerable<EventStaffViewModel> Staff { get; set; }
    }
}
