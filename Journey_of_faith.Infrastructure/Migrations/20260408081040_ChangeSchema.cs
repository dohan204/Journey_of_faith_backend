using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Journey_of_faith.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "UserFavoriteSong",
                newName: "UserFavoriteSong",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "UserEvent",
                newName: "UserEvent",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "UserChurch",
                newName: "UserChurch",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "User",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "SongCategoryMapping",
                newName: "SongCategoryMapping",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "SongCategory",
                newName: "SongCategory",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Song",
                newName: "Song",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "SchoolLevel",
                newName: "SchoolLevel",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "School",
                newName: "School",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ReminderSetting",
                newName: "ReminderSetting",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "QuizQuestion",
                newName: "QuizQuestion",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "QuizLevel",
                newName: "QuizLevel",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "QuizAttempt",
                newName: "QuizAttempt",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Quiz",
                newName: "Quiz",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "QuestionType",
                newName: "QuestionType",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "QuestionCategory",
                newName: "QuestionCategory",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Question",
                newName: "Question",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Province",
                newName: "Province",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "PrayerRequest",
                newName: "PrayerRequest",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "PrayerComment",
                newName: "PrayerComment",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "PlaylistSong",
                newName: "PlaylistSong",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Playlist",
                newName: "Playlist",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "NotificationPreference",
                newName: "NotificationPreference",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "MessageStatus",
                newName: "MessageStatus",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "MessageReaction",
                newName: "MessageReaction",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "MessageAttachment",
                newName: "MessageAttachment",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Message",
                newName: "Message",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "MassVideo",
                newName: "MassVideo",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "MassType",
                newName: "MassType",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "MassSchedule",
                newName: "MassSchedule",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "LiveStream",
                newName: "LiveStream",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ListeningHistory",
                newName: "ListeningHistory",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GroupMember",
                newName: "GroupMember",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GroupEvent",
                newName: "GroupEvent",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Group",
                newName: "Group",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Friendship",
                newName: "Friendship",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "EventParticipant",
                newName: "EventParticipant",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "EventNotification",
                newName: "EventNotification",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "EventImage",
                newName: "EventImage",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "EventFollower",
                newName: "EventFollower",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "EventComment",
                newName: "EventComment",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "EventCategoryMapping",
                newName: "EventCategoryMapping",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "EventCategory",
                newName: "EventCategory",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Event",
                newName: "Event",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Diocese",
                newName: "Diocese",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "DeviceToken",
                newName: "DeviceToken",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "DailyWord",
                newName: "DailyWord",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ConversationParticipant",
                newName: "ConversationParticipant",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Conversation",
                newName: "Conversation",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Church",
                newName: "Church",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "CatholicFeast",
                newName: "CatholicFeast",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AttemptAnswer",
                newName: "AttemptAnswer",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "AspNetUserTokens",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "AspNetUserRoles",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "AspNetUserLogins",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "AspNetUserClaims",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "AspNetRoles",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "AspNetRoleClaims",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Artist",
                newName: "Artist",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Answer",
                newName: "Answer",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Album",
                newName: "Album",
                newSchema: "dbo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "UserFavoriteSong",
                schema: "dbo",
                newName: "UserFavoriteSong");

            migrationBuilder.RenameTable(
                name: "UserEvent",
                schema: "dbo",
                newName: "UserEvent");

            migrationBuilder.RenameTable(
                name: "UserChurch",
                schema: "dbo",
                newName: "UserChurch");

            migrationBuilder.RenameTable(
                name: "User",
                schema: "dbo",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "SongCategoryMapping",
                schema: "dbo",
                newName: "SongCategoryMapping");

            migrationBuilder.RenameTable(
                name: "SongCategory",
                schema: "dbo",
                newName: "SongCategory");

            migrationBuilder.RenameTable(
                name: "Song",
                schema: "dbo",
                newName: "Song");

            migrationBuilder.RenameTable(
                name: "SchoolLevel",
                schema: "dbo",
                newName: "SchoolLevel");

            migrationBuilder.RenameTable(
                name: "School",
                schema: "dbo",
                newName: "School");

            migrationBuilder.RenameTable(
                name: "ReminderSetting",
                schema: "dbo",
                newName: "ReminderSetting");

            migrationBuilder.RenameTable(
                name: "QuizQuestion",
                schema: "dbo",
                newName: "QuizQuestion");

            migrationBuilder.RenameTable(
                name: "QuizLevel",
                schema: "dbo",
                newName: "QuizLevel");

            migrationBuilder.RenameTable(
                name: "QuizAttempt",
                schema: "dbo",
                newName: "QuizAttempt");

            migrationBuilder.RenameTable(
                name: "Quiz",
                schema: "dbo",
                newName: "Quiz");

            migrationBuilder.RenameTable(
                name: "QuestionType",
                schema: "dbo",
                newName: "QuestionType");

            migrationBuilder.RenameTable(
                name: "QuestionCategory",
                schema: "dbo",
                newName: "QuestionCategory");

            migrationBuilder.RenameTable(
                name: "Question",
                schema: "dbo",
                newName: "Question");

            migrationBuilder.RenameTable(
                name: "Province",
                schema: "dbo",
                newName: "Province");

            migrationBuilder.RenameTable(
                name: "PrayerRequest",
                schema: "dbo",
                newName: "PrayerRequest");

            migrationBuilder.RenameTable(
                name: "PrayerComment",
                schema: "dbo",
                newName: "PrayerComment");

            migrationBuilder.RenameTable(
                name: "PlaylistSong",
                schema: "dbo",
                newName: "PlaylistSong");

            migrationBuilder.RenameTable(
                name: "Playlist",
                schema: "dbo",
                newName: "Playlist");

            migrationBuilder.RenameTable(
                name: "NotificationPreference",
                schema: "dbo",
                newName: "NotificationPreference");

            migrationBuilder.RenameTable(
                name: "MessageStatus",
                schema: "dbo",
                newName: "MessageStatus");

            migrationBuilder.RenameTable(
                name: "MessageReaction",
                schema: "dbo",
                newName: "MessageReaction");

            migrationBuilder.RenameTable(
                name: "MessageAttachment",
                schema: "dbo",
                newName: "MessageAttachment");

            migrationBuilder.RenameTable(
                name: "Message",
                schema: "dbo",
                newName: "Message");

            migrationBuilder.RenameTable(
                name: "MassVideo",
                schema: "dbo",
                newName: "MassVideo");

            migrationBuilder.RenameTable(
                name: "MassType",
                schema: "dbo",
                newName: "MassType");

            migrationBuilder.RenameTable(
                name: "MassSchedule",
                schema: "dbo",
                newName: "MassSchedule");

            migrationBuilder.RenameTable(
                name: "LiveStream",
                schema: "dbo",
                newName: "LiveStream");

            migrationBuilder.RenameTable(
                name: "ListeningHistory",
                schema: "dbo",
                newName: "ListeningHistory");

            migrationBuilder.RenameTable(
                name: "GroupMember",
                schema: "dbo",
                newName: "GroupMember");

            migrationBuilder.RenameTable(
                name: "GroupEvent",
                schema: "dbo",
                newName: "GroupEvent");

            migrationBuilder.RenameTable(
                name: "Group",
                schema: "dbo",
                newName: "Group");

            migrationBuilder.RenameTable(
                name: "Friendship",
                schema: "dbo",
                newName: "Friendship");

            migrationBuilder.RenameTable(
                name: "EventParticipant",
                schema: "dbo",
                newName: "EventParticipant");

            migrationBuilder.RenameTable(
                name: "EventNotification",
                schema: "dbo",
                newName: "EventNotification");

            migrationBuilder.RenameTable(
                name: "EventImage",
                schema: "dbo",
                newName: "EventImage");

            migrationBuilder.RenameTable(
                name: "EventFollower",
                schema: "dbo",
                newName: "EventFollower");

            migrationBuilder.RenameTable(
                name: "EventComment",
                schema: "dbo",
                newName: "EventComment");

            migrationBuilder.RenameTable(
                name: "EventCategoryMapping",
                schema: "dbo",
                newName: "EventCategoryMapping");

            migrationBuilder.RenameTable(
                name: "EventCategory",
                schema: "dbo",
                newName: "EventCategory");

            migrationBuilder.RenameTable(
                name: "Event",
                schema: "dbo",
                newName: "Event");

            migrationBuilder.RenameTable(
                name: "Diocese",
                schema: "dbo",
                newName: "Diocese");

            migrationBuilder.RenameTable(
                name: "DeviceToken",
                schema: "dbo",
                newName: "DeviceToken");

            migrationBuilder.RenameTable(
                name: "DailyWord",
                schema: "dbo",
                newName: "DailyWord");

            migrationBuilder.RenameTable(
                name: "ConversationParticipant",
                schema: "dbo",
                newName: "ConversationParticipant");

            migrationBuilder.RenameTable(
                name: "Conversation",
                schema: "dbo",
                newName: "Conversation");

            migrationBuilder.RenameTable(
                name: "Church",
                schema: "dbo",
                newName: "Church");

            migrationBuilder.RenameTable(
                name: "CatholicFeast",
                schema: "dbo",
                newName: "CatholicFeast");

            migrationBuilder.RenameTable(
                name: "AttemptAnswer",
                schema: "dbo",
                newName: "AttemptAnswer");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "dbo",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "dbo",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "dbo",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "dbo",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "dbo",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "dbo",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "Artist",
                schema: "dbo",
                newName: "Artist");

            migrationBuilder.RenameTable(
                name: "Answer",
                schema: "dbo",
                newName: "Answer");

            migrationBuilder.RenameTable(
                name: "Album",
                schema: "dbo",
                newName: "Album");
        }
    }
}
