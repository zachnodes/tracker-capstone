using melee_tracker_capstone.Data;
using melee_tracker_capstone.Data.Entities;
using melee_tracker_capstone.DTOs;
using Microsoft.EntityFrameworkCore;

namespace melee_tracker_capstone.Services
{
    public class SetService
    {
        private readonly AppDbContext _context; // use your real DbContext name

        public SetService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateManualSetAsync(Guid userId, CreateSetRequest request)
        {
            var tag = request.OpponentTag.Trim();

            var opponent = await GetOrCreateOpponentAsync(userId, tag);

            var set = new Set
            {
                UserId = userId,
                OpponentId = opponent.Id,
                RoundName = request.RoundName,
                BracketType = request.BracketType,
                ScoreUser = request.ScoreUser,
                ScoreOpponent = request.ScoreOpponent,
                Result = request.ScoreUser > request.ScoreOpponent ? ResultType.Win : ResultType.Loss,
                UserCharacterId = request.UserCharacterId,
                OpponentCharacterId = request.OpponentCharacterId,
                PlayedAt = request.PlayedAt.ToUniversalTime(),
                Source = SourceType.Manual,
                Notes = request.Notes
            };

            _context.Sets.Add(set);
            await _context.SaveChangesAsync();
            return set.Id;
        }

        private async Task<Opponent> GetOrCreateOpponentAsync(Guid userId, string tag)
        {
            var query = from o in _context.Opponents
                        where o.UserId == userId && o.Tag == tag
                        select o;

            var opponent = await query.FirstOrDefaultAsync();
            if (opponent != null) return opponent;

            opponent = new Opponent { UserId = userId, Tag = tag };
            _context.Opponents.Add(opponent);
            await _context.SaveChangesAsync();
            return opponent;
        }
    }
}
