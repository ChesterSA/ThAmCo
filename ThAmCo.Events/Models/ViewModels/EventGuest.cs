using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// Model used  for displaying guest details on the event details page
    /// </summary>
    public class EventGuest
    {
        public int EventId { get; set; }

        /// <summary>
        /// The unique id of the guest
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The guest's name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The email address of the guest
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// a boolean indicating whether the guest attended the event
        /// </summary>
        public bool Attended { get; set; }

    }
}
