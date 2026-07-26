using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Infrastructure.graphql.types;
using Journey_of_faith.Infrastructure.persistence.configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.graphql.DataLoaders.users;

public static partial class UserChurchsDataLoader
{
    [DataLoader]
    public static async Task<Dictionary<Guid, Church[]>> GetChurchsByUserId(
        IReadOnlyList<Guid> userIds,
        IDbConnectionFactory dbConnectionFactory,
        CancellationToken cancellationToken)
    {
        using var connection = dbConnectionFactory.CreateConnection();
        var sql = @"Select uc.UserId, c.* from [jcodepro_journey_of_faith].[UserChurch] uc
                    inner join [jcodepro_journey_of_faith].[Church] 
                    c on uc.ChurchId = c.Id where uc.UserId in @UserId";

        var result = await connection.QueryAsync<Guid, Church, (Guid UserId, Church church)>(
            sql,
            (userId, church) => (userId, church),
            new { UserId = userIds.Distinct().ToArray() },
            splitOn: "Id");

        return result
             .GroupBy(e => e.UserId)
             .ToDictionary(e => e.Key, e => e.Select(e => e.church).ToArray());
    }
}

