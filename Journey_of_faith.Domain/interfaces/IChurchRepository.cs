using Journey_of_faith.Domain.dtos;
using Journey_of_faith.Domain.entities.catholic;
using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Domain.entities.masslive;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.interfaces
{
    public class ChurchListItemView
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Thumbnail { get; set; }
        public string? Website { get; set; }
        public string? Address { get; set; }
        public int? DioceseId { get; set; }
        public string? DioceseName { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public bool IsFollowed { get; set; }
    }

    public class PersonalizedMassScheduleView
    {
        public int MassScheduleId { get; set; }
        public int ChurchId { get; set; }
        public string ChurchName { get; set; } = string.Empty;
        public string? ChurchAddress { get; set; }
        public bool? IsFixed { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public TimeSpan Time { get; set; }
        public int? MassTypeId { get; set; }
        public string? MassTypeName { get; set; }
    }

    public class ReminderSettingView
    {
        public bool MassReminderEnabled { get; set; }
        public int MinutesBefore { get; set; }
        public string? SpeechGender { get; set; }
        public double? SpeechSpeed { get; set; }
    }

    public interface IChurchRepository
    {
        // MassTypye
        Task<int> CreateAsync(MassType massType);
        Task<int> DeleteMassType(int id);
        // Church
        Task<Church?> GetChurchByIdAsync(int id, CancellationToken cancellationTokenc);
        Task<PagedResult<Church>> GetChurchesAsync(int page, int pageSize, string? search);
        Task<int> CreateAsync(Church church);
        Task<int> UpdateAsync(Church church, Guid UserId);
        Task<bool> DeleteChurchAsync(int id, bool? force = false);
        // Task<bool> UpdateChurchAsync(int id)
        // Dicosce
        Task<bool> GetDioceseExistsAsync(int dioceseId);
        Task<bool> UniqueNameDiocese(string name);
        Task<Diocese?> GetDioceseByIdAsync(int id);
        Task<IEnumerable<Diocese>> GetAllDiocesesAsync();
        Task<int> CreateAsync(Diocese diocese);
        Task<int> UpdateAsync(Diocese diocese);
        Task<int> DeleteDiocese(int id);

        // Use cases: follow churches, personalized mass schedules, reminder settings
        Task<IEnumerable<ChurchListItemView>> SearchChurchesAsync(string? keyword, int? dioceseId, Guid? userId);
        Task<bool> ChurchExistsAsync(int churchId);
        Task<bool> IsFollowingChurchAsync(Guid userId, int churchId);
        Task<bool> FollowChurchAsync(Guid userId, int churchId);
        Task<bool> UnfollowChurchAsync(Guid userId, int churchId);
        Task<IEnumerable<ChurchListItemView>> GetFollowedChurchesAsync(Guid userId);
        Task<IEnumerable<PersonalizedMassScheduleView>> GetPersonalizedMassSchedulesAsync(Guid userId, DateTime fromDate, DateTime toDate, int? churchId);
        Task<ReminderSettingView> GetReminderSettingAsync(Guid userId);
        Task<ReminderSettingView> SaveReminderSettingAsync(Guid userId, bool isEnabled, int minutesBefore, string? speechGender, double? speechSpeed);
        // Daily words
        Task<bool> CreateDailyWorld(DailyWord dailyWord);
        Task<DailyWord?> GetDailyWorldAsync(DateTime dailyDay);
        Task<PagedResult<Church>> GetChurchWithCondition(string churchName, string province, string wards, string time, int page, int pageSize);
    }
}
