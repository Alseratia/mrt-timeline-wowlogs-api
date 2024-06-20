using System;
using System.Collections.Generic;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext() {}
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options) {}

    public virtual DbSet<Ability> Abilities { get; set; } = null!;

    public virtual DbSet<Boss> Bosses { get; set; } = null!;

    public virtual DbSet<BossAbility> BossAbilities { get; set; } = null!;

    public virtual DbSet<BossEvent> BossEvents { get; set; } = null!;

    public virtual DbSet<BossStage> BossStages { get; set; } = null!;

    public virtual DbSet<WoWSpec> WoWSpecs { get; set; } = null!;

    public virtual DbSet<Zone> Zones { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ability>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Ability_pkey");

            entity.HasOne(d => d.WowSpec).WithMany(p => p.Abilities)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Ability_wowSpecId_fkey");
        });

        modelBuilder.Entity<Boss>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Boss_pkey");

            entity.HasOne(d => d.Zone).WithMany(p => p.Bosses)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Boss_zoneId_fkey");
        });

        modelBuilder.Entity<BossAbility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("BossAbility_pkey");

            entity.Property(e => e.IsDefaultVisible).HasDefaultValueSql("true");

            entity.HasOne(d => d.Boss).WithMany(p => p.BossAbilities)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("BossAbility_bossId_fkey");
        });

        modelBuilder.Entity<BossEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("BossEvent_pkey");

            entity.HasOne(d => d.Ability).WithMany(p => p.BossEvents)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("BossEvent_abilityId_fkey");

            entity.HasOne(d => d.Stage).WithMany(p => p.BossEvents)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("BossEvent_stageId_fkey");
        });

        modelBuilder.Entity<BossStage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("BossStage_pkey");

            entity.HasOne(d => d.Boss).WithMany(p => p.BossStages)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("BossStage_bossId_fkey");
        });

        modelBuilder.Entity<WoWSpec>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("WoWSpec_pkey");
        });

        modelBuilder.Entity<Zone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Zone_pkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
