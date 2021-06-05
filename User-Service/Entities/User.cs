namespace User_Service.Entities
{
    /// <summary>
    /// Provides various properties for User.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Mail.
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// Gets or sets OtherData.
        /// </summary>
        public string OtherData { get; set; }
    }
}
