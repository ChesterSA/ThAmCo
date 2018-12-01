using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    public class StaffDetailsViewModel
    {
        public int Id { get; set; }

        [Required]
        public string StaffCode { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string FirstName { get; set; }

        public bool FirstAider { get; set; }

        public IEnumerable<StaffEventViewModel> Events { get; set; }
    }
}
