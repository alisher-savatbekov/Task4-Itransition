using authTwo.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace authTwo.Context
{
    
    public class EntityConnect:DbContext
    {
        private readonly string _connectionString = "Host=localhost;Database=postgres;Username=postgres;Password=klewer123579;";

        public EntityConnect(DbContextOptions<EntityConnect> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = _connectionString;
            optionsBuilder.UseNpgsql(connectionString);
        }

        public DbSet<User> Users { get; set; }  
    }
}
