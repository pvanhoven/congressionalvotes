using congress.Model;
using Microsoft.EntityFrameworkCore;

namespace congress
{
    public class CongressDataContext : DbContext
    {
        public CongressDataContext(DbContextOptions<CongressDataContext> contextOptions) : base(contextOptions) { }

        public DbSet<Session> Sessions => Set<Session>();

        public DbSet<Senator> Senators => Set<Senator>();

        public DbSet<LegislativeItem> LegislativeItems => Set<LegislativeItem>();

        public DbSet<Model.Vote> Votes => Set<Model.Vote>();
    }
}