using Journey_of_faith.Infrastructure.identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.messaging
{
    public class MessageReaction
    {
        public long Id { get; set; }
        public long MessageId { get; set; }
        public Guid UserId { get; set; }
        public string? Reaction { get; set; }

        public Message Message { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}
