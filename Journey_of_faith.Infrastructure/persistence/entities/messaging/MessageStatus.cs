using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.messaging
{
    public class MessageStatus
    {
        public long Id { get; set; }
        public long MessageId { get; set; }
        public Guid UserId { get; set; }
        public int? Status { get; set; }
        public DateTime? UpdateTime { get; set; }

        public Message Message { get; set; } = null!;
    }
}
