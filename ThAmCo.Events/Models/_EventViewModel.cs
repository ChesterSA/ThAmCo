using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// The partial event view model
    /// Used for connecting events to Customers and Staff
    /// </summary>
    public class _EventViewModel
    {
        /// <summary>
        /// The id of the event
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The title of the event
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// The date of the event
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// The type id of the event
        /// </summary>
        [Required, MaxLength(3), MinLength(3)]
        public string TypeId { get; set; }
    }
}
