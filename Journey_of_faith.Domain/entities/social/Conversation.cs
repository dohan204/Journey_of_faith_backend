using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities.social
{
    public class Conversation
    {
        public long Id { get; set; }
        public int? GroupId { get; set; }
        public bool? IsGroup { get; set; }
        public long CreatorUserId { get; set; }
        public long? LastMessageId { get; set; }
        public DateTime? CreationTime { get; set; }
        public string? Avatar { get; set; }

        private readonly List<ConversationParticipant> _participants = new();
        private readonly List<Message> _messages = new();

        public IReadOnlyCollection<ConversationParticipant> Participants => _participants.AsReadOnly();
        public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();
    }
}
