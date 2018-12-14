using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// Model used for creating an event from a venue
    /// </summary>
    public class VenueEvent
    {
        /// <summary>
        /// The unique id of the event
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The title of the event
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// The date of the event
        /// Handled as a string as passing a date to the view seemed impossible
        /// Tyrone has tried to figure this out and seriously there's no other solution
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// How long the event will last
        /// </summary>
        [Required]
        public TimeSpan? Duration { get; set; }

        /// <summary>
        /// The Type id of the event
        /// </summary>
        [MaxLength(3), MinLength(3)]
        public string TypeId { get; set; }

        /// <summary>
        /// The venue code of the venue the event is booked on
        /// </summary>
        public string Venue { get; set; }
    }
}
