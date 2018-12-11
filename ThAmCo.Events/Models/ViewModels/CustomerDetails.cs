using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// Data type used for the customer details view
    /// </summary>
    public class CustomerDetails
    {
        /// <summary>
        /// The unique id of the customer
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The customer's first names
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// The customer's last name
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// The email address of the customer
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// A list of all events the customer has worked
        /// </summary>
        public IEnumerable<_Event> Events { get; set; }
    }
}
