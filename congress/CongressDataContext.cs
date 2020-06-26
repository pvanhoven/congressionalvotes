using congress.Model;
using Microsoft.EntityFrameworkCore;

namespace congress
{
    public class CongressDataContext : DbContext
    {
        public DbSet<Session> Sessions { get; set; }

        public DbSet<Senator> Senators { get; set; }

        public DbSet<LegislativeItem> LegislativeItems { get; set; }

        public DbSet<Model.Vote> Votes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string databaseName = "CongVotes";
            string userName = "testuser";
            string password = "testuser";
            optionsBuilder.UseSqlServer($"Server=localhost;Database={databaseName};User Id={userName};Password={password};");
        }
    }
}