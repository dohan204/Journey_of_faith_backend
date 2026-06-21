using Journey_of_faith.Infrastructure.identity;
using Journey_of_faith.Infrastructure.persistence.entities.events;
using Journey_of_faith.Infrastructure.persistence.entities.faith_notifications;
using Journey_of_faith.Infrastructure.persistence.entities.location;
using Journey_of_faith.Infrastructure.persistence.entities.messaging;
using Journey_of_faith.Infrastructure.persistence.entities.music;
using Journey_of_faith.Infrastructure.persistence.entities.quiz;
using Journey_of_faith.Infrastructure.persistence.entities.social;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.EnableDetailedErrors(); // hiển thị chi teiets lôi từ db.
            optionsBuilder.EnableSensitiveDataLogging(); // hiển thị ra kiểu dữ liệu nếu có trường nào bị lỗi
            base.OnConfiguring(optionsBuilder);
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        // ── Location ──────────────────────────────────────────────────────
        public DbSet<Province> Provinces => Set<Province>();
        public DbSet<Diocese> Dioceses => Set<Diocese>();
        public DbSet<Church> Churches => Set<Church>();
        public DbSet<SchoolLevel> SchoolLevels => Set<SchoolLevel>();
        public DbSet<School> Schools => Set<School>();
        public DbSet<UserChurch> UserChurches => Set<UserChurch>();

        // ── Social ────────────────────────────────────────────────────────
        public DbSet<Friendship> Friendships => Set<Friendship>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<GroupMember> GroupMembers => Set<GroupMember>();

        // ── Messaging ─────────────────────────────────────────────────────
        public DbSet<Conversation> Conversations => Set<Conversation>();
        public DbSet<ConversationParticipant> ConversationParticipants => Set<ConversationParticipant>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<MessageAttachment> MessageAttachments => Set<MessageAttachment>();
        public DbSet<MessageReaction> MessageReactions => Set<MessageReaction>();
        public DbSet<MessageStatus> MessageStatuses => Set<MessageStatus>();
        public DbSet<GroupEvent> GroupEvents => Set<GroupEvent>();

        // ── Music ─────────────────────────────────────────────────────────
        public DbSet<Artist> Artists => Set<Artist>();
        public DbSet<Album> Albums => Set<Album>();
        public DbSet<Song> Songs => Set<Song>();
        public DbSet<SongCategory> SongCategories => Set<SongCategory>();
        public DbSet<SongCategoryMapping> SongCategoryMappings => Set<SongCategoryMapping>();
        public DbSet<Playlist> Playlists => Set<Playlist>();
        public DbSet<PlaylistSong> PlaylistSongs => Set<PlaylistSong>();
        public DbSet<UserFavoriteSong> UserFavoriteSongs => Set<UserFavoriteSong>();
        public DbSet<ListeningHistory> ListeningHistories => Set<ListeningHistory>();

        // ── Events ────────────────────────────────────────────────────────
        public DbSet<Event> Events => Set<Event>();
        public DbSet<EventCategory> EventCategories => Set<EventCategory>();
        public DbSet<EventCategoryMapping> EventCategoryMappings => Set<EventCategoryMapping>();
        public DbSet<EventComment> EventComments => Set<EventComment>();
        public DbSet<EventFollower> EventFollowers => Set<EventFollower>();
        public DbSet<EventParticipant> EventParticipants => Set<EventParticipant>();
        public DbSet<EventImage> EventImages => Set<EventImage>();
        public DbSet<EventNotification> EventNotifications => Set<EventNotification>();
        public DbSet<UserEvent> UserEvents => Set<UserEvent>();

        // ── Quiz ──────────────────────────────────────────────────────────
        public DbSet<Topic> Topics => Set<Topic>();
        public DbSet<QuizLevel> QuizLevels => Set<QuizLevel>();
        public DbSet<QuestionType> QuestionTypes => Set<QuestionType>();
        public DbSet<QuestionCategory> QuestionCategories => Set<QuestionCategory>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Answer> Answers => Set<Answer>();
        public DbSet<Quiz> Quizzes => Set<Quiz>();
        public DbSet<QuizQuestion> QuizQuestions => Set<QuizQuestion>();
        public DbSet<QuizAttempt> QuizAttempts => Set<QuizAttempt>();
        public DbSet<AttemptAnswer> AttemptAnswers => Set<AttemptAnswer>();

        // ── Faith Content ─────────────────────────────────────────────────
        public DbSet<MassType> MassTypes => Set<MassType>();
        public DbSet<MassSchedule> MassSchedules => Set<MassSchedule>();
        public DbSet<MassVideo> MassVideos => Set<MassVideo>();
        public DbSet<LiveStream> LiveStreams => Set<LiveStream>();
        public DbSet<CatholicFeast> CatholicFeasts => Set<CatholicFeast>();
        public DbSet<DailyWord> DailyWords => Set<DailyWord>();
        public DbSet<PrayerRequest> PrayerRequests => Set<PrayerRequest>();
        public DbSet<PrayerComment> PrayerComments => Set<PrayerComment>();

        // ── Notification ──────────────────────────────────────────────────
        public DbSet<DeviceToken> DeviceTokens => Set<DeviceToken>();
        public DbSet<NotificationPreference> NotificationPreferences => Set<NotificationPreference>();
        public DbSet<ReminderSetting> ReminderSettings => Set<ReminderSetting>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Identity tables trước
            //builder.HasDefaultSchema("dbo");
            // Đăng ký toàn bộ config theo assembly (tự scan IEntityTypeConfiguration<>)
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

    }
}
