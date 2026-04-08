using Journey_of_faith.Infrastructure.identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.faith_notifications
{
    public class PrayerRequest
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public string? Title { get; set; }
        public string? RequestContent { get; set; }
        public bool? IsAnonymous { get; set; }

        public Guid? CreatorUserId { get; set; }
        public DateTime? CreationTime { get; set; }
        public Guid? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }

        public ApplicationUser User { get; set; } = null!;
        public ICollection<PrayerComment> Comments { get; set; } = [];
    }
}
