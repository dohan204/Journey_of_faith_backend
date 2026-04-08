using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities
{
    public abstract class AuditableEntity
    {
        public Guid Id { get; set; }

        public long? CreatorUserId { get; set; }
        public DateTime? CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
