// See https://aka.ms/new-console-template for more information
using Azure.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TwitchBot2.Model;


namespace TwitchBot2
{
    public class Progran
    {
        private readonly IConfiguration _configuration;
        private readonly BotContext _context;

        public static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            await new Progran().RunJob();
        }

        private Task RunJob()
        {

            var users = new List<Users>(); //_context.Users.ToList();
            Console.WriteLine("Hello, World! users " + users.Count());

            var config = new Config()
            {
                BotUserName = _configuration["BotUserName"],
                Channelname = _configuration["Channelname"],
                CLIENT_SECRET = _configuration["CLIENT_SECRET"],
                CLIENT_ID = _configuration["CLIENT_ID"],
                ACCESS_TOKEN = _configuration["ACCESS_TOKEN"],
                REFRESH_TOKEN = _configuration["REFRESH_TOKEN"],
            };

            BotClient bot = new BotClient(config);
            Console.ReadLine();

            return Task.CompletedTask;
        }

        public Progran()
        {
            var environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                   .AddJsonFile($"appsettings.{environment}.json", optional: true)
                   .AddUserSecrets<Progran>()
                   .AddEnvironmentVariables();

            _configuration = builder.Build();

            var host = _configuration["DB_Host"];
            var port = _configuration["DB_Port"];
            var db = _configuration["DB_DB"];
            var username = _configuration["DB_Username"];
            var password = _configuration["DB_Password"];

            var con2 = $"Host={host};Port={port};Database={db};Username={username};Password={password};";
            Console.WriteLine("Hello, con2:: " + con2);

            //_context = new BotContext(con2);

        }


    }
}
