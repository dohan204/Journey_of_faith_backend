using Journey_of_faith.Domain.entities.masslive;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities.location
{
    public class Church : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Thumbnail { get; set; }
        public string? Website { get; set; }
        public string? Address { get; set; }
        public int? DioceseId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        private readonly List<MassSchedule> _massSchedules = new();
        private readonly List<LiveStream> _liveStreams = new();
        private readonly List<User> _users = new();
        private readonly List<UserChurch> _userChurches = new();

        public IReadOnlyCollection<MassSchedule> MassSchedules => _massSchedules.AsReadOnly();
        public IReadOnlyCollection<LiveStream> LiveStreams => _liveStreams.AsReadOnly();
        public IReadOnlyCollection<User> Users => _users.AsReadOnly();
        public IReadOnlyCollection<UserChurch> UserChurches => _userChurches.AsReadOnly();

        public void AddMassSchedule(MassSchedule ms) => _massSchedules.Add(ms);
        public void AddLiveStream(LiveStream ls) => _liveStreams.Add(ls);
    }
}
