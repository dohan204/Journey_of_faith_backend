using Journey_of_faith.Infrastructure.persistence.entities.messaging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.social
{
    public class Group
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Avatar { get; set; }
        public string? GroupType { get; set; }
        public string? Privacy { get; set; }

        public Guid? CreatorUserId { get; set; }
        public DateTime? CreationTime { get; set; }
        public Guid? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<GroupMember> Members { get; set; } = [];
        public ICollection<Conversation> Conversations { get; set; } = [];
    }
}
