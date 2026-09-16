using Amazon.S3;
using Amazon.S3.Model;
using melee_tracker_capstone.Data;
using melee_tracker_capstone.Data.Entities;
using melee_tracker_capstone.DTOs;

namespace melee_tracker_capstone.Services
{
    public class ReplayService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IAmazonS3 _s3Client;
        public ReplayService(AppDbContext context, IConfiguration config, IAmazonS3 s3Client) 
        {   
            _context = context;
            _config = config;
            _s3Client = s3Client;
        }

        public async Task<ReplayResponse> UploadReplay(IFormFile file, Guid userId)
        {
            // 1. Stream .slp replay to s3 bucket
            var bucketName = _config["AWS:BucketName"];
            var replayId = Guid.NewGuid();
            var s3Key = $"replays/{userId}/{replayId}{Path.GetExtension(file.FileName)}";

            using var stream = file.OpenReadStream();

            var putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = s3Key,
                InputStream = stream,
                ContentType = file.ContentType
            };

            await _s3Client.PutObjectAsync(putRequest);

            // 2. Publish job to RabbitMQ

            // 3. Create replay with status=pending
            var replay = new Replay
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                S3Key = s3Key,
                Status = ReplayStatus.Pending,
                UploadedAt = DateTime.UtcNow
            };

            _context.Replays.Add(replay);
            await _context.SaveChangesAsync();

            // 4. Return the response
            return new ReplayResponse
            {
                Id = replay.Id,
                Status = replay.Status.ToString(),
                UploadedAt = replay.UploadedAt
            };
        }

        
    }
}
