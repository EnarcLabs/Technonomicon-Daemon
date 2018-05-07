using System;

namespace EnarcLabs.Technonomicon.Daemon.Models
{
    /// <summary>
    /// Represents a member of the Technonomicon.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The unique ID of the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The user's unique username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The email associated with this user.
        /// </summary>
        public string Email { get; set; }
    }
}
