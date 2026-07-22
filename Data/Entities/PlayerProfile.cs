using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace melee_tracker_capstone.Data.Entities;

[Table("player_profiles")]
[Index("MainCharacterId", Name = "idx_player_profiles_main_char_id")]
[Index("SecondaryCharacterId", Name = "idx_player_profiles_secondary_char_id")]
[Index("UserId", Name = "idx_player_profiles_user_id")]
[Index("UserId", Name = "player_profiles_user_id_key", IsUnique = true)]
public partial class PlayerProfile
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("gamertag")]
    [StringLength(100)]
    public string Gamertag { get; set; } = null!;

    [Column("main_character_id")]
    public Guid? MainCharacterId { get; set; }

    [Column("secondary_character_id")]
    public Guid? SecondaryCharacterId { get; set; }

    [Column("region")]
    [StringLength(100)]
    public string? Region { get; set; }

    [Column("startgg_player_id")]
    [StringLength(100)]
    public string? StartggPlayerId { get; set; }

    [Column("startgg_slug")]
    [StringLength(255)]
    public string? StartggSlug { get; set; }

    [Column("avatar_url")]
    [StringLength(500)]
    public string? AvatarUrl { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("MainCharacterId")]
    [InverseProperty("PlayerProfileMainCharacters")]
    public virtual Character? MainCharacter { get; set; }

    [ForeignKey("SecondaryCharacterId")]
    [InverseProperty("PlayerProfileSecondaryCharacters")]
    public virtual Character? SecondaryCharacter { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("PlayerProfile")]
    public virtual User User { get; set; } = null!;
}
