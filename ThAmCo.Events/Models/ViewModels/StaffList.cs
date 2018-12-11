using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    public class StaffList
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string FullName {get; set;}

        [Required]
        public string StaffCode { get; set; }
    }
}
