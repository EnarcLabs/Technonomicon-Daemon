using System;

namespace EnarcLabs.Technonomicon.Daemon.Models
{
    /// <summary>
    /// Classifies an event or post.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// The unique ID of this tag.
        /// </summary>
        public Guid TagId { get; set; }

        /// <summary>
        /// The short name used to identify this tag.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The post which describes this tag.
        /// </summary>
        public Post TagPost { get; set; }

        /// <summary>
        /// The parent of this tag. Used to create a hierarchy.
        /// </summary>
        public Tag ParentTag { get; set; }
    }
}
