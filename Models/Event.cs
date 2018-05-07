using System;

namespace EnarcLabs.Technonomicon.Daemon.Models
{
    /// <summary>
    /// An event in a story, which chronicles progress through a task.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// This event's unique ID.
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// The post that describes this event.
        /// </summary>
        public Post EventPost { get; set; }

        /// <summary>
        /// The next event in the series.
        /// </summary>
        public Event NextEvent { get; set; }
    }
}
