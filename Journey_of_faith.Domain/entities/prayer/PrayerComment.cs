using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities.prayer
{
    public class PrayerComment : AuditableEntity
    {
        public long PrayerRequestId { get; set; }
        public Guid UserId { get; set; }
        public string CommentContent { get; set; } = string.Empty;
    }
}
