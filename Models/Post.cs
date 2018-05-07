using System;
using System.Collections.Generic;

namespace EnarcLabs.Technonomicon.Daemon.Models
{
    /// <summary>
    /// A stand-alone block of formatted text.
    /// </summary>
    public class Post
    {
        /// <summary>
        /// This post's unique ID.
        /// </summary>
        public Guid PostId { get; set; }

        /// <summary>
        /// The post title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The post body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// The creator of this post.
        /// </summary>
        public User Creator { get; set; }

        /// <summary>
        /// Public list of posts that relate closely to this topic.
        /// </summary>
        public List<Post> RelatedPosts { get; set; }
    }
}
