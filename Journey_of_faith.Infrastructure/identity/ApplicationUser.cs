using Microsoft.AspNetCore.Identity;
using Journey_of_faith.Infrastructure.persistence.entities.events;
using Journey_of_faith.Infrastructure.persistence.entities.quiz;
using Journey_of_faith.Infrastructure.persistence.entities.location;
using Journey_of_faith.Infrastructure.persistence.entities.messaging;
using Journey_of_faith.Infrastructure.persistence.entities.social;
using Journey_of_faith.Infrastructure.persistence.entities.music;
using Journey_of_faith.Infrastructure.persistence.entities.faith_notifications;

namespace Journey_of_faith.Infrastructure.identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string? Avatar { get; set; }

        public int? ChurchId { get; set; }
        public int? ProvinceId { get; set; }
        public int? SchoolId { get; set; }

        // Audit fields
        public Guid? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? LastModifierUserId { get; set; }
        public DateTime LastModificationTime { get; set; }
        public Guid? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation
        public Church? Church { get; set; }
        public Province? Province { get; set; }
        public School? School { get; set; }

        public ICollection<UserChurch> UserChurches { get; set; } = [];
        public ICollection<Friendship> Friendships { get; set; } = [];
        public ICollection<Friendship> FriendOf { get; set; } = [];
        public ICollection<GroupMember> GroupMembers { get; set; } = [];
        public ICollection<Playlist> Playlists { get; set; } = [];
        public ICollection<UserFavoriteSong> FavoriteSongs { get; set; } = [];
        public ICollection<ListeningHistory> ListeningHistories { get; set; } = [];
        public ICollection<EventComment> EventComments { get; set; } = [];
        public ICollection<EventFollower> EventFollowers { get; set; } = [];
        public ICollection<EventParticipant> EventParticipants { get; set; } = [];
        public ICollection<UserEvent> UserEvents { get; set; } = [];
        public ICollection<QuizAttempt> QuizAttempts { get; set; } = [];
        public ICollection<PrayerRequest> PrayerRequests { get; set; } = [];
        public ICollection<PrayerComment> PrayerComments { get; set; } = [];
        public ICollection<DeviceToken> DeviceTokens { get; set; } = [];
        public ICollection<NotificationPreference> NotificationPreferences { get; set; } = [];
        public ICollection<ReminderSetting> ReminderSettings { get; set; } = [];
        public ICollection<Conversation> CreatedConversations { get; set; } = [];
        public ICollection<ConversationParticipant> ConversationParticipants { get; set; } = [];
        public ICollection<Message> SentMessages { get; set; } = [];
        public ICollection<MessageReaction> MessageReactions { get; set; } = [];
        public ICollection<UserActive> userActives{ get; set; } = [];
    }
}
