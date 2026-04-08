using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities.events
{
    public class EventFollower
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public long UserId { get; set; }
        public DateTime? FollowedTime { get; set; }
    }
}
