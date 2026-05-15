using System.Data;
using System.Security.Principal;
using Dapper;
using GreenDonut; 
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities;
using Journey_of_faith.Domain.interfaces;
using Journey_of_faith.Infrastructure.persistence.entities.events;
using Journey_of_faith.Infrastructure.persistence.entities.faith_notifications;
using Journey_of_faith.Infrastructure.persistence.entities.location;
using Journey_of_faith.Infrastructure.persistence.entities.quiz;
using Journey_of_faith.Infrastructure.repositories;


namespace Journey_of_faith.Api.types.data;


internal static class  UserDataLoader
{
    private const string Schema = "jcodepro_journey_of_faith";
    [DataLoader]
    public static async Task<Dictionary<Guid, Church[]>> GetChurchByUserIdAsync(
        IReadOnlyList<Guid> userIds,
        GetDataRepository<Guid,Church> dataLoader,
        CancellationToken cancellationToken
    )
    {
        const string sql = $"Select * from [{Schema}].[Church] where UserId in @Ids";
        return await dataLoader.GetDataByIdsAsync(sql, ids: userIds, 
        (Church c ) => c.UserId, cancellationToken);
    }

    [DataLoader]
    public static async Task<Dictionary<Guid, EventComment[]>> GetEventCommentByUserIdAsync(
        IReadOnlyList<Guid> userIds,
        GetDataRepository<Guid, EventComment> dataLoader,
        CancellationToken cancellationToken
    )
    {
        const string sql = $"Select * from [{Schema}].[EventComment] where UserId in @Ids";
        return await dataLoader.GetDataByIdsAsync(sql, ids: userIds, 
        (EventComment ec) => ec.UserId, cancellationToken);
    }


    [DataLoader]
    public static async Task<Dictionary<Guid, EventFollower[]>> GetEventFollowerByUserIdAsync(
        IReadOnlyList<Guid> userIds,
        GetDataRepository<Guid, EventFollower> dataLoader,
        CancellationToken cancellationToken
    )
    {
        const string sql = $"Select * from [{Schema}].[EventFollower] where UserId in @Ids";
        return await dataLoader.GetDataByIdsAsync(sql, ids: userIds, 
        (EventFollower ef) => ef.UserId, cancellationToken);
    }

    [DataLoader]
    public static async Task<Dictionary<Guid, QuizAttempt>> GetQuizAttemptByUserIdAsync(
        IReadOnlyList<Guid> userIds,
        GetDataRepository<Guid, QuizAttempt> dataLoader,
        CancellationToken cancellationToken
    )
    {
        const string sql = $"Select * from [{Schema}].[QuizAttempt] where UserId in @Ids";
        return await dataLoader.GetOneToOneDataAsync(sql, ids: userIds, 
        (QuizAttempt qa) => qa.UserId, cancellationToken);
    }
    [DataLoader]
    public static async Task<Dictionary<Guid, ReminderSetting>> GetReminderSettingByUserIdAsync(
        IReadOnlyList<Guid> userIds,
        GetDataRepository<Guid, ReminderSetting> dataLoader,
        CancellationToken cancellationToken
    )
    {
        const string sql = $"Select * from [{Schema}].[ReminderSetting] where UserId in @Ids";
        return await dataLoader.GetOneToOneDataAsync(sql, ids: userIds, 
        (ReminderSetting rs) => rs.UserId, cancellationToken);
    }
    // [DataLoader]
    // public static async Task<Dictionary<Guid, Event>> GetEventByUserIdAsync(
    //     IReadOnlyList<Guid> userIds,
    //     GetDataRepository<Guid, Event> dataLoader,
    //     CancellationToken cancellationToken
    // )
    // {
    //     const string sql = $"Select * from [{Schema}].[Event] where UserId in @Ids";
    //     return await dataLoader.GetOneToOneDataAsync(sql, ids: userIds, 
    //     (Event e) => e.UserId, cancellationToken);
    // }
}
