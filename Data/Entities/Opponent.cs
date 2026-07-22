using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace melee_tracker_capstone.Data.Entities;

[Table("opponents")]
[Index("UserId", Name = "idx_opponents_user_id")]
public partial class Opponent
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("tag")]
    [StringLength(100)]
    public string Tag { get; set; } = null!;

    [Column("startgg_player_id")]
    [StringLength(100)]
    public string? StartggPlayerId { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [InverseProperty("Opponent")]
    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();

    [ForeignKey("UserId")]
    [InverseProperty("Opponents")]
    public virtual User User { get; set; } = null!;
}
