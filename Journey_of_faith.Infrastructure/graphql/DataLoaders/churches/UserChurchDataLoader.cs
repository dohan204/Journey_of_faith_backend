using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.graphql.DataLoaders.churches
{
    public static partial class UserChurchDataLoader
    {
        [DataLoader]
        public static async Task<Dictionary<int, User[]>> UserChurchByMapping(
            IReadOnlyList<int> churchIds,
            IDbConnectionFactory dbConnectionFactory,
            CancellationToken cancellationToken
        )
        {
            using var connection = dbConnectionFactory.CreateConnection();
            var sql = @"Select * from [jcodepro_journey_of_faith].[UserChurch] uc 
                        inner join [jcodepro_journey_of_faith].[User] u on uc.UserId = u.Id
                        Where uc.ChurchId in @Ids";

            var result = await connection.QueryAsync<int, User, (int churchId, User users)>(
                sql,
                (churchId, user) => (churchId, user),
                new { Ids = churchIds.Distinct().ToArray() },
                splitOn: "Id"
            );

            return result.GroupBy(e => e.churchId)
                    .ToDictionary(e => e.Key, e => e.Select(e => e.users).ToArray());
        }
    }
}
