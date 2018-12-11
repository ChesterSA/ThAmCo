using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    public class VenuesDto
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }


    }
}
