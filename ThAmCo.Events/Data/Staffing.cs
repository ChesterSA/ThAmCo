using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Data
{
    /// <summary>
    /// The data type used to connect Events and Staff
    /// </summary>
    public class Staffing
    {
        /// <summary>
        /// The Id of the staff member
        /// </summary>
        [Key, Column(Order = 0)]
        public int StaffId { get; set; }

        /// <summary>
        /// Staff member object of the staffing
        /// </summary>
        public Staff Staff { get; set; }

        /// <summary>
        /// The event id
        /// </summary>
        [Key, Column(Order = 1)]
        public int EventId { get; set; }

        /// <summary>
        /// The event linked to the staffing
        /// </summary>
        public Event Event { get; set; }
    }
}
