using melee_tracker_capstone.Data.Entities;

namespace melee_tracker_capstone.DTOs
{
    public class CreateSetRequest
    {
        public string OpponentTag { get; set; } = "";
        public int ScoreUser { get; set; }
        public int ScoreOpponent { get; set; }
        public Guid? UserCharacterId { get; set; }
        public Guid? OpponentCharacterId { get; set; }
        public string? RoundName { get; set; }
        public BracketType? BracketType { get; set; }
        public string? Notes { get; set; }
        public DateTime PlayedAt { get; set; }
    }
}
