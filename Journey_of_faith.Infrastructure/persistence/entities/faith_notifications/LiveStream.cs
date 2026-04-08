using Journey_of_faith.Infrastructure.persistence.entities.location;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.faith_notifications
{
    public class LiveStream
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string YouTubeUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int? ChurchId { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public bool IsLive { get; set; }
        public DateTime CreatedAt { get; set; }

        public Church? Church { get; set; }
    }
}
