using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities.social
{
    public class MessageStatus
    {
        public long Id { get; set; }
        public long MessageId { get; set; }
        public long UserId { get; set; }
        public int? Status { get; set; }   // 0 Sent, 1 Delivered, 2 Seen
        public DateTime? UpdateTime { get; set; }
    }
}
