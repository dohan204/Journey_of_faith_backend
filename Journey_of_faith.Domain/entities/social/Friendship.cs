using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities.social
{
    public class Friendship : AuditableEntity
    {
        public long UserId { get; set; }
        public long FriendId { get; set; }
        public string Status { get; set; } = string.Empty;   // Pending, Accepted, Rejected, Blocked
    }
}
