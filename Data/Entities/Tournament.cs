using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace melee_tracker_capstone.Data.Entities;

[Table("tournaments")]
[Index("UserId", Name = "idx_tournaments_user_id")]
[Index("StartggTournamentId", Name = "tournaments_startgg_tournament_id_key", IsUnique = true)]
public partial class Tournament
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("event_date")]
    public DateOnly EventDate { get; set; }

    [Column("placement")]
    [StringLength(50)]
    public string? Placement { get; set; }

    [Column("startgg_tournament_id")]
    [StringLength(100)]
    public string? StartggTournamentId { get; set; }

    [InverseProperty("Tournament")]
    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();

    [ForeignKey("UserId")]
    [InverseProperty("Tournaments")]
    public virtual User User { get; set; } = null!;
}
