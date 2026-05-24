using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities.masslive;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.graphql.DataLoaders.churches
{
    public static partial class MassScheduleDataLoader
    {
        [DataLoader]
        public static async Task<Dictionary<int, MassSchedule[]>> GetMassSchedules(
            IReadOnlyList<int> churchIds,
            [Service] IDbConnectionFactory connectionFactory,
            CancellationToken cancellation
        )
        {
            using var connection = connectionFactory.CreateConnection();

            var sql = @"Select * from [jcodepro_journey_of_faith].[MassSchedule] where ChurchId in @Ids";
            var result = await connection.QueryAsync<MassSchedule>( sql, new {Ids = churchIds.Distinct().ToArray()} );

            return result.GroupBy(e => e.ChurchId)
                    .ToDictionary(g => g.Key, g => g.ToArray());
        }
    }
}
