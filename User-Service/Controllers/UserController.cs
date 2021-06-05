namespace User_Service.Controllers
{ 
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using RabbitMQ.Client;
    using User_Service.DbContext;
    using User_Service.Entities;

    /// <summary>
    /// Provides members to manage Users.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Member Variables

        private readonly AppDbContext appDbContext;

        #endregion Member Variables

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UserController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        #endregion Constructors

        #region Public Methods

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await appDbContext.User.ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            appDbContext.Entry(user).State = EntityState.Modified;
            await appDbContext.SaveChangesAsync();

            var integrationEventData = JsonConvert.SerializeObject(new
            {
                id = user.Id,
                newname = user.Name
            });

            PublishToMessageQueue("user.update", integrationEventData);

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            appDbContext.User.Add(user);
            await appDbContext.SaveChangesAsync();

            var integrationEventData = JsonConvert.SerializeObject(new
            {
                id = user.Id,
                name = user.Name
            });

            PublishToMessageQueue("user.add", integrationEventData);

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        #endregion Public Methods

        #region Private Methods

        private void PublishToMessageQueue(string integrationEvent, string eventData)
        {
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var body = Encoding.UTF8.GetBytes(eventData);

            channel.BasicPublish(exchange: "User",
                                 routingKey: integrationEvent,
                                 basicProperties: null,
                                 body: body);
            
            channel.Close();
            connection.Close();
        }

        #endregion Private Methods
    }
}
