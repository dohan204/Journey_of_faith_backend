using Journey_of_faith.Infrastructure.identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.messaging
{
    public class Message
    {
        public long Id { get; set; }
        public Guid FromUserId { get; set; }
        public long ConversationId { get; set; }
        public string MessageContent { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; }
        public int? MessageType { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool? IsDeleted { get; set; }

        public ApplicationUser FromUser { get; set; } = null!;
        public Conversation Conversation { get; set; } = null!;
        public ICollection<MessageAttachment> Attachments { get; set; } = [];
        public ICollection<MessageReaction> Reactions { get; set; } = [];
        public ICollection<MessageStatus> Statuses { get; set; } = [];
    }
}
