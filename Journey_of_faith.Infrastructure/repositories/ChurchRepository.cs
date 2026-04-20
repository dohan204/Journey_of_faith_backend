using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Application.usecases.churchs.dtos;
using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Domain.entities.masslive;
using Journey_of_faith.Domain.interfaces;
using Journey_of_faith.Infrastructure.common;
using Microsoft.Extensions.Options;
using System.Data;

namespace Journey_of_faith.Infrastructure.repositories
{
    public class ChurchRepository : BaseRepository, IChurchRepository
    {
        private const int DefaultReminderMinutes = 30;

        public ChurchRepository(IDbConnectionFactory dbConnection, IOptions<TableSchemaName> schemaName)
            : base(dbConnection, schemaName)
        {
        }

        #region Crud church
        public async Task<int> CreateAsync(Church church)
        {
            var churchDto = new CreateChurchDto
            {
                Name = church.Name,
                Thumbnail = church.Thumbnail ?? "",
                Website = church.Website ?? "",
                Address = church.Address,
                DioceseId = church.DioceseId ?? 0,
                Latitude = church.GeoLocation.Latitude,
                Longitude = church.GeoLocation.Longitude,
                CreatorUserId = church.CreatorUserId,
                LastModifierUserId = church.LastModifierUserId
            };

            return await ExecuteAsync(async connection =>
                await connection.ExecuteScalarAsync<int>($@"
                    INSERT INTO [{_schemaName.Schema}].[{TableTopicChurch.Church}]
                    (
                        Name,
                        Thumbnail,
                        Website,
                        Address,
                        DioceseId,
                        Latitude,
                        Longitude,
                        CreatorUserId,
                        LastModifierUserId
                    )
                    OUTPUT inserted.Id
                    VALUES
                    (
                        @Name,
                        @Thumbnail,
                        @Website,
                        @Address,
                        @DioceseId,
                        @Latitude,
                        @Longitude,
                        @CreatorUserId,
                        @LastModifierUserId
                    )
                ", churchDto)
            );
        }

        public async Task<IEnumerable<Church>> GetAllAsync()
        {
            return await ExecuteAsync(async connection =>
            {
                using var multiple = await connection.QueryMultipleAsync($@"
                    SELECT *
                    FROM [{_schemaName.Schema}].[{TableTopicChurch.Church}]
                    WHERE IsDeleted = 0;

                    SELECT *
                    FROM [{_schemaName.Schema}].[{TableTopicChurch.MassSchedule}]
                    WHERE IsDeleted = 0;
                ");

                var churches = (await multiple.ReadAsync<Church>()).ToList();
                var massSchedules = (await multiple.ReadAsync<MassSchedule>()).ToList();

                foreach (var church in churches)
                {
                    church.SetMassSchedule(massSchedules.Where(ms => ms.ChurchId == church.Id).ToList());
                }

                return churches;
            });
        }

        public async Task<Church?> GetByIdAsync(int id)
        {
            return await ExecuteAsync(async connection =>
            {
                using var multiple = await connection.QueryMultipleAsync($@"
                    SELECT *
                    FROM [{_schemaName.Schema}].[{TableTopicChurch.Church}]
                    WHERE Id = @Id AND IsDeleted = 0;

                    SELECT *
                    FROM [{_schemaName.Schema}].[{TableTopicChurch.MassSchedule}]
                    WHERE ChurchId = @Id AND IsDeleted = 0;
                ", new { Id = id });

                var church = await multiple.ReadSingleOrDefaultAsync<Church>();
                if (church is null)
                {
                    return null;
                }

                var massSchedules = (await multiple.ReadAsync<MassSchedule>()).ToList();
                church.SetMassSchedule(massSchedules);
                return church;
            });
        }

        public async Task<int> UpdateAsync(Church church)
        {
            var updateChurchDto = new ChurchUpdateDto
            {
                Id = church.Id,
                Name = church.Name,
                Thumbnail = church.Thumbnail,
                Website = church.Website,
                Address = church.Address,
                DioceseId = church.DioceseId,
                Latitude = church.GeoLocation.Latitude,
                Longitude = church.GeoLocation.Longitude
            };

            return await ExecuteAsync(async connection =>
                await connection.ExecuteAsync($@"
                    UPDATE [{_schemaName.Schema}].[{TableTopicChurch.Church}] SET
                        Name = COALESCE(@Name, Name),
                        Thumbnail = COALESCE(@Thumbnail, Thumbnail),
                        Website = COALESCE(@Website, Website),
                        Address = COALESCE(@Address, Address),
                        DioceseId = COALESCE(@DioceseId, DioceseId),
                        Latitude = COALESCE(@Latitude, Latitude),
                        Longitude = COALESCE(@Longitude, Longitude),
                        LastModificationTime = GETDATE()
                    WHERE Id = @Id AND IsDeleted = 0
                ", updateChurchDto)
            );
        }
        #endregion

        #region Diocese
        public async Task<int> CreateAsync(Diocese diocese)
        {
            var dioceseDto = new CreateDiscoseDto
            {
                Name = diocese.Name,
                Website = diocese.Website ?? "",
                Address = diocese.Address ?? "",
                Thumbnail = diocese.Thumbnail ?? "",
                CreatorUserId = diocese.CreatorUserId,
                LastModifierUserId = diocese.LastModifierUserId
            };

            return await ExecuteAsync(async connection =>
                await connection.ExecuteScalarAsync<int>($@"
                    INSERT INTO [{_schemaName.Schema}].[{TableTopicChurch.Diocese}]
                    (
                        Name,
                        Website,
                        Address,
                        Thumbnail,
                        CreatorUserId,
                        LastModifierUserId
                    )
                    OUTPUT inserted.Id
                    VALUES
                    (
                        @Name,
                        @Website,
                        @Address,
                        @Thumbnail,
                        @CreatorUserId,
                        @LastModifierUserId
                    )
                ", dioceseDto)
            );
        }

        public async Task<Diocese?> GetDioceseByIdAsync(int id)
        {
            return await ExecuteAsync(async connection =>
            {
                using var multiple = await connection.QueryMultipleAsync($@"
                    SELECT *
                    FROM [{_schemaName.Schema}].[{TableTopicChurch.Diocese}]
                    WHERE Id = @Id AND IsDeleted = 0;

                    SELECT *
                    FROM [{_schemaName.Schema}].[{TableTopicChurch.Church}]
                    WHERE DioceseId = @Id AND IsDeleted = 0;
                ", new { Id = id });

                var diocese = await multiple.ReadSingleOrDefaultAsync<Diocese>();
                if (diocese is null)
                {
                    return null;
                }

                var churches = (await multiple.ReadAsync<Church>()).ToList();
                diocese.SetChurch(churches);
                return diocese;
            });
        }

        public async Task<IEnumerable<Diocese>> GetAllDiocesesAsync()
        {
            return await ExecuteAsync(async connection =>
            {
                using var multi = await connection.QueryMultipleAsync($@"
                    SELECT *
                    FROM [{_schemaName.Schema}].[{TableTopicChurch.Diocese}]
                    WHERE IsDeleted = 0;

                    SELECT *
                    FROM [{_schemaName.Schema}].[{TableTopicChurch.Church}]
                    WHERE IsDeleted = 0;
                ");

                var dioceses = (await multi.ReadAsync<Diocese>()).ToList();
                var churches = (await multi.ReadAsync<Church>()).ToList();

                foreach (var diocese in dioceses)
                {
                    diocese.SetChurch(churches.Where(c => c.DioceseId == diocese.Id).ToList());
                }

                return dioceses;
            });
        }

        public async Task<int> UpdateAsync(Diocese diocese)
        {
            return await ExecuteAsync(async connection =>
                await connection.ExecuteAsync($@"
                    UPDATE [{_schemaName.Schema}].[{TableTopicChurch.Diocese}] SET
                        Name = COALESCE(@Name, Name),
                        Website = COALESCE(@Website, Website),
                        Address = COALESCE(@Address, Address),
                        Thumbnail = COALESCE(@Thumbnail, Thumbnail),
                        LastModifierUserId = @LastModifierUserId,
                        LastModificationTime = GETDATE()
                    WHERE Id = @Id AND IsDeleted = 0
                ", new
                {
                    diocese.Id,
                    diocese.Name,
                    diocese.Website,
                    diocese.Address,
                    diocese.Thumbnail,
                    diocese.LastModifierUserId
                })
            );
        }

        public async Task<int> DeleteDiocese(int id)
        {
            return await ExecuteAsync(async connection =>
                await connection.ExecuteAsync($@"
                    UPDATE [{_schemaName.Schema}].[{TableTopicChurch.Diocese}]
                    SET IsDeleted = 1,
                        DeletionTime = GETDATE()
                    WHERE Id = @Id
                ", new { Id = id })
            );
        }
        #endregion

        #region MassType
        public async Task<int> CreateAsync(MassType massType)
        {
            return await ExecuteAsync(async connection =>
                await connection.ExecuteScalarAsync<int>($@"
                    INSERT INTO [{_schemaName.Schema}].[{TableTopicChurch.MassType}] (Name)
                    OUTPUT inserted.Id
                    VALUES (@Name)
                ", new { massType.Name })
            );
        }

        public async Task<int> DeleteMassType(int id)
        {
            return await ExecuteAsync(async connection =>
                await connection.ExecuteAsync($@"
                    DELETE FROM [{_schemaName.Schema}].[{TableTopicChurch.MassType}]
                    WHERE Id = @Id
                ", new { Id = id })
            );
        }
        #endregion

        #region Basic validations
        public async Task<bool> GetDioceseExistsAsync(int dioceseId)
        {
            return await ExecuteAsync(async connection =>
                await connection.ExecuteScalarAsync<int>($@"
                    IF EXISTS
                    (
                        SELECT 1
                        FROM [{_schemaName.Schema}].[{TableTopicChurch.Diocese}]
                        WHERE Id = @Id AND IsDeleted = 0
                    )
                        SELECT 1
                    ELSE
                        SELECT 0
                ", new { Id = dioceseId }) > 0
            );
        }

        public async Task<bool> UniqueNameDiocese(string name)
        {
            return await ExecuteAsync(async connection =>
                await connection.ExecuteScalarAsync<int>($@"
                    IF EXISTS
                    (
                        SELECT 1
                        FROM [{_schemaName.Schema}].[{TableTopicChurch.Diocese}]
                        WHERE Name = @Name AND IsDeleted = 0
                    )
                        SELECT 1
                    ELSE
                        SELECT 0
                ", new { Name = name }) > 0
            );
        }

        public async Task<bool> ChurchExistsAsync(int churchId)
        {
            return await ExecuteAsync(async connection =>
                await connection.ExecuteScalarAsync<int>($@"
                    IF EXISTS
                    (
                        SELECT 1
                        FROM [{_schemaName.Schema}].[{TableTopicChurch.Church}]
                        WHERE Id = @ChurchId AND IsDeleted = 0
                    )
                        SELECT 1
                    ELSE
                        SELECT 0
                ", new { ChurchId = churchId }) > 0
            );
        }
        #endregion

        #region Follow church use cases
        public async Task<IEnumerable<ChurchListItemView>> SearchChurchesAsync(string? keyword, int? dioceseId, Guid? userId)
        {
            return await ExecuteAsync(async connection =>
                await connection.QueryAsync<ChurchListItemView>($@"
                    SELECT
                        c.Id,
                        c.Name,
                        c.Thumbnail,
                        c.Website,
                        c.Address,
                        c.DioceseId,
                        d.Name AS DioceseName,
                        c.Latitude,
                        c.Longitude,
                        CASE
                            WHEN @UserId IS NULL THEN CAST(0 AS bit)
                            WHEN EXISTS
                            (
                                SELECT 1
                                FROM [{_schemaName.Schema}].[{TableTopicChurch.UserChurch}] uc
                                WHERE uc.UserId = @UserId
                                  AND uc.ChurchId = c.Id
                            ) THEN CAST(1 AS bit)
                            ELSE CAST(0 AS bit)
                        END AS IsFollowed
                    FROM [{_schemaName.Schema}].[{TableTopicChurch.Church}] c
                    LEFT JOIN [{_schemaName.Schema}].[{TableTopicChurch.Diocese}] d
                        ON d.Id = c.DioceseId
                    WHERE c.IsDeleted = 0
                      AND (@DioceseId IS NULL OR c.DioceseId = @DioceseId)
                      AND
                      (
                          @Keyword IS NULL
                          OR @Keyword = ''
                          OR c.Name LIKE '%' + @Keyword + '%'
                          OR c.Address LIKE '%' + @Keyword + '%'
                          OR d.Name LIKE '%' + @Keyword + '%'
                      )
                    ORDER BY c.Name ASC
                ", new
                {
                    Keyword = keyword?.Trim(),
                    DioceseId = dioceseId,
                    UserId = userId
                })
            );
        }

        public async Task<bool> IsFollowingChurchAsync(Guid userId, int churchId)
        {
            return await ExecuteAsync(async connection =>
                await connection.ExecuteScalarAsync<int>($@"
                    IF EXISTS
                    (
                        SELECT 1
                        FROM [{_schemaName.Schema}].[{TableTopicChurch.UserChurch}]
                        WHERE UserId = @UserId
                          AND ChurchId = @ChurchId
                    )
                        SELECT 1
                    ELSE
                        SELECT 0
                ", new { UserId = userId, ChurchId = churchId }) > 0
            );
        }

        public async Task<bool> FollowChurchAsync(Guid userId, int churchId)
        {
            return await ExecuteAsync(async connection =>
                await connection.ExecuteScalarAsync<int>($@"
                    IF EXISTS
                    (
                        SELECT 1
                        FROM [{_schemaName.Schema}].[{TableTopicChurch.UserChurch}]
                        WHERE UserId = @UserId
                          AND ChurchId = @ChurchId
                    )
                        SELECT 0
                    ELSE
                    BEGIN
                        INSERT INTO [{_schemaName.Schema}].[{TableTopicChurch.UserChurch}] (UserId, ChurchId)
                        VALUES (@UserId, @ChurchId);

                        SELECT 1;
                    END
                ", new { UserId = userId, ChurchId = churchId }) > 0
            );
        }

        public async Task<bool> UnfollowChurchAsync(Guid userId, int churchId)
        {
            return await ExecuteAsync(async connection =>
                await connection.ExecuteAsync($@"
                    DELETE FROM [{_schemaName.Schema}].[{TableTopicChurch.UserChurch}]
                    WHERE UserId = @UserId
                      AND ChurchId = @ChurchId
                ", new { UserId = userId, ChurchId = churchId }) > 0
            );
        }

        public async Task<IEnumerable<ChurchListItemView>> GetFollowedChurchesAsync(Guid userId)
        {
            return await ExecuteAsync(async connection =>
                await connection.QueryAsync<ChurchListItemView>($@"
                    SELECT
                        c.Id,
                        c.Name,
                        c.Thumbnail,
                        c.Website,
                        c.Address,
                        c.DioceseId,
                        d.Name AS DioceseName,
                        c.Latitude,
                        c.Longitude,
                        CAST(1 AS bit) AS IsFollowed
                    FROM [{_schemaName.Schema}].[{TableTopicChurch.UserChurch}] uc
                    INNER JOIN [{_schemaName.Schema}].[{TableTopicChurch.Church}] c
                        ON c.Id = uc.ChurchId
                       AND c.IsDeleted = 0
                    LEFT JOIN [{_schemaName.Schema}].[{TableTopicChurch.Diocese}] d
                        ON d.Id = c.DioceseId
                    WHERE uc.UserId = @UserId
                    ORDER BY c.Name ASC
                ", new { UserId = userId })
            );
        }
        #endregion

        #region Personalized mass schedules
        public async Task<IEnumerable<PersonalizedMassScheduleView>> GetPersonalizedMassSchedulesAsync(Guid userId, DateTime fromDate, DateTime toDate, int? churchId)
        {
            return await ExecuteAsync(async connection =>
                await connection.QueryAsync<PersonalizedMassScheduleView>($@"
                    SELECT
                        ms.Id AS MassScheduleId,
                        ms.ChurchId,
                        c.Name AS ChurchName,
                        c.Address AS ChurchAddress,
                        ms.IsFixed,
                        CAST(ms.[Date] AS datetime2) AS [Date],
                        CAST(ms.FromDate AS datetime2) AS FromDate,
                        CAST(ms.ToDate AS datetime2) AS ToDate,
                        CAST(ms.[Time] AS time) AS [Time],
                        ms.MassTypeId,
                        mt.Name AS MassTypeName
                    FROM [{_schemaName.Schema}].[{TableTopicChurch.MassSchedule}] ms
                    INNER JOIN [{_schemaName.Schema}].[{TableTopicChurch.Church}] c
                        ON c.Id = ms.ChurchId
                       AND c.IsDeleted = 0
                    INNER JOIN [{_schemaName.Schema}].[{TableTopicChurch.UserChurch}] uc
                        ON uc.ChurchId = c.Id
                       AND uc.UserId = @UserId
                    LEFT JOIN [{_schemaName.Schema}].[{TableTopicChurch.MassType}] mt
                        ON mt.Id = ms.MassTypeId
                    WHERE ms.IsDeleted = 0
                      AND (@ChurchId IS NULL OR ms.ChurchId = @ChurchId)
                      AND
                      (
                          (ms.[Date] IS NOT NULL AND ms.[Date] BETWEEN @FromDate AND @ToDate)
                          OR
                          (
                              ms.[Date] IS NULL
                              AND (ms.FromDate IS NULL OR ms.FromDate <= @ToDate)
                              AND (ms.ToDate IS NULL OR ms.ToDate >= @FromDate)
                          )
                      )
                    ORDER BY COALESCE(ms.[Date], ms.FromDate, ms.ToDate), ms.[Time]
                ", new
                {
                    UserId = userId,
                    FromDate = fromDate.Date,
                    ToDate = toDate.Date,
                    ChurchId = churchId
                })
            );
        }
        #endregion

        #region Reminder setting
        public async Task<ReminderSettingView> GetReminderSettingAsync(Guid userId)
        {
            return await ExecuteAsync(async connection =>
            {
                using var multi = await connection.QueryMultipleAsync($@"
                    SELECT TOP 1 MassReminder
                    FROM [{_schemaName.Schema}].[{TableTopicChurch.NotificationPreference}]
                    WHERE UserId = @UserId
                    ORDER BY Id DESC;

                    SELECT TOP 1 MinutesBefore, SpeechGender, SpeechSpeed
                    FROM [{_schemaName.Schema}].[{TableTopicChurch.ReminderSetting}]
                    WHERE UserId = @UserId
                    ORDER BY Id DESC;
                ", new { UserId = userId });

                var massReminder = await multi.ReadSingleOrDefaultAsync<bool?>() ?? false;
                var setting = await multi.ReadSingleOrDefaultAsync<ReminderSettingView>() ?? new ReminderSettingView
                {
                    MinutesBefore = DefaultReminderMinutes,
                    SpeechGender = null,
                    SpeechSpeed = null
                };

                setting.MassReminderEnabled = massReminder;
                if (setting.MinutesBefore <= 0)
                {
                    setting.MinutesBefore = DefaultReminderMinutes;
                }

                return setting;
            });
        }

        public async Task<ReminderSettingView> SaveReminderSettingAsync(Guid userId, bool isEnabled, int minutesBefore, string? speechGender, double? speechSpeed)
        {
            return await ExecuteAsync(async connection =>
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using var transaction = connection.BeginTransaction();
                try
                {
                    var preferenceId = await connection.ExecuteScalarAsync<int?>($@"
                        SELECT TOP 1 Id
                        FROM [{_schemaName.Schema}].[{TableTopicChurch.NotificationPreference}]
                        WHERE UserId = @UserId
                        ORDER BY Id DESC
                    ", new { UserId = userId }, transaction);

                    if (preferenceId.HasValue)
                    {
                        await connection.ExecuteAsync($@"
                            UPDATE [{_schemaName.Schema}].[{TableTopicChurch.NotificationPreference}]
                            SET MassReminder = @MassReminder
                            WHERE Id = @Id
                        ", new
                        {
                            Id = preferenceId.Value,
                            MassReminder = isEnabled
                        }, transaction);
                    }
                    else
                    {
                        await connection.ExecuteAsync($@"
                            INSERT INTO [{_schemaName.Schema}].[{TableTopicChurch.NotificationPreference}]
                            (
                                UserId,
                                MassReminder,
                                FeastReminder,
                                DailyWord,
                                EventUpdates,
                                FriendRequests,
                                Messages
                            )
                            VALUES
                            (
                                @UserId,
                                @MassReminder,
                                0,
                                0,
                                0,
                                0,
                                0
                            )
                        ", new
                        {
                            UserId = userId,
                            MassReminder = isEnabled
                        }, transaction);
                    }

                    var reminderSettingId = await connection.ExecuteScalarAsync<int?>($@"
                        SELECT TOP 1 Id
                        FROM [{_schemaName.Schema}].[{TableTopicChurch.ReminderSetting}]
                        WHERE UserId = @UserId
                        ORDER BY Id DESC
                    ", new { UserId = userId }, transaction);

                    var normalizedMinutes = minutesBefore > 0 ? minutesBefore : DefaultReminderMinutes;

                    if (reminderSettingId.HasValue)
                    {
                        await connection.ExecuteAsync($@"
                            UPDATE [{_schemaName.Schema}].[{TableTopicChurch.ReminderSetting}]
                            SET
                                MinutesBefore = @MinutesBefore,
                                SpeechGender = @SpeechGender,
                                SpeechSpeed = @SpeechSpeed
                            WHERE Id = @Id
                        ", new
                        {
                            Id = reminderSettingId.Value,
                            MinutesBefore = normalizedMinutes,
                            SpeechGender = speechGender,
                            SpeechSpeed = speechSpeed
                        }, transaction);
                    }
                    else
                    {
                        await connection.ExecuteAsync($@"
                            INSERT INTO [{_schemaName.Schema}].[{TableTopicChurch.ReminderSetting}]
                            (
                                UserId,
                                MinutesBefore,
                                SpeechGender,
                                SpeechSpeed
                            )
                            VALUES
                            (
                                @UserId,
                                @MinutesBefore,
                                @SpeechGender,
                                @SpeechSpeed
                            )
                        ", new
                        {
                            UserId = userId,
                            MinutesBefore = normalizedMinutes,
                            SpeechGender = speechGender,
                            SpeechSpeed = speechSpeed
                        }, transaction);
                    }

                    transaction.Commit();

                    return new ReminderSettingView
                    {
                        MassReminderEnabled = isEnabled,
                        MinutesBefore = normalizedMinutes,
                        SpeechGender = speechGender,
                        SpeechSpeed = speechSpeed
                    };
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            });
        }
        #endregion
    }

    public static class TableTopicChurch
    {
        public const string Church = "Church";
        public const string MassSchedule = "MassSchedule";
        public const string LiveStream = "LiveStream";
        public const string Diocese = "Diocese";
        public const string MassType = "MassType";
        public const string UserChurch = "UserChurch";
        public const string NotificationPreference = "NotificationPreference";
        public const string ReminderSetting = "ReminderSetting";
    }
}
