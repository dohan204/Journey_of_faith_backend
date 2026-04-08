using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities.social
{
    public class MessageReaction
    {
        public long Id { get; set; }
        public long MessageId { get; set; }
        public long UserId { get; set; }
        public string? Reaction { get; set; }   // 👍 ❤️ 😂 ...
    }
}
