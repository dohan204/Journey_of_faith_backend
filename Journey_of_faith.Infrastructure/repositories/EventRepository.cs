using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.interfaces;
using Journey_of_faith.Infrastructure.common;
using Microsoft.Extensions.Options;
using System.Data;

namespace Journey_of_faith.Infrastructure.repositories
{
    public class EventRepository : BaseRepository, IEventRepository
    {
        public EventRepository(IDbConnectionFactory dbConnection, IOptions<TableSchemaName> schemaName)
            : base(dbConnection, schemaName)
        {
        }

        public async Task<bool> EventExistsAsync(int eventId)
        {
            return await ExecuteAsync(async connection =>
                await connection.ExecuteScalarAsync<int>($@"
                    IF EXISTS (
                        SELECT 1
                        FROM [{_schemaName.Schema}].[{EventTables.Event}]
                        WHERE Id = @EventId AND IsDeleted = 0
                    ) SELECT 1 ELSE SELECT 0
                ", new { EventId = eventId }) > 0
            );
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await ExecuteAsync(async connection =>
                await connection.ExecuteScalarAsync<int>($@"
                    IF EXISTS (
                        SELECT 1
                        FROM [{_schemaName.Schema}].[{EventTables.EventCategory}]
                        WHERE Id = @CategoryId
                    ) SELECT 1 ELSE SELECT 0
                ", new { CategoryId = categoryId }) > 0
            );
        }

        public async Task<bool> CategoryNameExistsAsync(string categoryName)
        {
            return await ExecuteAsync(async connection =>
                await connection.ExecuteScalarAsync<int>($@"
                    IF EXISTS (
                        SELECT 1
                        FROM [{_schemaName.Schema}].[{EventTables.EventCategory}]
                        WHERE Name = @Name
                    ) SELECT 1 ELSE SELECT 0
                ", new { Name = categoryName }) > 0
            );
        }

        public async Task<int> CreateCategoryAsync(string categoryName)
        {
            return await ExecuteAsync(async connection =>
                await connection.ExecuteScalarAsync<int>($@"
                    INSERT INTO [{_schemaName.Schema}].[{EventTables.EventCategory}] (Name)
                    OUTPUT inserted.Id
                    VALUES (@Name)
                ", new { Name = categoryName })
            );
        }

        public async Task<IEnumerable<EventCategoryView>> GetCategoriesAsync()
        {
            return await ExecuteAsync(async connection =>
                await connection.QueryAsync<EventCategoryView>($@"
                    SELECT Id, Name
                    FROM [{_schemaName.Schema}].[{EventTables.EventCategory}]
                    ORDER BY Name ASC
                ")
            );
        }

        public async Task<int> CreateEventAsync(CreateEventPayload payload)
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
                    var eventId = await connection.ExecuteScalarAsync<int>($@"
                        INSERT INTO [{_schemaName.Schema}].[{EventTables.Event}]
                        (
                            Title,
                            Description,
                            Location,
                            StartDate,
                            EndDate,
                            ImageUrl,
                            CreatorUserId,
                            LastModifierUserId
                        )
                        OUTPUT inserted.Id
                        VALUES
                        (
                            @Title,
                            @Description,
                            @Location,
                            @StartDate,
                            @EndDate,
                            @ImageUrl,
                            @CreatorUserId,
                            @CreatorUserId
                        )
                    ", new
                    {
                        payload.Title,
                        payload.Description,
                        payload.Location,
                        payload.StartDate,
                        payload.EndDate,
                        payload.ImageUrl,
                        payload.CreatorUserId
                    }, transaction);

                    var categoryIds = payload.CategoryIds.Distinct().ToList();
                    foreach (var categoryId in categoryIds)
                    {
                        await connection.ExecuteAsync($@"
                            INSERT INTO [{_schemaName.Schema}].[{EventTables.EventCategoryMapping}] (EventId, CategoryId)
                            VALUES (@EventId, @CategoryId)
                        ", new { EventId = eventId, CategoryId = categoryId }, transaction);
                    }

                    var imageUrls = payload.ImageUrls
                        .Where(url => !string.IsNullOrWhiteSpace(url))
                        .Select(url => url.Trim())
                        .Distinct()
                        .ToList();

                    foreach (var imageUrl in imageUrls)
                    {
                        await connection.ExecuteAsync($@"
                            INSERT INTO [{_schemaName.Schema}].[{EventTables.EventImage}] (EventId, ImageUrl)
                            VALUES (@EventId, @ImageUrl)
                        ", new { EventId = eventId, ImageUrl = imageUrl }, transaction);
                    }

                    transaction.Commit();
                    return eventId;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            });
        }

        public async Task<bool> UpdateEventAsync(UpdateEventPayload payload)
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
                    var affectedRows = await connection.ExecuteAsync($@"
                        UPDATE [{_schemaName.Schema}].[{EventTables.Event}] SET
                            Title = @Title,
                            Description = @Description,
                            Location = @Location,
                            StartDate = @StartDate,
                            EndDate = @EndDate,
                            ImageUrl = @ImageUrl,
                            LastModifierUserId = @LastModifierUserId,
                            LastModificationTime = GETDATE()
                        WHERE Id = @Id AND IsDeleted = 0
                    ", new
                    {
                        payload.Id,
                        payload.Title,
                        payload.Description,
                        payload.Location,
                        payload.StartDate,
                        payload.EndDate,
                        payload.ImageUrl,
                        payload.LastModifierUserId
                    }, transaction);

                    if (affectedRows == 0)
                    {
                        transaction.Rollback();
                        return false;
                    }

                    if (payload.CategoryIds is not null)
                    {
                        await connection.ExecuteAsync($@"
                            DELETE FROM [{_schemaName.Schema}].[{EventTables.EventCategoryMapping}]
                            WHERE EventId = @EventId
                        ", new { EventId = payload.Id }, transaction);

                        var categoryIds = payload.CategoryIds.Distinct().ToList();
                        foreach (var categoryId in categoryIds)
                        {
                            await connection.ExecuteAsync($@"
                                INSERT INTO [{_schemaName.Schema}].[{EventTables.EventCategoryMapping}] (EventId, CategoryId)
                                VALUES (@EventId, @CategoryId)
                            ", new { EventId = payload.Id, CategoryId = categoryId }, transaction);
                        }
                    }

                    if (payload.ImageUrls is not null)
                    {
                        await connection.ExecuteAsync($@"
                            DELETE FROM [{_schemaName.Schema}].[{EventTables.EventImage}]
                            WHERE EventId = @EventId
                        ", new { EventId = payload.Id }, transaction);

                        var imageUrls = payload.ImageUrls
                            .Where(url => !string.IsNullOrWhiteSpace(url))
                            .Select(url => url.Trim())
                            .Distinct()
                            .ToList();

                        foreach (var imageUrl in imageUrls)
                        {
                            await connection.ExecuteAsync($@"
                                INSERT INTO [{_schemaName.Schema}].[{EventTables.EventImage}] (EventId, ImageUrl)
                                VALUES (@EventId, @ImageUrl)
                            ", new { EventId = payload.Id, ImageUrl = imageUrl }, transaction);
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            });
        }

        public async Task<bool> DeleteEventAsync(int eventId, Guid deleterUserId)
        {
            return await ExecuteAsync(async connection =>
                await connection.ExecuteAsync($@"
                    UPDATE [{_schemaName.Schema}].[{EventTables.Event}] SET
                        IsDeleted = 1,
                        DeleterUserId = @DeleterUserId,
                        DeletionTime = GETDATE(),
                        LastModifierUserId = @DeleterUserId,
                        LastModificationTime = GETDATE()
                    WHERE Id = @EventId AND IsDeleted = 0
                ", new { EventId = eventId, DeleterUserId = deleterUserId }) > 0
            );
        }

        public async Task<EventDetailsView?> GetEventDetailsAsync(int eventId, Guid? userId)
        {
            return await ExecuteAsync(async connection =>
            {
                using var multi = await connection.QueryMultipleAsync("sp_GetEventDetails", new { EventId = eventId, UserId = userId }, commandType: CommandType.StoredProcedure);

                var eventDetails = await multi.ReadSingleOrDefaultAsync<EventDetailsView>();
                if (eventDetails is null)
                {
                    return null;
                }

                eventDetails.Categories = (await multi.ReadAsync<EventCategoryView>()).ToList();
                eventDetails.Images = (await multi.ReadAsync<EventImageView>()).ToList();
                return eventDetails;
            });
        }

        public async Task<EventPagedResult> GetEventsAsync(EventListFilter filter, Guid? userId)
        {
            return await ExecuteAsync(async connection =>
            {
                var offset = (filter.PageIndex - 1) * filter.PageSize;

                using var multi = await connection.QueryMultipleAsync("sp_EventsPage", new
                {
                    Keyword = filter.Keyword,
                    CategoryId = filter.CategoryId,
                    StartFrom = filter.StartFrom,
                    StartTo = filter.StartTo,
                    OnlyUpcoming = filter.OnlyUpcoming,
                    UserId = userId,
                    Offset = offset,
                    PageSize = filter.PageSize
                }, commandType: CommandType.StoredProcedure);

                var items = (await multi.ReadAsync<EventListItemView>()).ToList();
                var totalCount = await multi.ReadSingleAsync<int>();

                return new EventPagedResult
                {
                    TotalCount = totalCount,
                    PageIndex = filter.PageIndex,
                    PageSize = filter.PageSize,
                    Items = items
                };
            });
        }

        public async Task<bool> IsFollowingEventAsync(Guid userId, int eventId)
        {
            return await ExecuteAsync(async connection =>
                await connection.ExecuteScalarAsync<int>($@"
                    IF EXISTS (
                        SELECT 1
                        FROM [{_schemaName.Schema}].[{EventTables.UserEvent}]
                        WHERE UserId = @UserId
                          AND EventId = @EventId
                    ) SELECT 1 ELSE SELECT 0
                ", new { UserId = userId, EventId = eventId }) > 0
            );
        }

        public async Task<bool> FollowEventAsync(Guid userId, int eventId)
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
                    var exists = await connection.ExecuteScalarAsync<int>($@"
                        IF EXISTS (
                            SELECT 1
                            FROM [{_schemaName.Schema}].[{EventTables.UserEvent}]
                            WHERE UserId = @UserId
                              AND EventId = @EventId
                        ) SELECT 1 ELSE SELECT 0
                    ", new { UserId = userId, EventId = eventId }, transaction);

                    if (exists == 1)
                    {
                        transaction.Commit();
                        return false;
                    }

                    await connection.ExecuteAsync($@"
                        INSERT INTO [{_schemaName.Schema}].[{EventTables.UserEvent}] (UserId, EventId, FollowedAt)
                        VALUES (@UserId, @EventId, GETDATE())
                    ", new { UserId = userId, EventId = eventId }, transaction);

                    await connection.ExecuteAsync($@"
                        INSERT INTO [{_schemaName.Schema}].[{EventTables.EventFollower}] (EventId, UserId, FollowedTime)
                        VALUES (@EventId, @UserId, GETUTCDATE())
                    ", new { EventId = eventId, UserId = userId }, transaction);

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            });
        }

        public async Task<bool> UnfollowEventAsync(Guid userId, int eventId)
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
                    var deletedFromUserEvent = await connection.ExecuteAsync($@"
                        DELETE FROM [{_schemaName.Schema}].[{EventTables.UserEvent}]
                        WHERE UserId = @UserId AND EventId = @EventId
                    ", new { UserId = userId, EventId = eventId }, transaction);

                    await connection.ExecuteAsync($@"
                        DELETE FROM [{_schemaName.Schema}].[{EventTables.EventFollower}]
                        WHERE UserId = @UserId AND EventId = @EventId
                    ", new { UserId = userId, EventId = eventId }, transaction);

                    transaction.Commit();
                    return deletedFromUserEvent > 0;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            });
        }

        public async Task<IEnumerable<EventListItemView>> GetFollowedEventsAsync(Guid userId, DateTime? startFrom, DateTime? startTo)
        {
            return await ExecuteAsync(async connection =>
                await connection.QueryAsync<EventListItemView>("sp_GetFollowedEvents", new
                {
                    UserId = userId,
                    StartFrom = startFrom,
                    StartTo = startTo
                }, commandType: CommandType.StoredProcedure)
            );
        }
    }

    public static class EventTables
    {
        public const string Event = "Event";
        public const string EventCategory = "EventCategory";
        public const string EventCategoryMapping = "EventCategoryMapping";
        public const string EventFollower = "EventFollower";
        public const string EventParticipant = "EventParticipant";
        public const string EventImage = "EventImage";
        public const string EventNotification = "EventNotification";
        public const string UserEvent = "UserEvent";
    }
}
