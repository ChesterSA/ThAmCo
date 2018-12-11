using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// Data type used by the Event Details view
    /// </summary>
    public class EventDetails
    {
        /// <summary>
        /// The unique id of the event
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Boolean used to determine if an event is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The title of the event
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// The date of the event
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// How long the event will last
        /// </summary>
        public TimeSpan? Duration { get; set; }

        /// <summary>
        /// The Id defining the event type
        /// </summary>
        [Required, MaxLength(3), MinLength(3)]
        public string TypeId { get; set; }

        /// <summary>
        /// The Venue the event is booked in
        /// </summary>
        public string Venue { get; set; }

        /// <summary>
        /// A boolean indicating if the ratio of 1:10 Staff:Guests is met
        /// </summary>
        public bool CorrectStaff { get; set; }

        /// <summary>
        /// A boolean indicating if any of the staff on the event are first aiders
        /// </summary>
        public bool FirstAider { get; set; }

        /// <summary>
        /// The count of guests on the event
        /// </summary>
        public int GuestCount { get; set;}

        /// <summary>
        /// The booked menu for the event
        /// </summary>
        public string Menu { get; set; }

        //The cost of the menu per person
        public string FoodCost { get; set; }

        /// <summary>
        /// The total cost of the event's food, FoodCost * GuestCount
        /// </summary>
        public string TotalFoodCost { get; set; }

        /// <summary>
        /// The cost of hiring the venue for the duration of the event
        /// </summary>
        public string VenueCost { get; set; }

        /// <summary>
        /// The Total event cost, TotalFoodCost + VenueCost
        /// </summary>
        public string TotalCost { get; set; }

        /// <summary>
        /// List of guest who are assigned to the event
        /// </summary>
        public IEnumerable<EventGuest> Guests { get; set; } 

        /// <summary>
        /// List of staff who are assigned to the event
        /// </summary>
        public IEnumerable<EventStaff> Staff { get; set; }
    }
}
