using Microsoft.EntityFrameworkCore;

namespace Timeline;
public class TimelineDbContext : DbContext
{
  public TimelineDbContext(DbContextOptions<TimelineDbContext> options)
        : base(options) { }
  public DbSet<Zone> Zone { get; set; }
  public DbSet<Boss> Boss { get; set; }
  public DbSet<BossAbility> BossAbility { get; set; }
  public DbSet<BossStage> BossStage { get; set; }
  public DbSet<BossEvent> BossEvent { get; set; }

  public DbSet<WoWSpec> WoWSpec { get; set; }
  public DbSet<Ability> Ability { get; set; }
}

