using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// The model used for the venues index view
    /// </summary>
    public class Venues
    {
        /// <summary>
        /// The code of the venue
        /// </summary>
        [Key, MinLength(5), MaxLength(5)]
        public string Code { get; set; }

        /// <summary>
        /// The name of the venue
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The description of the venue and all its features
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// How many people the venue can hold
        /// </summary>
        [Range(1, Int32.MaxValue)]
        public int Capacity { get; set; }
    }
}
