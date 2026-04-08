using Journey_of_faith.Infrastructure.persistence.entities.location;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.faith_notifications
{
    public class MassSchedule
    {
        public int Id { get; set; }
        public bool? IsFixed { get; set; }
        public int ChurchId { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public DateOnly? Date { get; set; }
        public TimeOnly Time { get; set; }
        public int? MassTypeId { get; set; }

        public Guid? CreatorUserId { get; set; }
        public DateTime? CreationTime { get; set; }
        public Guid? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }

        public Church Church { get; set; } = null!;
        public MassType? MassType { get; set; }
    }
}
