namespace melee_tracker_capstone.DTOs
{
    public class ReplayResponse
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
    }
}
