using Journey_of_faith.Infrastructure.identity;
using System;
using System.Collections.Generic;
using System.Text;
using Journey_of_faith.Infrastructure.persistence.entities.faith_notifications;
namespace Journey_of_faith.Infrastructure.persistence.entities.location
{
    public class Church
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Thumbnail { get; set; }
        public string? Website { get; set; }
        public string? Address { get; set; }
        public int? DioceseId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public Guid? CreatorUserId { get; set; }
        public DateTime? CreationTime { get; set; }
        public Guid? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }

        public Diocese? Diocese { get; set; }
        public ICollection<ApplicationUser> Users { get; set; } = [];
        public ICollection<UserChurch> UserChurches { get; set; } = [];
        public ICollection<MassSchedule> MassSchedules { get; set; } = [];
        public ICollection<LiveStream> LiveStreams { get; set; } = [];
    }
}
