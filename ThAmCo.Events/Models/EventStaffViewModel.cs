using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// Model used to add staff details to event details view
    /// </summary>
    public class EventStaffViewModel
    {
        /// <summary>
        /// The Unique id of the staff member
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// A code describing the staff details
        /// </summary>
        [Required]
        public string StaffCode { get; set; }

        /// <summary>
        /// The full name of the staff member
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
