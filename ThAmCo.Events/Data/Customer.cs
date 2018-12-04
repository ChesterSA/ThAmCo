using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Data
{
    /// <summary>
    /// Data type used to represent a customer
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// The unique id of the customer
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The surname of the customer
        /// </summary>
        [Required]
        public string Surname { get; set; }

        /// <summary>
        /// The first name of the customer
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// The customer's email address
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// The list of bookings the customer is included in
        /// </summary>
        public List<GuestBooking> Bookings { get; set; }
    }
}
