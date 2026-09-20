using System.Data;
using Dapper;
using Journey_of_faith.Application.common.dtos.church;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Application.usecases.churchs;
using Journey_of_faith.Domain.dtos;
using Journey_of_faith.Domain.entities.catholic;
using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Domain.interfaces;
using Journey_of_faith.Infrastructure.context;
using Journey_of_faith.Infrastructure.common;
using Journey_of_faith.Infrastructure.repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Journey_of_faith.Infrastructure.persistence.queries;

public sealed class ChurchQueries : BaseRepository, IChurchQueries
{
    private const int DefaultReminderMinutes = 30;
    private readonly ApplicationDbContext _dbContext;
    private readonly string _schema;

    public ChurchQueries(
        IDbConnectionFactory factory,
        ApplicationDbContext dbContext,
        IOptions<TableSchemaName> options)
        : base(factory)
    {
        _dbContext = dbContext;
        _schema = options.Value.Schema;
    }

    public async Task<ChurchViewDto?> GetChurchByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var church = await _dbContext.Churches
            .AsNoTracking()
            .Include(church => church.Diocese)
            .Include(church => church.MassSchedules)
                .ThenInclude(schedule => schedule.MassType)
            .Include(church => church.MassSchedules)
                .ThenInclude(schedule => schedule.Liturgy)
            .Include(church => church.ChurchImages)
            .FirstOrDefaultAsync(
                church => church.Id == id && church.IsDeleted != true,
                cancellationToken);

