using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// The model used for the staff details view
    /// </summary>
    public class StaffDetails
    {
        /// <summary>
        /// The unique id of the staff member
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The staffcode describing the staff member
        /// </summary>
        [Required]
        public string StaffCode { get; set; }

        /// <summary>
        /// The surname of the staff member
        /// </summary>
        [Required]
        public string Surname { get; set; }

        /// <summary>
        /// The first name of the staff member
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// A boolean indicating if the staff member is a first aider
        /// </summary>
        public bool FirstAider { get; set; }

        /// <summary>
        /// The list of events the staff member is working
        /// </summary>
        public IEnumerable<_Event> Events { get; set; }
    }
}
