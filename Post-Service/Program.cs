using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Post_Service.DbContext;
using Post_Service.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post_Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ListenForIntegrationEvents();
            CreateHostBuilder(args).Build().Run();
        }

        private static void ListenForIntegrationEvents()
        {
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                                         .UseSqlite(@"Data Source=post.db")
                                         .Options;
                var dbContext = new AppDbContext(contextOptions);

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);

                var data = JObject.Parse(message);
                var type = ea.RoutingKey;

                if (type == "user.add")
                {
                    dbContext.User.Add(new User()
                    {
                        Id = data["id"].Value<int>(),
                        Name = data["name"].Value<string>()
                    });

                    dbContext.SaveChanges();
                }
                else if (type == "user.update")
                {
                    var user = dbContext.User.First(a => a.Id == data["Id"].Value<int>());
                    user.Name = data["newname"].Value<string>();

                    dbContext.SaveChanges();
                }
            };

            channel.BasicConsume(queue: "User.PostService",
                                 autoAck: true,
                                 consumer: consumer);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
