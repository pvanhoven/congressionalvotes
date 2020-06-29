using congress.Model;
using Microsoft.EntityFrameworkCore;

namespace congress
{
    public class CongressDataContext : DbContext
    {
        public CongressDataContext(DbContextOptions<CongressDataContext> contextOptions) : base(contextOptions) { }

        public DbSet<Session> Sessions { get; set; }

        public DbSet<Senator> Senators { get; set; }

        public DbSet<LegislativeItem> LegislativeItems { get; set; }

        public DbSet<Model.Vote> Votes { get; set; }
    }
}