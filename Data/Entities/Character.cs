using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace melee_tracker_capstone.Data.Entities;

[Table("characters")]
public partial class Character
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("sprite_url")]
    [StringLength(500)]
    public string? SpriteUrl { get; set; }

    [InverseProperty("MainCharacter")]
    public virtual ICollection<PlayerProfile> PlayerProfileMainCharacters { get; set; } = new List<PlayerProfile>();

    [InverseProperty("SecondaryCharacter")]
    public virtual ICollection<PlayerProfile> PlayerProfileSecondaryCharacters { get; set; } = new List<PlayerProfile>();

    [InverseProperty("OpponentCharacter")]
    public virtual ICollection<Set> SetOpponentCharacters { get; set; } = new List<Set>();

    [InverseProperty("UserCharacter")]
    public virtual ICollection<Set> SetUserCharacters { get; set; } = new List<Set>();
}
