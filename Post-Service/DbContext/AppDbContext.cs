namespace Post_Service.DbContext
{
    using Microsoft.EntityFrameworkCore;
    using Post_Service.Entities;

    /// <summary>
    /// Provides in-memory representation of database as a database context.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Configures various settings related to DB context.
        /// </summary>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// Gets Sets collection of User entity.
        /// </summary>
        public DbSet<User> User { get; set; }

        /// <summary>
        /// Gets Sets collection of Post entity.
        /// </summary>
        public DbSet<Post> Post { get; set; }
    }
}
