using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace melee_tracker_capstone.Data.Entities;

[Table("replays")]
[Index("UserId", Name = "idx_replays_user_id")]
public partial class Replay
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("s3_key")]
    [StringLength(500)]
    public string? S3Key { get; set; }

    [Column("processed_s3_key")]
    public string? ProcessedS3Key { get; set; }

    [Column("stage_id")]
    public int? StageId { get; set; }

    [Column("played_at")]
    public DateTime? PlayedAt { get; set; }

    [Column("last_frame")]
    public int? LastFrame { get; set; }

    [Column("failure_reason")]
    public string? FailureReason { get; set; }

    [Column("status")]
    public ReplayStatus Status { get; set; }

    [Column("uploaded_at")]
    public DateTime UploadedAt { get; set; }

    // Refering to user_id as a FK
    [ForeignKey("UserId")]
    [InverseProperty("Replays")]
    public virtual User User { get; set; } = null!;
}
