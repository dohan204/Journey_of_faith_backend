using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.faith_notifications
{
    public class DailyWord
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string? Title { get; set; }
        public string BibleContent { get; set; } = string.Empty;
        public string? Gospel { get; set; }
        public bool? IsShortWord { get; set; }

        public Guid? CreatorUserId { get; set; }
        public DateTime? CreationTime { get; set; }
        public Guid? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
