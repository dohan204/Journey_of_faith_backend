using Journey_of_faith.Domain.entities.musics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities
{
    public class User : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public int? ChurchId { get; set; }
        public int? ProvinceId { get; set; }
        public int? SchoolId { get; set; }

        private readonly List<UserChurch> _userChurches = new();
        private readonly List<Friendship> _friendships = new();
        private readonly List<Friendship> _friendOf = new();
        private readonly List<Playlist> _playlists = new();
        private readonly List<PrayerRequest> _prayerRequests = new();
        private readonly List<UserFavoriteSong> _favoriteSongs = new();
        private readonly List<DeviceToken> _deviceTokens = new();
        private readonly List<NotificationPreference> _notificationPreferences = new();
        private readonly List<ReminderSetting> _reminderSettings = new();
        private readonly List<QuizAttempt> _quizAttempts = new();
        private readonly List<ListeningHistory> _listeningHistories = new();
        private readonly List<UserEvent> _userEvents = new();

        public IReadOnlyCollection<UserChurch> UserChurches => _userChurches.AsReadOnly();
        public IReadOnlyCollection<Friendship> Friendships => _friendships.AsReadOnly();
        public IReadOnlyCollection<Friendship> FriendOf => _friendOf.AsReadOnly();
        public IReadOnlyCollection<Playlist> Playlists => _playlists.AsReadOnly();
        public IReadOnlyCollection<PrayerRequest> PrayerRequests => _prayerRequests.AsReadOnly();
        public IReadOnlyCollection<UserFavoriteSong> FavoriteSongs => _favoriteSongs.AsReadOnly();
        public IReadOnlyCollection<DeviceToken> DeviceTokens => _deviceTokens.AsReadOnly();
        public IReadOnlyCollection<NotificationPreference> NotificationPreferences => _notificationPreferences.AsReadOnly();
        public IReadOnlyCollection<ReminderSetting> ReminderSettings => _reminderSettings.AsReadOnly();
        public IReadOnlyCollection<QuizAttempt> QuizAttempts => _quizAttempts.AsReadOnly();
        public IReadOnlyCollection<ListeningHistory> ListeningHistories => _listeningHistories.AsReadOnly();
        public IReadOnlyCollection<UserEvent> UserEvents => _userEvents.AsReadOnly();

        public void AddUserChurch(UserChurch uc) => _userChurches.Add(uc);
        public void AddPlaylist(Playlist p) => _playlists.Add(p);
        public void AddFavoriteSong(UserFavoriteSong fs) => _favoriteSongs.Add(fs);
        public void AddPrayerRequest(PrayerRequest pr) => _prayerRequests.Add(pr);
    }
}
