using Journey_of_faith.Infrastructure.identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.events
{
    public class EventComment
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Guid UserId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime? CreatedTime { get; set; }

        public Event Event { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}
