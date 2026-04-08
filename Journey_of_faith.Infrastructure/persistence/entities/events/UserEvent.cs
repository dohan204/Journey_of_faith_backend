using Journey_of_faith.Infrastructure.identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.events
{
    public class UserEvent
    {
        public Guid UserId { get; set; }
        public int EventId { get; set; }
        public DateTime FollowedAt { get; set; }

        public ApplicationUser User { get; set; } = null!;
        public Event Event { get; set; } = null!;
    }
}
