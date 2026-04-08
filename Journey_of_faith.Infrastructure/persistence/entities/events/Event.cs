using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.events
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ImageUrl { get; set; }

        public Guid? CreatorUserId { get; set; }
        public DateTime? CreationTime { get; set; }
        public Guid? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<EventCategoryMapping> CategoryMappings { get; set; } = [];
        public ICollection<EventComment> Comments { get; set; } = [];
        public ICollection<EventFollower> Followers { get; set; } = [];
        public ICollection<EventParticipant> Participants { get; set; } = [];
        public ICollection<EventImage> Images { get; set; } = [];
        public ICollection<EventNotification> Notifications { get; set; } = [];
        public ICollection<UserEvent> UserEvents { get; set; } = [];
    }
}
