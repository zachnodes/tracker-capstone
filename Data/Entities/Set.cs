using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace melee_tracker_capstone.Data.Entities;

[Table("sets")]
[Index("OpponentCharacterId", Name = "idx_sets_opponent_character_id")]
[Index("OpponentId", Name = "idx_sets_opponent_id")]
[Index("TournamentId", Name = "idx_sets_tournament_id")]
[Index("UserCharacterId", Name = "idx_sets_user_character_id")]
[Index("UserId", Name = "idx_sets_user_id")]
[Index("StartggSetId", Name = "sets_startgg_set_id_key", IsUnique = true)]
public partial class Set
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("tournament_id")]
    public Guid? TournamentId { get; set; }

    [Column("opponent_id")]
    public Guid OpponentId { get; set; }

    [Column("round_name")]
    [StringLength(100)]
    public string? RoundName { get; set; }

    [Column("bracket_type")]
    public BracketType BracketType { get; set; } 

    [Column("score_user")]
    public int ScoreUser { get; set; }

    [Column("score_opponent")]
    public int ScoreOpponent { get; set; }

    [Column("result")]
    public ResultType Result { get; set; }

    [Column("user_character_id")]
    public Guid? UserCharacterId { get; set; }

    [Column("opponent_character_id")]
    public Guid? OpponentCharacterId { get; set; }

    [Column("played_at")]
    public DateTime PlayedAt { get; set; }

    [Column("source")]
    public SourceType Source { get; set; }

    [Column("startgg_set_id")]
    [StringLength(100)]
    public string? StartggSetId { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("OpponentId")]
    [InverseProperty("Sets")]
    public virtual Opponent Opponent { get; set; } = null!;

    [ForeignKey("OpponentCharacterId")]
    [InverseProperty("SetOpponentCharacters")]
    public virtual Character? OpponentCharacter { get; set; }

    [ForeignKey("TournamentId")]
    [InverseProperty("Sets")]
    public virtual Tournament? Tournament { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Sets")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("UserCharacterId")]
    [InverseProperty("SetUserCharacters")]
    public virtual Character? UserCharacter { get; set; }
}
