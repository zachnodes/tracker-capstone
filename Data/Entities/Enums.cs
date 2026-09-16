namespace melee_tracker_capstone.Data.Entities
{
    public enum SourceType
    {
        Manual,
        Startgg
    }

    public enum ResultType
    {
        Win,
        Loss
    }

    public enum BracketType
    {
        Winners,
        Losers,
        GrandFinals
    }

    public enum ReplayStatus
    {
        Pending,
        Processing,
        Complete,
        Failed
    }
}
