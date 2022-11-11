using Microsoft.EntityFrameworkCore;
using TwitchBot2.Model;

namespace TwitchBot2
{
    public class BotContext : DbContext
    {
        private readonly string _connectionString;
        public DbSet<Users> Users { get; set; }

        public BotContext(string connectionString) : base()
        {
            _connectionString = connectionString;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Users>().HasKey(x => x.Userid);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                base.OnConfiguring(optionsBuilder);
                optionsBuilder.UseNpgsql(_connectionString);
            }
        }

     
    }
}
