using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Data
{
    /// <summary>
    /// The Staff data type
    /// </summary>
    public class Staff
    {
        /// <summary>
        /// The unique id of each staff member
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// A string indicating the staff details
        /// </summary>
        [Required]
        public string StaffCode { get; set; }

        /// <summary>
        /// The surname of the staff member
        /// </summary>
        [Required]
        public string Surname { get; set; }

        /// <summary>
        /// The First Name of the staff member
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// A boolean indicating whether the staff member is a first aider
        /// </summary>
        public bool FirstAider { get; set; }

        /// <summary>
        /// A list of all Staffing the staff member is used in
        /// </summary>
        public List<Staffing> Jobs { get; set; }
    }
}
