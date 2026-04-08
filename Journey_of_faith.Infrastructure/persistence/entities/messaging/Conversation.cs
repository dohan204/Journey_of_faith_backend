using Journey_of_faith.Infrastructure.identity;
using Journey_of_faith.Infrastructure.persistence.entities.social;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.messaging
{
    public class Conversation
    {
        public long Id { get; set; }
        public int? GroupId { get; set; }
        public bool? IsGroup { get; set; }
        public Guid CreatorUserId { get; set; }
        public long? LastMessageId { get; set; }
        public DateTime? CreationTime { get; set; }
        public string? Avatar { get; set; }

        public Group? Group { get; set; }
        public ApplicationUser Creator { get; set; } = null!;
        public ICollection<ConversationParticipant> Participants { get; set; } = [];
        public ICollection<Message> Messages { get; set; } = [];
        public ICollection<GroupEvent> GroupEvents { get; set; } = [];
    }
}
