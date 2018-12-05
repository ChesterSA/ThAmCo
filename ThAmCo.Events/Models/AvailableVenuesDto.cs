using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// The data type used to transfer data to ThAmCo.Venues
    /// </summary>
    public class AvailableVenuesDto
    {
        /// <summary>
        /// The Venue Code, unique identifier
        /// </summary>
        [Key, MinLength(5), MaxLength(5)]
        public string Code { get; set; }

        /// <summary>
        /// The name of the venue
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Short summary of venue's features
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Hoe many people the venue can hold
        /// </summary>
        [Range(1, Int32.MaxValue)]
        public int Capacity { get; set; }

        /// <summary>
        /// The date the event will occur on
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        /// <summary>
        /// The cost of hiring the venue per hour
        /// </summary>
        [Range(0.0, Double.MaxValue)]
        public double CostPerHour { get; set; }

        public string StaffCode { get; set; }
    }
}
