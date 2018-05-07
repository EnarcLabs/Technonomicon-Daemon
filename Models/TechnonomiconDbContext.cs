using Microsoft.EntityFrameworkCore;

namespace EnarcLabs.Technonomicon.Daemon.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Database context for everything to do with the Technonomicon itself.
    /// </summary>
    public class TechnonomiconDbContext : DbContext
    {
        /// <summary>
        /// The set of all registered users.
        /// </summary>
        public DbSet<User> Users { get; set; }
        /// <summary>
        /// The set of all tags.
        /// </summary>
        public DbSet<Tag> Tags { get; set; }
        /// <summary>
        /// The set of all events.
        /// </summary>
        public DbSet<Event> Events { get; set; }
        /// <summary>
        /// The set of all posts.
        /// </summary>
        public DbSet<Post> Posts { get; set; }

        public TechnonomiconDbContext(DbContextOptions<TechnonomiconDbContext> options) : base(options)
        {

        }
    }
}
