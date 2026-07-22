using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace melee_tracker_capstone.Data.Entities;

[Table("users")]
[Index("Email", Name = "users_email_key", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("email")]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [Column("password_hash")]
    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Opponent> Opponents { get; set; } = new List<Opponent>();

    [InverseProperty("User")]
    public virtual PlayerProfile? PlayerProfile { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();

    [InverseProperty("User")]
    public virtual ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
}
