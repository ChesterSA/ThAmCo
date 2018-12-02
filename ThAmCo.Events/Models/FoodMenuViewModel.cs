using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Events.Models
{
    public class FoodMenuViewModel
    {
        [Required, Key]
        public int Id { get; set; }

        public string Starter { get; set; }

        [Required]
        public string Main { get; set; }

        public string Dessert { get; set; }

        public decimal Cost { get; set; }
    }
}
