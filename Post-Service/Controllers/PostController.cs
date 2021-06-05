namespace Post_Service.Controllers
{ 
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Post_Service.DbContext;
    using Post_Service.Entities;

    /// <summary>
    /// Provides members to manage Users.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        #region Member Variables

        private readonly AppDbContext appDbContext;

        #endregion Member Variables

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PostController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        #endregion Constructors

        #region Public Methods

        [HttpGet]
        [Route("User")]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await appDbContext.User.ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPost()
        {
            return await appDbContext.Post.Include(x => x.User).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(Post post)
        {
            appDbContext.Post.Add(post);
            await appDbContext.SaveChangesAsync();

            return CreatedAtAction("GetPost", new { id = post.Id }, post);
        }

        #endregion Public Methods
    }
}
