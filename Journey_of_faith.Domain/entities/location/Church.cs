using Journey_of_faith.Domain.entities.masslive;
using Journey_of_faith.Domain.objectvalues.churchs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities.location
{
    public class Church : AuditableEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string? Thumbnail { get; private set; }
        public string? Email { get; private set; }
        public string? Address { get; private set; }
        public int DioceseId { get; private set; }
        public string Boss {get; private set;}
        public string? Description {get; private set;}
        public GeoLocation GeoLocation { get; private set; }

        private readonly List<MassSchedule> _massSchedules = new();
        private readonly List<LiveStream> _liveStreams = new();
        private readonly List<User> _users = new();
        private readonly List<UserChurch> _userChurches = new();

        public IReadOnlyCollection<MassSchedule> MassSchedules => _massSchedules.AsReadOnly();
        public IReadOnlyCollection<LiveStream> LiveStreams => _liveStreams.AsReadOnly();
        public IReadOnlyCollection<User> Users => _users.AsReadOnly();
        public IReadOnlyCollection<UserChurch> UserChurches => _userChurches.AsReadOnly();

        private Church() { }
        public Church(string name, string thumbnail, 
            string website, string address, int discoceId, double latitude, 
            double longtitude, Guid Userid, Guid modifier, string boss, string description, 
            List<MassSchedule> massSchedules
            )
        {
            if(string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Tên nhà thờ không được để trống");
            }

            if(discoceId <= 0)
            {
                throw new ArgumentOutOfRangeException("DioceseId phải là số dương");
            }
            if(string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException("Địa chỉ nhà thờ không được để trống");
            }
            Name = name;
            Thumbnail = thumbnail;
            Email = website;
            Address = address;
            DioceseId = discoceId;
            
            GeoLocation = GeoLocation.FromCoordinates(latitude, longtitude);

            CreatorUserId = Userid;
            LastModifierUserId = modifier;
            Boss = boss;
            Description = description;
            _massSchedules = massSchedules;
        }
        public Church(int id, string name, string email, string address, int discoceId, string boss, string description, Guid lastModifier, List<MassSchedule> massSchedules)
        {
            Id = id;
            Name = name;
            Email = email;
            Address = address;
            DioceseId = discoceId;
            Boss = boss;
            Description = description;
            LastModifierUserId = lastModifier;
            _massSchedules = massSchedules;
        }
        public Church(string name, string address, int discoceId, double latitude, double longtitude)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Tên nhà thờ không được để trống");
            }
            
            if (discoceId <= 0)
            {
                throw new ArgumentOutOfRangeException("DioceseId phải là số dương");
            }

            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException("Địa chỉ nhà thờ không được để trống");
            }

            Name = name;
            Address = address;
            DioceseId = discoceId;
            GeoLocation = GeoLocation.FromCoordinates(latitude, longtitude);
        }

        public void SetLocation(double latitude, double longtitude)
        {
            GeoLocation = GeoLocation.FromCoordinates(latitude, longtitude);
        }
        public void AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User không được null");
            }
            _users.Add(user);
        }


        public void SetMassSchedule(List<MassSchedule> massSchedules)
        {
            if (massSchedules == null)
            {
                throw new ArgumentNullException(nameof(massSchedules), "MassSchedules không được null");
            }
            _massSchedules.Clear();
            _massSchedules.AddRange(massSchedules);
        }

        public void AddMassSchedule(MassSchedule ms) => _massSchedules.Add(ms);
        public void AddLiveStream(LiveStream ls) => _liveStreams.Add(ls);
    }
}
