using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities.social
{
    public class GroupEvent
    {
        public long Id { get; set; }
        public long ConversationId { get; set; }
        public long UserId { get; set; }
        public int? ActionType { get; set; }
        public DateTime? ActionTime { get; set; }
    }
}