        return church is null ? null : MapChurch(church);
    }

    public async Task<PagedResult<ChurchViewDto>> GetChurchesAsync(
        int page,
        int pageSize,
        string? search,
        CancellationToken cancellationToken = default)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Max(pageSize, 1);

        var query = _dbContext.Churches
            .AsNoTracking()
            .Where(church => church.IsDeleted != true);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var keyword = search.Trim();
            query = query.Where(church => church.Name.Contains(keyword));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var data = await query
            .Include(church => church.Diocese)
            .Include(church => church.MassSchedules)
                .ThenInclude(schedule => schedule.MassType)
            .Include(church => church.MassSchedules)
                .ThenInclude(schedule => schedule.Liturgy)
            .Include(church => church.ChurchImages)
            .OrderBy(church => church.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<ChurchViewDto>
        {
            Data = data.Select(church => MapChurch(church)).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<LiturgyViewDto?> GetLiturgyTodayAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        return await _dbContext.Liturgy
            .AsNoTracking()
            .Where(liturgy => liturgy.DateActive >= today && liturgy.DateActive < tomorrow)
            .Select(liturgy => new LiturgyViewDto
            {
                Id = liturgy.Id,
                MassScheduleId = liturgy.MassScheduleId,
                ReadingOne = liturgy.ReadingOne,
                ResponsorialPsalm = liturgy.ResponsorialPsalm,
                Gospel = liturgy.GoodNew,
                EndWord = liturgy.EndWord,
                DateActive = liturgy.DateActive
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<MassScheduleTodayDto>> GetMassScheduleTodayViewsAsync(
        CancellationToken cancellationToken = default)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        return await _dbContext.MassSchedules
            .AsNoTracking()
            .Where(schedule => schedule.Date == today && schedule.IsDeleted != true)
            .Select(schedule => new MassScheduleTodayDto
            {
                Time = schedule.Time,
                MassName = schedule.Name,
                ChurchName = schedule.Church.Name,
                Description = schedule.Church.Description
            })
            .ToListAsync(cancellationToken);
    }

    public Task<bool> DioceseExistsAsync(
        int dioceseId,
        CancellationToken cancellationToken = default)
        => _dbContext.Dioceses
            .AsNoTracking()
            .AnyAsync(diocese => diocese.Id == dioceseId && diocese.IsDeleted != true, cancellationToken);

    public Task<bool> DioceseNameExistsAsync(
        string name,
        CancellationToken cancellationToken = default)
        => _dbContext.Dioceses
            .AsNoTracking()
            .AnyAsync(diocese => diocese.Name == name && diocese.IsDeleted != true, cancellationToken);

    public async Task<DioceseViewDto?> GetDioceseByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Dioceses
            .AsNoTracking()
            .Where(diocese => diocese.Id == id && diocese.IsDeleted != true)
            .Select(diocese => new DioceseViewDto
            {
                Id = diocese.Id,
                Name = diocese.Name,
                Website = diocese.Website,
                Address = diocese.Address,
                Thumbnail = diocese.Thumbnail,
                ChurchCount = diocese.Churches.Count(church => church.IsDeleted != true)
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<DioceseViewDto>> GetAllDiocesesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Dioceses
            .AsNoTracking()
            .Where(diocese => diocese.IsDeleted != true)
            .OrderBy(diocese => diocese.Name)
            .Select(diocese => new DioceseViewDto
            {
                Id = diocese.Id,
                Name = diocese.Name,
                Website = diocese.Website,
                Address = diocese.Address,
                Thumbnail = diocese.Thumbnail,
                ChurchCount = diocese.Churches.Count(church => church.IsDeleted != true)
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ChurchListItemDto>> SearchChurchesAsync(
        string? keyword,
        int? dioceseId,
        Guid? userId,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Churches
            .AsNoTracking()
            .Include(church => church.Diocese)
            .Include(church => church.MassSchedules)
                .ThenInclude(schedule => schedule.MassType)
            .Include(church => church.UserChurches)
            .Where(church => church.IsDeleted != true);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var normalizedKeyword = keyword.Trim();
            query = query.Where(church =>
                church.Name.Contains(normalizedKeyword) ||
                (church.Address != null && church.Address.Contains(normalizedKeyword)));
        }

        if (dioceseId.HasValue)
        {
            query = query.Where(church => church.DioceseId == dioceseId.Value);
        }

        var churches = await query
            .OrderBy(church => church.Name)
            .ToListAsync(cancellationToken);

        return churches.Select(church => new ChurchListItemDto
        {
            Id = church.Id,
            Name = church.Name,
            Thumbnail = church.Thumbnail,
            Email = church.Email,
            Address = church.Address,
            DioceseId = church.DioceseId,
            DioceseName = church.Diocese?.Name,
            Latitude = church.GeoLocation?.Latitude,
            Longitude = church.GeoLocation?.Longitude,
            IsFollowed = userId.HasValue && church.UserChurches.Any(link => link.UserId == userId.Value),
            MassSchedules = church.MassSchedules.Select(schedule => new MassScheduleViewDto
            {
                Id = schedule.Id,
                ChurchId = church.Id,
                MassName = schedule.Name,
                ChurchName = church.Name,
                IsFixed = schedule.IsFixed,
                Date = schedule.Date?.ToDateTime(TimeOnly.MinValue),
                FromDate = schedule.FromDate,
                ToDate = schedule.ToDate,
                Time = schedule.Time,
                MassTypeId = schedule.MassTypeId,
                MassTypeName = schedule.MassType?.Name
            }).ToList()
        });
    }

    public Task<bool> ChurchExistsAsync(
        int churchId,
        CancellationToken cancellationToken = default)
        => _dbContext.Churches
            .AsNoTracking()
            .AnyAsync(church => church.Id == churchId && church.IsDeleted != true, cancellationToken);

    public Task<bool> IsFollowingChurchAsync(
        Guid userId,
        int churchId,
        CancellationToken cancellationToken = default)
        => _dbContext.UserChurches
            .AsNoTracking()
            .AnyAsync(
                link => link.UserId == userId && link.ChurchId == churchId,
                cancellationToken);

    public async Task<IEnumerable<ChurchViewDto>> GetFollowedChurchesAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var churches = await _dbContext.UserChurches
            .AsNoTracking()
            .Where(link => link.UserId == userId && link.Church.IsDeleted != true)
            .Select(link => link.Church)
            .Include(church => church.Diocese)
            .Include(church => church.MassSchedules)
                .ThenInclude(schedule => schedule.MassType)
            .Include(church => church.MassSchedules)
                .ThenInclude(schedule => schedule.Liturgy)
            .Include(church => church.ChurchImages)
            .ToListAsync(cancellationToken);

        return churches.Select(church => MapChurch(church, true));
    }

    public async Task<ReminderSettingDto> GetReminderSettingAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await QueryAsync(async connection =>
        {
            var command = new CommandDefinition(
                $"""
                SELECT TOP 1 MassReminder
                FROM [{_schema}].[{TableTopicChurch.NotificationPreference}]
                WHERE UserId = @UserId
                ORDER BY Id DESC;

                SELECT TOP 1 MinutesBefore, SpeechGender, SpeechSpeed
                FROM [{_schema}].[{TableTopicChurch.ReminderSetting}]
                WHERE UserId = @UserId
                ORDER BY Id DESC;
                """,
                new { UserId = userId },
                cancellationToken: cancellationToken);

            using var multi = await connection.QueryMultipleAsync(command);
            var massReminder = await multi.ReadSingleOrDefaultAsync<bool?>() ?? false;
            var setting = await multi.ReadSingleOrDefaultAsync<ReminderSettingDto>()
                ?? new ReminderSettingDto { MinutesBefore = DefaultReminderMinutes };

            setting.MassReminderEnabled = massReminder;
            if (setting.MinutesBefore <= 0) setting.MinutesBefore = DefaultReminderMinutes;
            return setting;
        });
    }

    public async Task<DailyWordViewDto?> GetDailyWordAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        return await _dbContext.DailyWords
            .AsNoTracking()
            .Where(word => word.Date >= today && word.Date < tomorrow && word.IsDeleted != true)
            .OrderByDescending(word => word.Id)
            .Select(word => new DailyWordViewDto
            {
                Id = word.Id,
                Date = word.Date,
                Title = word.Title,
                BibleContent = word.BibleContent,
                Gospel = word.Gospel,
                IsShortWord = word.IsShortWord
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResult<ChurchViewDto>> GetListAsync(QueryFilter filter)
    {
        return await QueryAsync(async connection =>
        {
            var page = Math.Max(filter.Page ?? 1, 1);
            var pageSize = Math.Max(filter.PageSize ?? 25, 1);
            var churches = (await connection.QueryAsync<ChurchViewDto>(
                "spSearchChurch",
                new
                {
                    ChurchName = filter.ChurchName,
                    Province = filter.Province,
                    Ward = filter.Ward,
                    Page = page,
                    PageSize = pageSize
                },
                commandType: CommandType.StoredProcedure)).ToList();

            var totalCount = await connection.ExecuteScalarAsync<int>(
                $"SELECT COUNT(*) FROM [{_schema}].[{TableTopicChurch.Church}] WHERE IsDeleted = 0");

            var churchIds = churches.Select(church => church.Id).ToArray();
            if (churchIds.Length == 0)
            {
                return new PagedResult<ChurchViewDto>
                {
                    Data = churches,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = totalCount
                };
            }

            var massSchedules = (await connection.QueryAsync<MassScheduleViewDto>(
                $"""
                SELECT Id, ChurchId, Name AS MassName, Time
                FROM [{_schema}].[{TableTopicChurch.MassSchedule}]
                WHERE ChurchId IN @ChurchIds
                  AND (@Time IS NULL OR [Time] LIKE '%' + @Time + '%')
                """,
                new
                {
                    ChurchIds = churchIds,
                    Time = string.IsNullOrWhiteSpace(filter.Time) ? null : filter.Time
                })).ToList();

            var scheduleIds = massSchedules.Select(schedule => schedule.Id).ToArray();
            var liturgies = scheduleIds.Length == 0
                ? Enumerable.Empty<LiturgyViewDto>().ToLookup(item => item.MassScheduleId)
                : (await connection.QueryAsync<LiturgyViewDto>(
                    $"""
                    SELECT Id, MassScheduleId, ReadingOne, ResponsorialPsalm, GoodNew AS Gospel, EndWord, DateActive
                    FROM [{_schema}].[{TableTopicChurch.Liturgy}]
                    WHERE MassScheduleId IN @MassScheduleIds
                    """,
                    new { MassScheduleIds = scheduleIds })).ToLookup(item => item.MassScheduleId);

            foreach (var schedule in massSchedules)
            {
                schedule.Liturgy = liturgies[schedule.Id].FirstOrDefault() ?? new LiturgyViewDto();
            }

            var images = (await connection.QueryAsync<ChurchImageViewDto>(
                $"""
                SELECT Id, ChurchId, ImageName
                FROM [{_schema}].[{TableTopicChurch.ChurchImages}]
                WHERE ChurchId IN @ChurchIds
                """,
                new { ChurchIds = churchIds })).ToLookup(image => image.ChurchId);
            var schedules = massSchedules.ToLookup(schedule => schedule.ChurchId);

            foreach (var church in churches)
            {
                church.MassSchedules = schedules[church.Id].ToList();
                church.ChurchImages = images[church.Id].ToList();
            }

            return new PagedResult<ChurchViewDto>
            {
                Data = churches,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        });
    }

    public async Task<IEnumerable<MassScheduleViewDto>> GetMassScheduleToday()
    {
        return await QueryAsync(async connection =>
        {
            return await connection.QueryAsync<MassScheduleViewDto>(
                $"""
                SELECT m.Id, m.ChurchId, m.Name AS MassName, m.Time, c.Name AS ChurchName
                FROM [{_schema}].[{TableTopicChurch.MassSchedule}] m
                INNER JOIN [{_schema}].[{TableTopicChurch.Church}] c ON m.ChurchId = c.Id
                WHERE m.Date = @CurrentDate
                """,
                new { CurrentDate = DateTime.Today });
        });
    }

    public async Task<IEnumerable<ChurchViewDto>> GetListFollowedAsync(Guid id)
    {
        return await QueryAsync(async connection =>
        {
            return await connection.QueryAsync<ChurchViewDto>(
                $"""
                SELECT c.Id, c.Name AS ChurchName, c.Email, c.Address, c.Boss, c.Description,
                       c.Latitude, c.Longitude
                FROM [{_schema}].[{TableTopicChurch.Church}] c
                INNER JOIN [{_schema}].[{TableTopicChurch.UserChurch}] uc ON uc.ChurchId = c.Id
                WHERE uc.UserId = @UserId AND c.IsDeleted = 0
                """,
                new { UserId = id });
        });
    }

    private static ChurchViewDto MapChurch(Church church, bool isFollowed = false)
    {
        return new ChurchViewDto
        {
            Id = church.Id,
            ChurchName = church.Name,
            Thumbnail = church.Thumbnail,
            Email = church.Email ?? string.Empty,
            Address = church.Address ?? string.Empty,
            DioceseId = church.DioceseId,
            DioceseName = church.Diocese?.Name,
            Boss = church.Boss ?? string.Empty,
            Description = church.Description ?? string.Empty,
            Latitude = church.GeoLocation?.Latitude ?? 0,
            Longitude = church.GeoLocation?.Longitude ?? 0,
            IsFollowed = isFollowed,
            MassSchedules = church.MassSchedules.Select(schedule => new MassScheduleViewDto
            {
                Id = schedule.Id,
                ChurchId = schedule.ChurchId,
                ChurchName = church.Name,
                MassName = schedule.Name,
                Time = schedule.Time,
                Date = schedule.Date?.ToDateTime(TimeOnly.MinValue),
                FromDate = schedule.FromDate,
                ToDate = schedule.ToDate,
                IsFixed = schedule.IsFixed,
                MassTypeId = schedule.MassTypeId,
                MassTypeName = schedule.MassType?.Name,
                Liturgy = schedule.Liturgy is null
                    ? new LiturgyViewDto()
                    : new LiturgyViewDto
                    {
                        Id = schedule.Liturgy.Id,
                        MassScheduleId = schedule.Liturgy.MassScheduleId,
                        ReadingOne = schedule.Liturgy.ReadingOne,
                        ResponsorialPsalm = schedule.Liturgy.ResponsorialPsalm,
                        Gospel = schedule.Liturgy.GoodNew,
                        EndWord = schedule.Liturgy.EndWord,
                        DateActive = schedule.Liturgy.DateActive
                    }
            }).ToList(),
            ChurchImages = church.ChurchImages.Select(image => new ChurchImageViewDto
            {
                Id = image.Id,
                ChurchId = image.ChurchId,
                ImageName = image.ImageName
            }).ToList()
        };
    }
}
