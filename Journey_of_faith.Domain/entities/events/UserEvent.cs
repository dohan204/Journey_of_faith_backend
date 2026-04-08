using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities.events
{
    public class UserEvent
    {
        public long UserId { get; set; }
        public int EventId { get; set; }
        public DateTime FollowedAt { get; set; }
    }
}
