using System;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// Data type used when for errors
    /// </summary>
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}