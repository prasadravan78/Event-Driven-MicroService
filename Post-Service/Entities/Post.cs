namespace Post_Service.Entities
{
    /// <summary>
    /// Provides various properties for Post.
    /// </summary>
    public class Post
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets Content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets UserId.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets User.
        /// </summary>
        public User User { get; set; }
    }
}
