using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using melee_tracker_capstone.Data.Entities;

namespace melee_tracker_capstone.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Character> Characters { get; set; }

    public virtual DbSet<Opponent> Opponents { get; set; }

    public virtual DbSet<PlayerProfile> PlayerProfiles { get; set; }

    public virtual DbSet<Set> Sets { get; set; }

    public virtual DbSet<Tournament> Tournaments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<SourceType>("source_type");

        modelBuilder.HasPostgresEnum<ResultType>("result_type");

        modelBuilder.HasPostgresEnum<BracketType>("bracket_type_enum");

        modelBuilder
            .HasPostgresEnum("bracket_type_enum", new[] { "winners", "losers", "grand_finals" })
            .HasPostgresEnum("result_type", new[] { "win", "loss" })
            .HasPostgresEnum("source_type", new[] { "manual", "startgg" });

        modelBuilder.Entity<Character>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("characters_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        });

        modelBuilder.Entity<Opponent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("opponents_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.User).WithMany(p => p.Opponents).HasConstraintName("opponents_user_id_fkey");
        });

        modelBuilder.Entity<PlayerProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("player_profiles_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.MainCharacter).WithMany(p => p.PlayerProfileMainCharacters)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("player_profiles_main_character_id_fkey");

            entity.HasOne(d => d.SecondaryCharacter).WithMany(p => p.PlayerProfileSecondaryCharacters)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("player_profiles_secondary_character_id_fkey");

            entity.HasOne(d => d.User).WithOne(p => p.PlayerProfile).HasConstraintName("player_profiles_user_id_fkey");
        });

        modelBuilder.Entity<Set>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sets_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.OpponentCharacter).WithMany(p => p.SetOpponentCharacters)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("sets_opponent_character_id_fkey");

            entity.HasOne(d => d.Opponent).WithMany(p => p.Sets)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("sets_opponent_id_fkey");

            entity.HasOne(d => d.Tournament).WithMany(p => p.Sets)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("sets_tournament_id_fkey");

            entity.HasOne(d => d.UserCharacter).WithMany(p => p.SetUserCharacters)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("sets_user_character_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Sets).HasConstraintName("sets_user_id_fkey");
        });

        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tournaments_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(d => d.User).WithMany(p => p.Tournaments).HasConstraintName("tournaments_user_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
