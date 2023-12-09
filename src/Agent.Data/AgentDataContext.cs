using Agent.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Agent.Data {
    public class AgentDataContext : DbContext {
        public DbSet<Friend> Friends { get; set; }
        public DbSet<WorldInstance> WorldInstances { get; set; }

        public string DbPath { get; }
        
        public AgentDataContext() {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "agent.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
    }
}
