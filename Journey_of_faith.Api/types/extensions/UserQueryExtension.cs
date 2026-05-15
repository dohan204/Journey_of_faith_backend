using Journey_of_faith.Api.cache;
using Journey_of_faith.Api.types.filter;
using Journey_of_faith.Api.types.resolvers;
using Journey_of_faith.Application.exceptions;
using Journey_of_faith.Application.usecases.users.queries;
using Journey_of_faith.Domain.entities;
using Journey_of_faith.Domain.entities.events;
using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Domain.entities.notifications;
using Journey_of_faith.Domain.entities.prayer;
using Journey_of_faith.Domain.entities.quiz;
using Journey_of_faith.Infrastructure.context;
using MediatR;

namespace Journey_of_faith.Api.types.extensions;

[ExtendObjectType(typeof(Query))]
public class UserQueryResolver
{
    public async Task<User?> GetDetailsUser(
        string id,
        [Service] UserCacheDataLoader cacheUser,
        CancellationToken cancellationToken
    )
    {
        return await cacheUser.LoadAsync(Guid.Parse(id), cancellationToken) ?? throw new NotFoundException("Không tìm thấy user");
    }
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering(typeof(UserFilterType))]
    public async Task<IEnumerable<User>> GetUsersAsync([Service] IMediator mediator)
    {
        return await mediator.Send(new GetUsersQuery());
    }
}


[ExtendObjectType(typeof(User))]
public partial class UserNode
{
    public async Task<EventComment[]> GetEventCommentsAsync(
        [Parent] User user,
        [Service] EventCommentByUserIdDataLoader dataLoader,
        CancellationToken cancellationToken
    )
    {
        return await dataLoader.LoadAsync(user.Id, cancellationToken) ?? [];
    }

    public async Task<EventFollower[]> GetEventFollowersAsync(
        [Parent] User user,
        [Service] EventFollowerByUserIdDataLoader dataLoader,
        CancellationToken cancellationToken
    )
    {
        return await dataLoader.LoadAsync(user.Id, cancellationToken) ?? [];
    }

    public async Task<QuizAttempt[]> GetQuizAttemptAsync(
        [Parent] User user,
        [Service] QuizAttemptByUserIdDataLoader dataLoader,
        CancellationToken cancellationToken
    )
    {
        return await dataLoader.LoadAsync(user.Id, cancellationToken) ?? [];
    }

    public async Task<ReminderSetting[]> GetReminderSettingsAsync(
        [Parent] User user,
        [Service] ReminderSettingByUserIdDataLoader dataLoader,
        CancellationToken cancellationToken
    )
    {
        return await dataLoader.LoadAsync(user.Id, cancellationToken) ?? [];
    }

    public async Task<PrayerComment[]> GetPrayerCommentsAsync(
        [Parent] User user,
        [Service] PrayCommentByUserIdDataLoader dataLoader,
        CancellationToken cancellationToken
    )
    {
        return await dataLoader.LoadAsync(user.Id, cancellationToken) ?? [];
    }

}

public class EventCommentByUserIdDataLoader : MappingOneToManyBatchDataLoader<Guid, EventComment>
{
    public EventCommentByUserIdDataLoader(
        IServiceProvider serviceProvider,
        IBatchScheduler batchScheduler,
        DataLoaderOptions options
    ) : base(
        serviceProvider,
        sql: "SELECT * FROM [jcodepro_journey_of_faith].[EventComment] WHERE UserId IN @Ids",
        selector: (EventComment ec) => ec.UserId,
        batchScheduler,
        options
    ) { }
}

public class ReminderSettingByUserIdDataLoader : MappingOneToManyBatchDataLoader<Guid, ReminderSetting>
{
    public ReminderSettingByUserIdDataLoader(
        IServiceProvider serviceProvider,
        IBatchScheduler batchScheduler,
        DataLoaderOptions options
    ) : base(
        serviceProvider,
        sql: "SELECT * FROM [jcodepro_journey_of_faith].[ReminderSetting] WHERE UserId IN @Ids",
        selector: (ReminderSetting rs) => rs.UserId,
        batchScheduler,
        options
    ) { }
}

public class EventFollowerByUserIdDataLoader : MappingOneToManyBatchDataLoader<Guid, EventFollower>
{
    public EventFollowerByUserIdDataLoader(
        IServiceProvider serviceProvider,
        IBatchScheduler batchScheduler,
        DataLoaderOptions options
    ) : base(
        serviceProvider,
        sql: "SELECT * FROM [jcodepro_journey_of_faith].[EventFollower] WHERE UserId IN @Ids",
        selector: (EventFollower ef) => ef.UserId,
        batchScheduler,
        options
    ) { }
}


public class QuizAttemptByUserIdDataLoader : MappingOneToManyBatchDataLoader<Guid, QuizAttempt>
{
    public QuizAttemptByUserIdDataLoader(
        IServiceProvider serviceProvider,
        IBatchScheduler batchScheduler,
        DataLoaderOptions options
    ) : base(
        serviceProvider,
        sql: "SELECT * FROM [jcodepro_journey_of_faith].[QuizAttempt] WHERE UserId IN @Ids",
        selector: (QuizAttempt qa) => qa.UserId,
        batchScheduler,
        options
    ) { }
}

public class PrayCommentByUserIdDataLoader : MappingOneToManyBatchDataLoader<Guid, PrayerComment>
{
    public PrayCommentByUserIdDataLoader(
        IServiceProvider serviceProvider,
        IBatchScheduler batchScheduler,
        DataLoaderOptions options
    ) : base(
        serviceProvider,
        sql: "SELECT * FROM [jcodepro_journey_of_faith].[PrayerComment] WHERE UserId IN @Ids",
        selector: (PrayerComment p) => p.UserId,
        batchScheduler,
        options
    ) { }
}