using Microsoft.EntityFrameworkCore;

namespace WumpWeb.Models
{
    public class ScoreContext : DbContext
    {
        public ScoreContext(DbContextOptions<ScoreContext> options)
                : base(options)
        {
        }

        public DbSet<Score> ScoreTable { get; set; }
    }
}