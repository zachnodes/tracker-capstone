namespace melee_tracker_capstone.DTOs
{
    public class ReplayProcessedMessage
    {
        public Guid ReplayId { get; set; }
        public string Status { get; set; } = "";
        public string? ProcessedS3Key { get; set; }
        public int? StageId { get; set; }
        public DateTime? PlayedAt { get; set; }
        public int? LastFrame { get; set; }
        public string? FailureReason { get; set; }
    }
}
