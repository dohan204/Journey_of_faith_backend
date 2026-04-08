using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.events
{
    public class EventNotification
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string NotifyContent { get; set; } = string.Empty;
        public DateTime? CreatedTime { get; set; }

        public Event Event { get; set; } = null!;
    }
}
